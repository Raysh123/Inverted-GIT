using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public Material team1Material;
    public Material team2Material;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;

    private float moveHorizontal;
    private float moveVertical;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().gameObject.SetActive(false);
            GetComponent<PlayerController>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
        }
    }

    void AssignTeamMaterial(GameObject player, int team)
    {
        Renderer playerRenderer = player.GetComponent<Renderer>();

        if (team == 1)
        {
            playerRenderer.material = team1Material;
        }
        else if (team == 2)
        {
            playerRenderer.material = team2Material;
        }
    }
    void Move()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

        // Log the new position
        Vector3 newPosition = rb.position + move * moveSpeed * Time.deltaTime;
       

        rb.MovePosition(newPosition);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // Adjust jumping if gravity is inverted
            float jumpDirection = (rb.useGravity) ? 1f : -1f;
            rb.AddForce(Vector3.up * jumpForce * jumpDirection, ForceMode.Impulse);
        }
    }



    bool IsGrounded()
    {
        // Adjust the raycast direction based on whether gravity is inverted
        Vector3 rayDirection = (rb.useGravity) ? Vector3.down : Vector3.up;
        return Physics.Raycast(transform.position, rayDirection, 1.1f);
    }

}