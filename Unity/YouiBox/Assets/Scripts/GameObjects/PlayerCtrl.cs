using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    //PlayerData m_playerData = null;

    [SerializeField] Image m_paper = null, m_scissor = null, m_rock = null;
    [SerializeField] Text m_playerName = null;

    //public int AssociatedPlayerID { get { return m_playerData.ID; } }

    /*
    public PlayerData PlayerData
    {
        get { return m_playerData; }

        set 
        {
            if (m_playerData != null)
            {
                m_playerData.NameChanged -= OnPlayerNameChanged;
                m_playerData.ChoiceChanged -= OnPlayerChoiceChanged;
            }

            m_playerData = value;

            if (value != null)
            {
                m_playerData.NameChanged += OnPlayerNameChanged;
                m_playerData.ChoiceChanged += OnPlayerChoiceChanged;

                OnPlayerNameChanged(m_playerData.Name);
                OnPlayerChoiceChanged(m_playerData.Choice);
            }
            else
            {
                // TODO: Handle null

            }
        }
    }*/

    // Use this for initialization
    void Start ()
    {

    }

    public void SetName(string playerName)
    {
        if(m_playerName != null && !m_playerName.text.Equals(playerName))
        {
            m_playerName.text = playerName;
        }
    }

    public void SetChoice(PlayerChoice choice)
    {
        ExecuteChoice(choice);
    }

    void ExecuteChoice(PlayerChoice choice)
    {
        if (m_rock) m_rock.gameObject.SetActive(choice == PlayerChoice.Rock);
        if (m_scissor) m_scissor.gameObject.SetActive(choice == PlayerChoice.Scissors);
        if (m_paper) m_paper.gameObject.SetActive(choice == PlayerChoice.Paper);
    }

    /*void OnPlayerStateChanged(PlayerState state)
    {
        switch(state)
        {
            case PlayerState.Active:
            case PlayerState.Inactive:
            case PlayerState.Disabled:
            default:
                break;
        }
    }*/
}
