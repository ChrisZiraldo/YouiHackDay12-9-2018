#define OFFLINE_MODE

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

    public Dictionary<int, PlayerData> m_playerData = new Dictionary<int, PlayerData>(2);

    public static GameData Instance
    {
        get
        {
            return m_instance;
        }
    }

    public int PlayerCount { get { return m_playerData.Count; } }

    private void Awake()
    {
        m_instance = this;
    }

    void AddPlayer(int playerID, PlayerData playerData)
    {
        m_playerData[playerID] = playerData;

    }

    void RemovePlayer(int playerID)
    {
        if (m_playerData.ContainsKey(playerID))
        {
            m_playerData[playerID] = null;
        }
    }

    public PlayerData GetPlayerDataForID(int playerID)
    {
        PlayerData data = null;

        if (m_playerData.Count > playerID)
            data = m_playerData[playerID];

        return data;
    }

    public void CreateRoom()
    {
        StartCoroutine(CreateRoom_crt());
    }

    IEnumerator CreateRoom_crt()
    {

#if OFFLINE_MODE

        if (RoomCreatedEvent != null)
            RoomCreatedEvent();

        yield break;
#else
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
        }
#endif

    }

    public void RefreshPlayerData()
    {
        StartCoroutine(RefreshPlayerData_ctr());
    }

    IEnumerator RefreshPlayerData_ctr()
    {

#if OFFLINE_MODE

        HandleRefreshPlayerDataResponse("[{\"Id\":0,\"Name\":\"Bob\"},{\"Id\":1,\"Name\":\"Chris\"}]");
        yield break;

#else
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
        }
#endif
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
                int id = n[i]["Id"].AsInt;
                PlayerData data = null;

                if (!m_playerData.ContainsKey(id))
                {
                    data = new PlayerData(id);
                    AddPlayer(data.ID, data);
                }
                else
                {
                    data = m_playerData[id];
                }

                data.Name = n[i]["Name"];


            }
        }

        if (PlayerDataRefreshed != null)
            PlayerDataRefreshed();
    }

    public void RefreshGameResults()
    {
        StartCoroutine(RefreshGameResults_ctr());
    }

    IEnumerator RefreshGameResults_ctr()
    {

#if OFFLINE_MODE
        HandleGameResultsResponse("[{\"Id\":0,\"Choice\":3},{\"Id\":1,\"Choice\":1}]");

        yield break;
#else

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
#endif
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
                if (data != null)
                {
                    PlayerChoice p = (PlayerChoice)n[i]["Choice"].AsInt;
                    data.Choice = p;
                }


            }
        }

        if (PlayerChoiceRefreshed != null)
            PlayerChoiceRefreshed();
    }

}
