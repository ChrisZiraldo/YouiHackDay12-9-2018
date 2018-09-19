using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerChoice { Undefined = 0, Rock = 1, Paper = 2, Scissors = 3 }

public class PlayerData
{
    public PlayerData(int id)
    {
        m_id = id;
    }

    int m_id = -1;
    string m_name = "unknown";
    PlayerChoice m_choice = PlayerChoice.Undefined;

    public event Action<string> NameChanged = null;
    public event Action<PlayerChoice> ChoiceChanged = null;

    public int ID
    {  
        get { return m_id; }
    }
    public string Name
    {
        get { return m_name; }
        set
        {
            if (m_name == value)
                return;

            m_name = value;

            if (NameChanged != null)
                NameChanged(m_name);
        }
    }
    public PlayerChoice Choice
    {
        get { return m_choice; }
        set
        {
            if (m_choice == value)
                return;

            m_choice = value;

            if (ChoiceChanged != null)
                ChoiceChanged(m_choice);
        }
    }

}
