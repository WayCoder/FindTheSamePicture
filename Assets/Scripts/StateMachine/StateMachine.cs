using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System;


[Serializable]
public class StateMachine 
{
    private State state;

    public void ChangeState(State state)
    {
        if (state == null)
        {
            return;
        }

        this.state?.Exit();

        this.state = state;

        this.state?.Enter();
    }
    public void Execute()
    {
        state?.Execute();

        state?.Check();
    }
}
