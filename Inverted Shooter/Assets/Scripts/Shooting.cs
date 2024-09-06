using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviourPun
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;
    public float fireRate = 0.1f; // Time between shots
    public int magazineSize = 25;
    private int currentAmmo;
    private float nextFireTime;
    private Animator animator;

    private void Start()
    {
        currentAmmo = magazineSize;
        animator = GetComponentInChildren<Animator>(); // Get Animator component
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Check if the fire button is pressed and enough time has passed for the next shot
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                if (currentAmmo > 0)
                {
                    // Call the Fire method over the network
                    photonView.RPC("Fire", RpcTarget.AllBuffered);
                    nextFireTime = Time.time + fireRate;
                }
                else
                {
                    Debug.Log("Out of Ammo!");
                }
            }
        }
    }

    [PunRPC]
    void Fire()
    {
        if (currentAmmo <= 0) return;

        Vector3 bulletDirection = bulletSpawnPoint.forward; // The direction the gun is facing

        // Call FireBullet over the network, sending position and direction
        photonView.RPC("FireBullet", RpcTarget.All, bulletSpawnPoint.position, bulletDirection);

        currentAmmo--;
        Debug.Log("Fired! Ammo left: " + currentAmmo);

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    // The RPC method that will be called on all clients
    [PunRPC]
    void FireBullet(Vector3 position, Vector3 direction)
    {
        // Spawn the bullet at the specified position and direction
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction));
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            // Initialize the bullet's movement
            Debug.Log($"Firing bullet from {position} in direction {direction}");
            bulletScript.Initialize(direction, bulletSpeed);
        }
        else
        {
            Debug.LogError("No Bullet script found on the bullet prefab!");
        }
    }
    IEnumerator Reload()
    {
        Debug.Log("Reloading...");
        if (animator != null)
        {
            animator.SetTrigger("Reload"); // Trigger the reload animation
        }
        yield return new WaitForSeconds(2.0f); // Simulate reload time
        currentAmmo = magazineSize;
        Debug.Log("Reloaded! Ammo: " + currentAmmo);
    }
}