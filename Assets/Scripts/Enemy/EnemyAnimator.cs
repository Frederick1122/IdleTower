using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public float GetAnimationLength()
    {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }
    
    public void PlayDead()
    {
        _animator.SetBool("IsDead", true);
    }

    public void PlayWalk()
    {
        _animator.SetBool("IsWalking", true);
        _animator.SetBool("IsAttack", false);
    }

    public void PlayAttack()
    {
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsAttack", true);
    }
}
