using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class Play : GameState
{
    public override GameStateID StateID { get { return GameStateID.Play; } }

    private const float m_checkIntervalSeconds = 0.25f;
    private bool m_gameOver = false;

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        StartCoroutine(WaitForResults());
    }

    IEnumerator WaitForResults()
    {
        m_gameOver = false;

        while (!m_gameOver)
        {
            // Poll server for play state
            //yield return CheckServerState();

            yield return null;
        }


    }



}
