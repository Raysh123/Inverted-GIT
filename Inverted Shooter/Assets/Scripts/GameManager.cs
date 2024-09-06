using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Transform[] team1SpawnPoints;
    public Transform[] team2SpawnPoints;

    public GameObject playerPrefab;  // Use a single prefab for both teams

    public Material team1Material;
    public Material team2Material;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.LocalPlayer != null)
        {
            // Ensure only the local player spawns their own prefab
            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                string team = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
                Transform spawnPoint = GetSpawnPointForTeam(team);

                if (spawnPoint != null) // Ensure a valid spawn point is returned
                {
                    GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
                    
                    // Assign the correct material based on the team
                    AssignTeamMaterial(spawnedPlayer, team);

                    // Ensure the camera is enabled for the local player
                    EnablePlayerCamera(spawnedPlayer);
                }
                else
                {
                    Debug.LogError("No valid spawn point found for the player.");
                }
            }
        }
        else
        {
            Debug.LogError("PhotonNetwork is not ready or LocalPlayer is null.");
        }
    }

    private Transform GetSpawnPointForTeam(string team)
    {
        Transform spawnPoint = null;
        if (team == "Team 1")
        {
            if (team1SpawnPoints != null && team1SpawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, team1SpawnPoints.Length);
                spawnPoint = team1SpawnPoints[randomIndex];
                Debug.Log($"Team 1 spawn point at index {randomIndex} selected: {spawnPoint.position}");
            }
            else
            {
                Debug.LogError("Team 1 spawn points array is empty or not assigned.");
            }
        }
        else if (team == "Team 2")
        {
            if (team2SpawnPoints != null && team2SpawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, team2SpawnPoints.Length);
                spawnPoint = team2SpawnPoints[randomIndex];
                Debug.Log($"Team 2 spawn point at index {randomIndex} selected: {spawnPoint.position}");
            }
            else
            {
                Debug.LogError("Team 2 spawn points array is empty or not assigned.");
            }
        }
        else
        {
            Debug.LogError($"Invalid team name: {team}. Expected 'Team 1' or 'Team 2'.");
        }

        return spawnPoint;
    }

    private void AssignTeamMaterial(GameObject player, string team)
    {
        Renderer playerRenderer = player.GetComponent<Renderer>();

        if (team == "Team 1")
        {
            playerRenderer.material = team1Material;
        }
        else if (team == "Team 2")
        {
            playerRenderer.material = team2Material;
        }
    }

    private void EnablePlayerCamera(GameObject player)
    {
        // Get the camera attached to the player and enable it if it's the local player
        Camera playerCamera = player.GetComponentInChildren<Camera>();
        PhotonView photonView = player.GetComponent<PhotonView>();

        if (photonView.IsMine && playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            Debug.Log("Camera enabled for local player.");
        }
        else if (!photonView.IsMine && playerCamera != null)
        {
            playerCamera.gameObject.SetActive(false);
            Debug.Log("Camera disabled for remote player.");
        }
        else
        {
            Debug.LogError("Camera component not found on player prefab.");
        }
    }

    private void InitializeGameMode(string gameMode)
    {
        if (gameMode == "Deathmatch")
        {
            // Set up Deathmatch-specific logic
        }
        else if (gameMode == "Capture the Flag")
        {
            // Set up Capture the Flag logic
        }
    }
}