using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private bool isAttacking = false;
    public bool IsAttacking => isAttacking;
    private bool isDying = false;
    public bool IsDying => isDying;

    public void PlayAttackAnim()
    {
        isAttacking = true;
        animator.Play("Pinata Hit");
    }

    public void PlayWalkAnim()
    {
        animator.Play("Pinata Walk");
    }

    public void PlayDeathAnim()
    {
        animator.Play("Pinata Die");
    }

    public void PlayIdleAnim()
    {
        animator.Play("Idle");
    }

    public void AttackAnimEnd()
    {
        isAttacking = false;
    }

    public void DeathAnimEnd()
    {
        isDying = false;
    }
}
