using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

[Serializable]
public class RunTimeState : State
{
    private GameManager gameManager;

    private float remainingTime;

    private int maxPareCard;

    private int pareCard;

    public RunTimeState(StateMachine stateMachine, GameManager gameManager) : base(stateMachine)
    {
        this.gameManager = gameManager;
    }
    public override void Check()
    {
        if (maxPareCard == pareCard)
        {
            gameManager.endingState.SetEndingType(EndingState.Ending.Clear);

            stateMachine.ChangeState(gameManager.endingState);

            return;
        }

        if (remainingTime <= 0f)
        {
            gameManager.endingState.SetEndingType(EndingState.Ending.Faild);

            stateMachine.ChangeState(gameManager.endingState);

            return;
        }
    }
    public override void Enter()
    {
        gameManager.state = GameManager.State.RunTime;

        gameManager.cardsParent.gameObject.SetActive(true);

        gameManager.clicker.gameObject.SetActive(true);

        pareCard = 0;

        maxPareCard = gameManager.cardArray.Length / 2;

        remainingTime = gameManager.data.gamestateData.gameplayTime;

        Shuffle();

        UIManager.instance?.SetTimerUI(true, remainingTime / gameManager.data.gamestateData.gameplayTime, (int)remainingTime);
    }
    public override void Execute()
    {
        UIManager.instance.SetTimerUI(true, remainingTime / gameManager.data.gamestateData.gameplayTime, (int)remainingTime);

        remainingTime -= Time.deltaTime;
    }
    public override void Exit()
    {
        UIManager.instance.SetTimerUI(false);

        gameManager.cardsParent.gameObject.SetActive(false);

        gameManager.clicker.gameObject.SetActive(false);

        gameManager.clicker.StopAllCoroutines();

        foreach (Card card in gameManager.cardArray)
        {
            card.StopAllCoroutines();
        }
    }
    public void MakePareCard()
    {
        pareCard++;
    }
    private void Shuffle()
    {
        List<Vector2> cloneList = new List<Vector2>(gameManager.positionList);

        foreach (Card card in gameManager.cardArray)
        {
            int index = UnityEngine.Random.Range(0, cloneList.Count);

            card.transform.position = cloneList[index]; 

            cloneList.RemoveAt(index);
        }
    }
}