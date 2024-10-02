using UnityEngine;

public class TimeChangeCollider : MonoBehaviour
{
    [SerializeField] private TimeManager timeManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timeManager.TimeJump = true;
            timeManager.AdvanceTime();
        }
    }
}
