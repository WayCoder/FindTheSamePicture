using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    
    public enum State
    {
        MainInit,
        RunTime,
        Ending
    }
    public enum Result
    {
        Planet_A,
        Planet_B,
        Draw
    }

    #region data
    [field: Header("Data")]
    [field: SerializeField] public RuntimeDataSO data { get; private set; }
    [field: SerializeField] public State state { get; set; }
    [field: SerializeField] public Result result { get; set; }
    public List<Vector2> positionList { get; private set; } 

   



    [field: Header("Object")]
    [field: SerializeField] public GameObject cardsParent { get; private set; }
    public Card[] cardArray { get; private set; }
    #endregion



    #region state
    private StateMachine stateMachine;
    public MainInitState mainInitState { get; private set; }
    public RunTimeState runTimeState { get; private set; }
    public EndingState endingState { get; private set; }
    #endregion


    #region public
    public void OnGameStartButton()
    {
        stateMachine?.ChangeState(runTimeState);
    }

    public void MakePareCard()
    {
        runTimeState.MakePareCard();
    }
    #endregion


    #region private
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);

            return;
        }

        instance = this;

        cardArray = cardsParent.GetComponentsInChildren<Card>();

        positionList = new List<Vector2>();

        foreach (Card card in cardArray)
        {
            positionList.Add(card.transform.position);

        }


        stateMachine = new StateMachine();

        mainInitState = new MainInitState(stateMachine, instance);

        runTimeState = new RunTimeState(stateMachine, instance);

        endingState = new EndingState(stateMachine, instance);
    }
    private void Start()
    {
        stateMachine?.ChangeState(mainInitState);
    }
    private void Update()
    {
        stateMachine?.Execute();
    }
    #endregion
}