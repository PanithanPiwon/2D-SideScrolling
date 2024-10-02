using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; 
    [SerializeField] private Transform firePoint;     
    [SerializeField] private float bulletSpeed;

    public void Shoot(int direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(direction * bulletSpeed, rb.velocity.y);
    }
}
