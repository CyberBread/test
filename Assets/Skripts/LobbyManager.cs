using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Text logText;

    private bool isConnected = false;

    void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);
        Log($"Player - {PhotonNetwork.NickName}");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        Log("Connected to Master Server");
        isConnected = true;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Log(message);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void CreateRoom()
    {
        if (isConnected)
        {
            PhotonNetwork.CreateRoom("1", new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
        }
        else
        {
            Log("Wait for connection to Master Server");
        }
    }

    public void JoinRoom()
    {
        if (isConnected)
        {
            PhotonNetwork.JoinRoom("1");
        }
        else
        {
            Log("Wait for connection to Master Server");
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Log(string message)
    {
        Debug.Log(message);
        logText.text += "\n";
        logText.text += message;
    }
}
