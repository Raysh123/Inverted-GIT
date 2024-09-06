using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LobbyManager : MonoBehaviour
{
    public TMP_Dropdown teamDropdown;
    public TMP_Dropdown gameModeDropdown;
    public TMP_Text feedbackText;
    public GameObject confirmButton;

    private void Start()
    {
        confirmButton.SetActive(false);
        teamDropdown.onValueChanged.AddListener(delegate { OnSelectionChanged(); });
        gameModeDropdown.onValueChanged.AddListener(delegate { OnSelectionChanged(); });
    }

    private void OnSelectionChanged()
    {
        confirmButton.SetActive(true);
    }

    public void ConfirmSelection()
    {
            // Proceed to the game scene or start the match
        string selectedTeam = teamDropdown.options[teamDropdown.value].text;
        string selectedGameMode = gameModeDropdown.options[gameModeDropdown.value].text;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable 

    {
        { "Team", selectedTeam },
        { "GameMode", selectedGameMode }
    });

        // Set room properties to ensure they are available to all players
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable
    {
        { "GameMode", selectedGameMode }
    };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);

        feedbackText.text = "Team " + selectedTeam + " selected with " + selectedGameMode + " mode!";

        FindObjectOfType<MatchManager>().StartMatch();


    }

}