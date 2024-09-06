using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class MatchManager : MonoBehaviourPunCallbacks
{
    public void StartMatch()
    {
        // Check if all players have selected teams and game mode
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("Team") || !player.CustomProperties.ContainsKey("GameMode"))
            {
                Debug.Log("Waiting for all players to select teams and game mode.");
                return;
            }
        }

        // All players are ready, start the game
        PhotonNetwork.LoadLevel("GameScene");
    }
}