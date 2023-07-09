using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject bat;
    private bool hasBat = true;
    private int life = 3;

    [SerializeField]
    private EnemyAnimController animator;

    [SerializeField]
    private EnemyFollow followBehavior;

    [SerializeField]
    private RunAway fleeBehavior;

    [SerializeField]
    private Switch switchButton;
    [SerializeField]
    private GameplayManager gameplayManager;

    private Vector3 previousPos;
    public delegate void OnHealthChange(int currentHealth);
    public event OnHealthChange onHealthChange;

    private void OnDisable()
    {
        switchButton.switchActivatedEvent -= SwitchActivated;
        gameplayManager.switchTimeUpEvent -= SwitchTimeUp;
    }

    private void OnEnable()
    {
        switchButton.switchActivatedEvent += SwitchActivated;
        gameplayManager.switchTimeUpEvent += SwitchTimeUp;
    }

    // Start is called before the first frame update
    void Start()
    {
        previousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bat.SetActive(hasBat);
        followBehavior.enabled = hasBat;
        fleeBehavior.enabled = !hasBat;

        if (!animator.IsAttacking)
        {
            if (followBehavior.enabled && Vector3.Distance(followBehavior.Player.transform.position, transform.position) <= 1.0f)
            {
                Attack();
                return;
            }
            
            if (Vector3.Distance(transform.position, previousPos) != 0.0f)
            {
                animator.PlayWalkAnim();
            }

            else
            {
                animator.PlayIdleAnim();
            }
            
        }
        previousPos = transform.position;
    }

    public void Hit()
    {
        life--;
        /*if (life <= 0)
        {
            animator.PlayDeathAnim();
        }*/
        onHealthChange?.Invoke(life);
        hasBat = true;
    }

    private void Attack()
    {
        if (animator.IsAttacking || !hasBat) return;
        Debug.Log("attack");
        animator.PlayAttackAnim();
    }

    private void SwitchActivated()
    {
        hasBat = false;
    }

    private void SwitchTimeUp()
    {
        hasBat = true;
    }
}
