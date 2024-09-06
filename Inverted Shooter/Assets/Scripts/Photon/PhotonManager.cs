using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // Connect to Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        // Join or create a lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        // Load the next scene or update the UI to show that the connection was successful
    }
}
