using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime;
    [SerializeField] private Animator animator;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Monster")
        {
            //Explo();
            Destroy(gameObject);
            //StartCoroutine(Delay(.5f, () =>
            //{

            //}));
        }
    }


    private void Explo()
    {
        animator.Play("BulletExplo");
    }

    IEnumerator Delay(float delay, UnityAction callback)
    {
        yield return new WaitForSeconds(delay);
        callback.Invoke();
    }
}
