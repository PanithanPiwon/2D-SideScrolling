using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator slimeAnim;
    [SerializeField] private Animator swordAnim;

    public void Attack() 
    {
        slimeAnim.Play("Slash");
        swordAnim.Play("SwordSlash");
    }
}
