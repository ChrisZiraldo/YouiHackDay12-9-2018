using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    [SerializeField]
    protected GameStateManager m_gameStateMgr = null;

    protected virtual void Start()
    {
        if (m_gameStateMgr)
            m_gameStateMgr.AssignState(this);
    }

    public abstract void Update();

    public abstract GameStateID StateID { get; }

    public virtual void EnterState() 
    {
        Debug.Log("State Entered: " + StateID);
    }
    public virtual void ExitState() 
    {
        Debug.Log("State Exited: " + StateID);
    }
}
