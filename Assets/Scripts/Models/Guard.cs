﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;

public class Guard
{
    public float X { get; protected set; }
    public float Y { get; protected set; }
    public Vector2 Position
    {
        get
        {
            return new Vector2(X, Y);
        }
        set
        {
            X = value.x;
            Y = value.y;
        }
    }

    public GuardData Data { get; protected set; }

    public Guard(float startX, float startY, GuardData guardData)
    {
        X = startX;
        Y = startY;
        Data = guardData;

        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        chaseState = new ChaseState(this);

        CurrentState = patrolState;

        PatrolWayPoints = new List<Vector2>();
        SetRandomPatrolPoints(guardData.numberOfWaypoints);
    }

    Random rand
    {
        get
        {
            return DungeonController.instance.dungeon.rand; // FIXME: Create a gameManager that stores the System.Random
        }
    }

    Dungeon dungeon
    {
        get
        {
            return DungeonController.instance.dungeon;
        }
    }

    private void SetRandomPatrolPoints(int numOfPoints)
    {
        for (int i = 0; i < numOfPoints; i++)
        {
            Vector2 pos = Vector2.zero;
            if (dungeon == null)
            {
                Debug.Log("Dungeon is Null!");
                return;
            }
            Tile t = null;
            while (t == null || t.room == null)
            {
                float r = (float)rand.NextDouble();
                r = Mathf.Sqrt(r);
                r *= Data.waypointRadius;
                float theta = (float)rand.NextDouble();
                theta *= Mathf.PI * 2;
                pos = (new Vector2(r * Mathf.Cos(theta), r * Mathf.Sin(theta)));
                t = dungeon.GetTileAt((int)pos.x + (int)X, (int)pos.y + (int)Y);
            }
            // At this point, we have found a valid position
            PatrolWayPoints.Add(t.Position);
        }
    }

    public IGuardState CurrentState { get; set; } // QUESTION: Should we override the default settter?
    public PatrolState patrolState;
    public AlertState alertState;
    public ChaseState chaseState;

    public List<Vector2> PatrolWayPoints { get; protected set; }

    /*
     * The Guards follow a state machine:
     *  Patrol - The Guards are moving between several randomly selected waypoints
     *  Alert - The Guard has either seen or heard a noise, and is looking around for the threat 
     *      Pawn can hide during Alert State. Guard can ignore Alert
     *  Chase - The Guard has identified the threat, and is attempting to follow it
     *      The threat must remain withing 20 units of the guards current position
     *  
     *  After each alert, the chance that the guard will ignore an alert decreases
     *  A Chasing gaurd can alert other gaurds nearby
     */ 

    public void Update(float deltaTime)
    {
        // This update is set in the GuardData constructor
        // Defined in the GuardActions class, which will eventually be extracted out and be moddable
        Data.UpdateAction(this, deltaTime);
    }

}