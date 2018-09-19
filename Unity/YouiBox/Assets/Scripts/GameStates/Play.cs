using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class Play : GameState
{
    public override GameStateID StateID { get { return GameStateID.Play; } }

    private const float m_checkIntervalSeconds = 0.25f;
    
    // Update is called once per frame
    public override void Update()
    {

    }

    public override void EnterState()
    {
        base.EnterState();

        GameData.Instance.PlayerChoiceRefreshed += OnPlayerChoiceRefreshed;

        OnPlayerChoiceRefreshed();
    }

    public override void ExitState()
    {
        GameData.Instance.PlayerChoiceRefreshed -= OnPlayerChoiceRefreshed;

        base.ExitState();
    }

    void OnPlayerChoiceRefreshed()
    {
        int playerCount = GameData.Instance.PlayerCount;
        for (int i = 0; i < playerCount; i++)
        {
            PlayerData data = GameData.Instance.GetPlayerDataForID(i);
            SetPlayerData(data);
        }

        if (AllChoicesComplete())
        {
            m_gameStateMgr.ChangeState(GameStateID.Play);
        }
        else
        {
            StartCoroutine(WaitForResults());
        }
    }

    IEnumerator WaitForResults()
    {
        yield return new WaitForSeconds(m_checkIntervalSeconds);
        GameData.Instance.RefreshGameResults();
    }

    void SetPlayerData(PlayerData data)
    {
        int index = data.ID;

        if (index < m_gameStateMgr.m_playerControllers.Count)
        {
            PlayerCtrl player = m_gameStateMgr.m_playerControllers[data.ID];
            player.SetChoice(data.Choice);
        }
    }

    bool AllChoicesComplete()
    {
        bool allReady = true;

        int playerCount = GameData.Instance.PlayerCount;
        for (int i = 0; i < playerCount; i++)
        {
            PlayerData data = GameData.Instance.GetPlayerDataForID(i);
            allReady = allReady && data.Choice != PlayerChoice.Undefined;
        }

        return allReady;
    }

}
