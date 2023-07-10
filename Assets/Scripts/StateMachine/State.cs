using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State 
{
    protected StateMachine stateMachine;

    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;    
    }

    public abstract void Check();

    public abstract void Enter();

    public abstract void Execute();

    public abstract void Exit();
}
