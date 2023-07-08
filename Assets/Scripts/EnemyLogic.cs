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
    private EnemyFollow followBehavior;

    [SerializeField]
    private RunAway fleeBehavior;

    [SerializeField]
    private Switch switchButton;
    [SerializeField]
    private GameplayManager gameplayManager;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        bat.SetActive(hasBat);
        followBehavior.enabled = hasBat;
        fleeBehavior.enabled = !hasBat;
    }

    private void SwitchActivated()
    {
        hasBat = false;
        life--;
        onHealthChange?.Invoke(life);
    }

    private void SwitchTimeUp()
    {
        hasBat = true;
    }
}
