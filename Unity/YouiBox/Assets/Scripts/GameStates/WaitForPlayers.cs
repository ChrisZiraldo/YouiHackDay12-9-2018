using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class WaitForPlayers : GameState
{
    public override GameStateID StateID { get { return GameStateID.WaitForPlayers; } }

    private const float m_checkIntervalSeconds = 0.25f;

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void EnterState()
    {
        base.EnterState();

        GameData.Instance.PlayerDataRefreshed += OnPlayerDataRefreshed;

        OnPlayerDataRefreshed();
    }

    public override void ExitState()
    {
        GameData.Instance.PlayerDataRefreshed -= OnPlayerDataRefreshed;

        base.ExitState();
    }

    void OnPlayerDataRefreshed()
    {
        int playerCount = GameData.Instance.PlayerCount;
        for (int i = 0; i < playerCount; i++)
        {
            PlayerData data = GameData.Instance.GetPlayerDataForID(i);
            SetPlayerData(data);
        }

        if (GameData.Instance.PlayerCount >= GameData.MAX_PLAYERS)
        {
            m_gameStateMgr.ChangeState(GameStateID.Play);
        }
        else
        {
            StartCoroutine(RecheckPlayerData());
        }
    } 

    IEnumerator RecheckPlayerData()
    {
        yield return new WaitForSecondsRealtime(m_checkIntervalSeconds);
        GameData.Instance.RefreshPlayerData();
    }

    void SetPlayerData(PlayerData data)
    {
        int index = data.ID;

        if (index < m_gameStateMgr.m_playerControllers.Count)
        {
            PlayerCtrl player = m_gameStateMgr.m_playerControllers[data.ID];
            player.SetName(data.Name);
        }
    }


}
