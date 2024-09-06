using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Log what the bullet hit
        Debug.Log($"Bullet hit: {collision.gameObject.name}");

        // Destroy the bullet upon collision
        Destroy(gameObject);
    }
}
