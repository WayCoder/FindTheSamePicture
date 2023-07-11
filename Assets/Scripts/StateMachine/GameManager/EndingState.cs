using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EndingState : State
{

    public enum Ending
    {
        Clear,
        Faild
    }


    private GameManager gameManager;

    private Ending ending;


    public EndingState(StateMachine stateMachine, GameManager gameManager) : base(stateMachine)
    {
        this.gameManager = gameManager;
    }

    public override void Check()
    {
       
    }

    public override void Enter()
    {
        gameManager.state = GameManager.State.Ending;

        gameManager.cardsParent.gameObject.SetActive(false);

        if (ending == Ending.Clear)
        {
            UIManager.instance.SetTitlePanelUI(true, gameManager.data.uiData.clearText, gameManager.data.uiData.retryText);

        }
        else
        {
            UIManager.instance.SetTitlePanelUI(true, gameManager.data.uiData.faildText, gameManager.data.uiData.retryText);
        }


       
    }

    public override void Execute()
    {
     
        


    }

    public override void Exit()
    {
        UIManager.instance.SetTitlePanelUI(false);

       // UIManager.instance.SetResultUI(false);

        
    }

    public void SetEndingType(Ending type)
    {
        ending = type;
    }

}
