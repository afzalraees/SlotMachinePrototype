using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public List<Sprite> slotItems = new List<Sprite>();
    public List<Sprite> slotItemsAtEnd = new List<Sprite>();
    public RectTransform[] slot;
    public List<Image> row1, row2, row3, row4, row5;
    List<int> finalPos = new List<int>();
    bool spin;
    public Image winScreen, loseScreen;
    public Sprite bigWin, superWin, megaWin;

    public AudioSource aud;
    public AudioClip win, lose;

    public Text winAmount;


    private void Awake()
    {
        FillRows(slotItems);
        SetFinalPos();
    }
    
    void SetFinalPos()
    {
        int finalPosY = 0 ;
        for(int i = 0; i < slotItems.Count; i++)
        {
            finalPos.Add(finalPosY);
            finalPosY = finalPosY + 80;
        }
    }
    void FillRows(List<Sprite> items)
    {
        for(int i = 0; i < row1.Count; i++)
        {
            row1[i].sprite = items[i];
        }
        for (int i = 0; i < row2.Count; i++)
        {
            row2[i].sprite = items[i];
        }
        for (int i = 0; i < row3.Count; i++)
        {
            row3[i].sprite = items[i];
        }
        for (int i = 0; i < row4.Count; i++)
        {
            row4[i].sprite = items[i];
        }
        for (int i = 0; i < row5.Count; i++)
        {
            row5[i].sprite = items[i];
        }


    }
    public void EndRoll()
    {
        List<int> pos = new List<int>();
        for(int i = 0; i < slot.Length; i++)
        {
            slot[i].anchoredPosition = new Vector2(slot[i].anchoredPosition.x, finalPos[(Random.Range(0, finalPos.Count))]);
            pos.Add((int)slot[i].anchoredPosition.y);
        }
        FillRows(slotItemsAtEnd);
        spin = false;
        StartCoroutine(Result(pos[0], pos[1], pos[2], pos[3], pos[4]));
    }

    IEnumerator Result(int a, int b, int c, int d, int e)
    {
        yield return new WaitForSeconds(1f);
        var diff1 = (a - b) / 80;
        var diff2 = (b - c) / 80;
        var diff3 = (c - d) / 80;
        var diff4 = (d - e) / 80;

        var sum = diff1 + diff2 + diff3 + diff4;

        if(diff1 >= -1 && diff1 <= 1 && diff2 >= -2 && diff2 <= 2 && diff3 >= -2 && diff3 <= 2 && diff4 >= -1 && diff4 <= 1)
        {
            aud.clip = win;
            aud.Play();
            winScreen.gameObject.SetActive(true);
            if (diff1 == 0 && diff2 == 0 && diff3 == 0 && diff4 == 0)
            {
                winScreen.sprite = bigWin;
                winAmount.text = "298,680,000";
            }
            if(diff1 != 0 && diff2 != 0 && diff3 != 0 && diff4 != 0)
            {
                winScreen.sprite = superWin;
                winAmount.text = "120,790,245";
            }
            if(diff2 == 2 || diff2 == -2 && diff3 == 2 || diff3 == -2)
            {
                winScreen.sprite = megaWin;
                winAmount.text = "67,830,123";
            }
        }
        else
        {
            loseScreen.gameObject.SetActive(true);
            aud.clip = lose;
            aud.Play();
        }
    }

    public void Roll()
    {
        FillRows(slotItems);
        spin = true;
        Invoke(nameof(EndRoll), 5.0f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(spin)
        {
            foreach (var obj in slot)
            {
                obj.anchoredPosition = new Vector2(obj.anchoredPosition.x, finalPos[(Random.Range(0, finalPos.Count))]);
            }
        }
    }
}
