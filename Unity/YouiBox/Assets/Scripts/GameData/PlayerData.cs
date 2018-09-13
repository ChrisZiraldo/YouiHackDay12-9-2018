using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayOption { Undefined = 0, Rock = 1, Paper = 2, Scissors = 3 }

public class PlayerData
{
    public string m_name = "unknown";
    public int m_ID = -1;
    public PlayOption m_play = PlayOption.Undefined;

    // TODO: Add data change events for above members
}
