using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private EnemyController EnemyController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet") 
        {
            EnemyController.GetHit();
        }

        if (collision.tag == "Sword") 
        {
            PlayerController.Instance.GetHit();
        }
    }
}
