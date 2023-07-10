using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    private List<Card> selectCardList;


    public void Init()
    {
        selectCardList.Clear();
    }



    private void Awake()
    {
        selectCardList = new List<Card>();
    }

    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (selectCardList.Count >= 2)
        {
            return;
        }

        switch (collider.tag)
        {
            case "Card":
                Card card = collider.GetComponent<Card>();
                card.Draw();
                selectCardList.Add(card);

                if (selectCardList.Count >= 2)
                {
                    StartCoroutine(DrawCardCheck());

                    break;
                }
                break;
        }

    }


    private IEnumerator DrawCardCheck()
    {
        yield return new WaitUntil(() => !selectCardList[0].drawing && !selectCardList[1].drawing);

        yield return new WaitForSeconds(0.1f);

        if (selectCardList[0].index == selectCardList[1].index)
        {
            GameManager.instance.MakePareCard();
        }
        else
        {
            selectCardList[0].Draw();

            selectCardList[1].Draw();
        }

        selectCardList.Clear();

    }

}