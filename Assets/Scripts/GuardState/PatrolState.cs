﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IGuardState
{

    public GuardState state
    {
        get
        {
            return GuardState.PatrolState;
        }
    }

    public Action<Guard, float> UpdateAction
    {
        get
        {
            return GuardActions.BasicPatrolStateUpdate;
        }
    }

    public Guard guard { get; set; }

    public PatrolState(Guard g)
    {
        guard = g;
    }

    

    public void ChangeState(GuardState state)
    {
        switch(state)
        {
            case GuardState.PatrolState:
                Debug.LogError("PatrolState::ChangeState - Can't switch from PatrolState to PatrolState");
                break;
            case GuardState.AlertState:
                // TODO: Set Guard State
                // DONE
                guard.CurrentState = guard.alertState;
                break;
            case GuardState.ChaseState:
                Debug.LogError("PatrolState::ChangeState - Can't switch from PatrolState to ChaseState");
                break;
        }
    }
}
