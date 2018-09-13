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
        GameData.Instance.RefreshPlayerData();
    }

    public override void ExitState()
    {
        GameData.Instance.PlayerDataRefreshed -= OnPlayerDataRefreshed;
        base.ExitState();
    }

    void OnPlayerDataRefreshed()
    {
        if (GameData.Instance.m_players.Count >= GameData.MAX_PLAYERS)
        {
            m_gameStateMgr.ChangeState(GameStateID.Play);
        }
        else
        {
            StartCoroutine(RecheckPlayerData(m_checkIntervalSeconds));
        }
    }

    IEnumerator RecheckPlayerData(float interval)
    {
        yield return new WaitForSecondsRealtime(interval);
        GameData.Instance.RefreshPlayerData();
    }
}
