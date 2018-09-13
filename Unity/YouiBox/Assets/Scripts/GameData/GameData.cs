using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class GameData : MonoBehaviour
{
    public static int MAX_PLAYERS = 2;
    private static GameData m_instance = null;

    public event Action RoomCreatedEvent = null;
    public event Action PlayerDataRefreshed = null;
    public event Action PlayerChoiceRefreshed = null;

    public event Action<PlayerData> PlayerDataAdded = null;
    public event Action<PlayerData> PlayerDataRemoved = null;

    public static GameData Instance
    {
        get
        {
            return m_instance;
        }
    }

    private void Awake()
    {
        m_instance = this;
    }

    public Dictionary<int, PlayerData> m_players = new Dictionary<int, PlayerData>(2);

    public void AddPlayer(int playerID, PlayerData playerData)
    {
        m_players[playerID] = playerData;

        if (PlayerDataAdded != null)
            PlayerDataAdded(playerData);
    }

    public void RemovePlayer(int playerID)
    {
        if (m_players.ContainsKey(playerID))
        {
            PlayerData data = m_players[playerID];
            m_players.Remove(playerID);

            if (PlayerDataRemoved != null)
                PlayerDataRemoved(data);
        }
    }

    public PlayerData GetPlayerDataForID(int playerID)
    {
        PlayerData data = null;

        if (m_players.ContainsKey(playerID))
            data = m_players[playerID];

        return data;
    }

    public void CreateRoom()
    {
        StartCoroutine(CreateRoom_crt());
    }

    IEnumerator CreateRoom_crt()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://10.100.89.228:3000/CreateRoom"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if(RoomCreatedEvent != null)
                    RoomCreatedEvent();
            }

            //if (RoomCreatedEvent != null)
                //RoomCreatedEvent();
        }
    }

    public void RefreshPlayerData()
    {
        StartCoroutine(RefreshPlayerData_ctr());
    }

    IEnumerator RefreshPlayerData_ctr()
    {
        //string url = "http://10.100.89.228:3000/GetPlayerInfo";
        string url = "http://10.100.89.228:3000/FakeGetPlayerInfo";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string response = www.downloadHandler.text;
                HandleRefreshPlayerDataResponse(response);
            }

            //HandleRefreshPlayerDataResponse("[{\"Id\":0,\"Name\":\"Bob\"},{\"Id\":1,\"Name\":\"Chris\"}]");
        }
    }

    void HandleRefreshPlayerDataResponse(string json)
    {
        var n = JSON.Parse(json);
        Debug.Log(n.ToString());

        if (n.IsArray)
        {
            int count = n.Count;
            for (int i = 0; i < count; i++)
            {
                // only add players to GameData if they are not yet added
                if (GameData.Instance.m_players.Count <= count)
                {
                    PlayerData data = new PlayerData();
                    data.m_ID = n[i]["Id"].AsInt;
                    data.m_name = n[i]["Name"];

                    AddPlayer(data.m_ID, data);
                }
            }

            if (PlayerDataRefreshed != null)
                PlayerDataRefreshed();
        }
    }

    IEnumerator RefreshGameResults()
    {
        //string url = "http://10.100.89.228:3000/GetResults";
        string url = "http://10.100.89.228:3000/FakeGetResults";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string response = www.downloadHandler.text;
                HandleGameResultsResponse(response);
            }
        }
    }

    void HandleGameResultsResponse(string json)
    {
        var n = JSON.Parse(json);
        Debug.Log(n.ToString());

        if (n.IsArray)
        {
            int count = n.Count;
            for (int i = 0; i < count; i++)
            {
                int id = n[i]["Id"].AsInt;

                PlayerData data = GetPlayerDataForID(id);
                if(data != null)
                {
                    PlayOption p = (PlayOption)n[i]["Choice"].AsInt;
                    data.m_play = p;
                }
            }

            if (PlayerChoiceRefreshed != null)
                PlayerChoiceRefreshed();
        }
    }

}
