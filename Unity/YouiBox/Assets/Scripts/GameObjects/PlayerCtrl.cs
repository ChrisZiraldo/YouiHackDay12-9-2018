using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    //PlayerData m_playerData = null;

    [SerializeField] Image m_paper = null, m_scissor = null, m_rock = null;
    [SerializeField] Text m_playerName = null;
    [SerializeField] Animator m_animator = null;

    //public int AssociatedPlayerID { get { return m_playerData.ID; } }

    public event Action<PlayerCtrl> ResultsShown = null;

    protected PlayerChoice m_playerChoice = PlayerChoice.Undefined;

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

    public void ShowResults(PlayerChoice choice)
    {
        m_playerChoice = choice;
        m_animator.Play("Play");
    }

    void OnPlayComplete()
    {
        ExecuteChoice(m_playerChoice);
        ResultsShown(this);
    }

    void ExecuteChoice(PlayerChoice choice)
    {
        if (m_rock) 
            m_rock.gameObject.SetActive(choice == PlayerChoice.Rock);

        if (m_scissor) 
            m_scissor.gameObject.SetActive(choice == PlayerChoice.Scissors);

        if (m_paper) 
            m_paper.gameObject.SetActive(choice == PlayerChoice.Paper);
    }
}
