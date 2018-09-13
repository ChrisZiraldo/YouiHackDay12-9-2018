using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] Image m_paper, m_scissor, m_rock;
    [SerializeField] int m_associatedPlayerID = -1;
    [SerializeField] Text m_playerName = null;

    protected GameData m_gameData = null;

    // Use this for initialization
    void Start ()
    {
        m_gameData = GameData.Instance;
        m_gameData.PlayerDataRefreshed += OnPlayerDataRefreshed;

        // TODO: Hook up this class to the player data events, not the GameData directly
    }

    void OnPlayerDataRefreshed()
    {
        // TODO: Listen to changes in the associated playerData, not GameData

        PlayerData data = m_gameData.GetPlayerDataForID(m_associatedPlayerID);
        if(data != null)
        {
            if(m_playerName != null)
            {
                m_playerName.text = data.m_name;
            }
        }
    }
}
