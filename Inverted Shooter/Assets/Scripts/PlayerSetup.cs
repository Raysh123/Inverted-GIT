using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    public Material team1Material;
    public Material team2Material;
    public float gravityScale = 1f; // Adjustable gravity scale

    private Rigidbody rb;
    private Camera playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();

        if (photonView.IsMine)
        {
            // Activate the camera and controls for the local player
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Camera component not found in children. Ensure the camera is a child of the player prefab.");
            }
            // This is the local player
            Debug.Log("This is my local player.");
        }
        else
        {
            // Disable the camera and controls for other players
            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(false);
            }
            Debug.Log("Another player is spawned.");

            GetComponent<PlayerController>().enabled = false;
        }

        ApplyTeamSettings();
    }

    void ApplyTeamSettings()
    {
        string team = (string)photonView.Owner.CustomProperties["Team"]; // Use photonView.Owner instead of LocalPlayer

        if (team == "Team 2")
        {
            // Rotate player for Team 2
            transform.Rotate(180, 0, 0); // Rotate 180 degrees on the X-axis

            // Disable default gravity and apply inverted gravity for Team 2
            rb.useGravity = false;

            // Apply custom gravity force
            rb.AddForce(Vector3.up * -Physics.gravity.y * gravityScale, ForceMode.Acceleration);
        }
        else
        {
            // Enable default gravity for Team 1
            rb.useGravity = true;
        }

        // Assign the correct material based on the team
        Renderer playerRenderer = GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            if (team == "Team 1")
            {
                playerRenderer.material = team1Material;
            }
            else if (team == "Team 2")
            {
                playerRenderer.material = team2Material;
            }
        }
    }

    void FixedUpdate()
    {
        // Apply custom gravity force if gravity is disabled
        if (!rb.useGravity)
        {
            rb.AddForce(Vector3.up * -Physics.gravity.y * gravityScale, ForceMode.Acceleration);
        }
    }
}