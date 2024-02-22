using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviourPunCallbacks
{
    public void OnClickStart()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        print("Start");
    }

    public override void OnConnectedToMaster()
    {
        print("Connected!");
        SceneManager.LoadScene("LobbyScene");
    }
}
