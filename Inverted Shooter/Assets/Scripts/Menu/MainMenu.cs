using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInputField;
    public GameObject createRoomButton;
    public GameObject joinRoomButton;

    public void CreateRoom()
    {
        string roomName = roomInputField.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 });
        }
    }

    public void JoinRoom()
    {
        string roomName = roomInputField.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public override void OnJoinedRoom()
    {
        // Load the game scene when the player joins a room
        PhotonNetwork.LoadLevel("TeamSelection");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message);
        // Handle room creation failure (e.g., show an error message)
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room join failed: " + message);
        // Handle room joining failure (e.g., show an error message)
    }
}