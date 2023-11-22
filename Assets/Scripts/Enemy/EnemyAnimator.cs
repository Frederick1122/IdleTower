using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void PlayDead()
    {
        _animator.SetBool("IsDead", true);
    }

    public void PlayWalk()
    {
        _animator.SetBool("IsWalking", true);
        _animator.SetBool("IsAttacking", false);
    }

    public void PlayAttack()
    {
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsAttacking", true);
    }
}
