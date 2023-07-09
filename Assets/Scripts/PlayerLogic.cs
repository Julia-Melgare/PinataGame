using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    private PlayerAnimController animator;
    [SerializeField]
    private GameplayManager gameplayManager;
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private GameObject bat;
    private bool hasBat = false;
    private int life = 3;
    private CharacterController controller = null;
    
    public delegate void OnHealthChange(int currentHealth);
    public event OnHealthChange onHealthChange;

    public delegate void OnHealthLoss();
    public event OnHealthLoss onHealthLoss;

    public delegate void OnHealthGain();
    public event OnHealthGain onHealthGain;

    private void OnDisable()
    {
        gameplayManager.switchTimeUpEvent -= SwitchTimeUp;
    }

    private void OnEnable()
    {
        gameplayManager.switchTimeUpEvent += SwitchTimeUp;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        bat.SetActive(hasBat);
        if (animator.IsAttacking) return;
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();   
        }
    }

    private void Move()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
            animator.PlayWalkAnim();
        }
        else
        {
            animator.PlayIdleAnim();
        }
    }

    private void Attack()
    {
        if (!hasBat || animator.IsAttacking) return;
        animator.PlayAttackAnim();  
    }

    private void SwitchTimeUp()
    {
        if(hasBat)
        {
            hasBat = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Switch")
        {
            hasBat = true;
            var switchBtn = other.gameObject.GetComponent<Switch>();
            switchBtn.DisableSwitch();
            life--;
            onHealthLoss?.Invoke();
            onHealthChange?.Invoke(life);
        }

        if(other.tag == "Lollipop")
        {
            life++;
            onHealthGain?.Invoke();
            onHealthChange?.Invoke(life);
            Destroy(other.gameObject);
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy" && hasBat && animator.IsAttacking)
        {
            Debug.Log("hit goblin");
            other.gameObject.GetComponent<EnemyLogic>().Hit();
            hasBat = false;
        }
    }
}
