using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyAnimation enemyAnimation;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float forceKnockBack;
    [SerializeField] private float speed;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float returnDelay;
    [SerializeField] public float minDistanceToPlayer;

    private bool isChasing = false;
    private bool facingRight = true;
    private float timeOutOfRange;
    private float tempSpd;
    private Vector2 homePosition;

    void Start()
    {
        homePosition = transform.position;
        tempSpd = speed;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        if (distanceToPlayer < detectionRadius)
        {
            isChasing = true;
            timeOutOfRange = 0f;
            FollowPlayer();
        }
        else
        {

            if (isChasing)
            {
                timeOutOfRange += Time.deltaTime;
                if (timeOutOfRange >= returnDelay)
                {
                    isChasing = false;
                    ReturnToHome();
                }
            }
            else
            {
                ReturnToHome();
            }
        }
    }


    void FollowPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        if (distanceToPlayer > minDistanceToPlayer)
        {
            // Chase

            if (speed == 0)
            {
                StartCoroutine(IncreaseValueSmoothly(speed, tempSpd, 1f));
            }

            Vector2 targetPosition = PlayerController.Instance.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);


            if (targetPosition.x > transform.position.x && !facingRight)
            {
                Flip();
            }
            else if (targetPosition.x < transform.position.x && facingRight)
            {
                Flip();
            }
        }
        else
        {
            AttckToPlayer();
        }
    }


    void ReturnToHome()
    {
        Vector2 targetPosition = homePosition;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (targetPosition.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (targetPosition.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void AttckToPlayer()
    {
        speed = 0;
        StartCoroutine(Delay(.3f, () =>
        {
            enemyAnimation.Attack();
        }));
    }


    IEnumerator Delay(float delay, UnityAction callback = null)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    IEnumerator IncreaseValueSmoothly(float fromValue, float toValue, float duration, UnityAction callback = null)
    {
        while (speed < duration)
        {
            float currentValue = Mathf.Lerp(fromValue, toValue, speed / duration);
            speed += Time.deltaTime * 1f;
            yield return null;
        }
        speed = tempSpd;
        callback?.Invoke();
    }

    public void GetHit()
    {
        if (facingRight)
            rb.AddForce(new Vector2(-forceKnockBack, 2f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(forceKnockBack, 2f), ForceMode2D.Impulse);
    }
}
