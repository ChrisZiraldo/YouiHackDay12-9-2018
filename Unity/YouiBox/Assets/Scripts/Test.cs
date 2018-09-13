using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
endpoints:
/CreateRoom
~/GetPlayerCount <--New~
/GetPlayerInfo
/GetResults
/DestroyRoom (edited)
*/

public class Test : MonoBehaviour {

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://10.100.89.228:3000/GetPlayerInfo"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
