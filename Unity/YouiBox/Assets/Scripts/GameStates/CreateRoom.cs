using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateRoom : GameState
{
    public override GameStateID StateID { get { return GameStateID.CreateRoom; } }

    protected override void Start()
    {
        base.Start();
        m_gameStateMgr.ChangeState(StateID);
    }

    // Update is called once per frame
    public override void Update ()
    {
		
	}

    public override void EnterState()
    {
        base.EnterState();

        GameData.Instance.RoomCreatedEvent += OnRoomCreated;
        GameData.Instance.CreateRoom();
    }

    public override void ExitState()
    {
        base.ExitState();
        GameData.Instance.RoomCreatedEvent -= OnRoomCreated;
    }

    protected void OnRoomCreated()
    {
        m_gameStateMgr.ChangeState(GameStateID.WaitForPlayers);
    }
}
