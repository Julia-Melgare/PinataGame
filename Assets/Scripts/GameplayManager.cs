using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private float playerBatTime = 10f;
    [SerializeField]
    private float switchRespawnTime = 15f;
    [SerializeField]
    private float minLollipopSpawnTime = 30f;
    [SerializeField]
    private float maxLollipopSpawnTime = 60f;
    [SerializeField]
    private Switch switchButton;
    [SerializeField]
    private GameObject lollipopPrefab;

    [SerializeField]
    private float maxXPos = 7.22f;
    [SerializeField]
    private float minXPos = -8.85f;
    [SerializeField]
    private float maxZPos = 5.5f;
    [SerializeField]
    private float minZPos = -10.41f;

    [SerializeField]
    private PlayerLogic player;
    [SerializeField]
    private EnemyLogic enemy;
    private Vector3 initialSwitchPos;

    public delegate void SwitchTimeUp();
    public event SwitchTimeUp switchTimeUpEvent;
    private void OnDisable()
    {
        switchButton.switchActivatedEvent -= SwitchActivated;
        player.onHealthChange -= CheckLoss;
        enemy.onHealthChange -= CheckWin;
    }

    private void OnEnable()
    {
        switchButton.switchActivatedEvent += SwitchActivated;
        player.onHealthChange += CheckLoss;
        enemy.onHealthChange += CheckWin;

    }
    void Start()
    {
        initialSwitchPos = switchButton.transform.position;
        StartLollipopSpawn();
    }

    void Update()
    {
        
    }

    private void SwitchActivated()
    {
        Debug.Log("switch activated!");
        StartCoroutine(SwitchCountdown());
        StartCoroutine(SwitchRespawn());
    }

    private void UpdateSwitchPos()
    {
        float xPos = Random.Range(minXPos, maxXPos);
        float zPos = Random.Range(minZPos, maxZPos);
        var newPos = new Vector3(xPos, initialSwitchPos.y, zPos);
        switchButton.gameObject.transform.position = newPos;
        switchButton.gameObject.SetActive(true);
    }

    private void StartLollipopSpawn()
    {
        float lollipopSpawnTime = Random.Range(minLollipopSpawnTime, maxLollipopSpawnTime);
        StartCoroutine(SpawnLollipop(lollipopSpawnTime));
    }

    private void CheckLoss(int currentPlayerHealth)
    {
        if (currentPlayerHealth <= 0)
        {
            Time.timeScale = 0;
        }
    }

    private void CheckWin(int currentEnemyHealth)
    {
        if (currentEnemyHealth <= 0)
        {
            Time.timeScale = 0;
        }
    }

    IEnumerator SwitchRespawn()
    {
        yield return new WaitForSeconds(switchRespawnTime);
        UpdateSwitchPos();
    }
    IEnumerator SwitchCountdown()
    {
        yield return new WaitForSeconds(playerBatTime);
        switchTimeUpEvent?.Invoke();
    }
    IEnumerator SpawnLollipop(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        float xPos = Random.Range(minXPos, maxXPos);
        float zPos = Random.Range(minZPos, maxZPos);
        var newPos = new Vector3(xPos, initialSwitchPos.y, zPos);
        var lollipop = GameObject.Instantiate(lollipopPrefab);
        lollipop.transform.position = newPos;
        StartLollipopSpawn();
    }
}
