using System.Collections;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView), typeof(Rigidbody))]
public class Bullet : MonoBehaviourPun
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("Bullet Rigidbody initialized");
    }

    public void Initialize(Vector3 direction, float speed)
    {
        if (!photonView.IsMine)
        {
            return; // Only the owner should control bullet movement
        }

        // Ensure the Rigidbody is not kinematic and can move
        rb.isKinematic = false;

        // Apply velocity to the bullet
        rb.velocity = direction.normalized * speed;

        Debug.Log($"Bullet initialized with velocity: {rb.velocity}");
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            return; // The bullet is controlled by the network
        }

        // Destroy the bullet after 2 seconds if it hasn't hit anything
        Destroy(gameObject, 2f);
    }
}
