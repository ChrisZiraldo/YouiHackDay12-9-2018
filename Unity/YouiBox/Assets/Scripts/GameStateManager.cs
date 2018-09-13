using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateID { Undefined = -1, CreateRoom, WaitForPlayers, Play, Results, GameStateIDCount }

public class GameStateManager : MonoBehaviour
{
    protected Dictionary<GameStateID, GameState> m_gameStates = new Dictionary<GameStateID, GameState>((int)GameStateID.GameStateIDCount);

    protected GameState m_currentState  = null;
    protected GameStateID m_previousStateID = GameStateID.Undefined;
	
	void Update ()
    {
        if (m_currentState != null)
            m_currentState.Update();
    }

    public void AssignState(GameState state)
    {
        if(state != null)
            m_gameStates[state.StateID] = state;
    }

    public void ChangeState(GameStateID stateID)
    {
        if (stateID == GameStateID.Undefined || (m_currentState != null && stateID == m_currentState.StateID) || !m_gameStates.ContainsKey(stateID))
        {
            Debug.Log("State change failed");
            return;
        }

        if (m_currentState != null)
        {
            m_currentState.ExitState();
            m_previousStateID = m_currentState.StateID;
        }

        m_currentState = m_gameStates[stateID];

        if (m_currentState != null)
            m_currentState.EnterState();
    }
}
