using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class Results : GameState
{
    public override GameStateID StateID { get { return GameStateID.Results; } }

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void EnterState()
    {
        base.EnterState();

        // show results for each player
        int count = m_gameStateMgr.m_playerControllers.Count;
        for (int i = 0; i < count; i++)
        {
            PlayerCtrl player = m_gameStateMgr.m_playerControllers[i];
            player.ResultsShown += OnShowResultsComplete;
            player.ShowResults(GameData.Instance.GetPlayerDataForID(i).Choice);
        }

        
    }

    public override void ExitState()
    {


        base.ExitState();
    }

    void OnShowResultsComplete(PlayerCtrl player)
    {
        //m_gameStateMgr.ChangeState(GameStateID.CreateRoom);
            

    }


}
