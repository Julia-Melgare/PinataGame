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

    [SerializeField]
    private GameObject corre;
    [SerializeField]
    private GameObject pega;
    [SerializeField]
    private GameObject goblinSounds;
    
    [SerializeField]
    private AudioSource oofPinata;
    [SerializeField]
    private AudioSource oofGoblin;
    [SerializeField]
    private AudioSource healing;
    [SerializeField]
    private AudioSource pinataDeath;
    [SerializeField]
    private AudioSource goblinDeath;
    [SerializeField]
    private AudioSource victory;
    [SerializeField]
    private AudioSource defeat;
    

    public delegate void SwitchTimeUp();
    public event SwitchTimeUp switchTimeUpEvent;
    private void OnDisable()
    {
        switchButton.switchActivatedEvent -= SwitchActivated;
        player.onHealthChange -= CheckLoss;
        player.onHealthLoss -= PlayOofPinata;
        player.onHealthGain -= PlayHealing;
        enemy.onHealthChange -= CheckWin;
        enemy.onHealthLoss -= PlayOofGoblin;
    }

    private void OnEnable()
    {
        switchButton.switchActivatedEvent += SwitchActivated;
        player.onHealthChange += CheckLoss;
        player.onHealthLoss += PlayOofPinata;
        player.onHealthGain += PlayHealing;
        enemy.onHealthChange += CheckWin;
        enemy.onHealthLoss += PlayOofGoblin;
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
        corre.SetActive(false);
        pega.SetActive(true);
        goblinSounds.SetActive(false);
        StartCoroutine(SwitchCountdown());
        StartCoroutine(SwitchRespawn());
    }

    private void UpdateSwitchPos()
    {
        float xPos = Random.Range(minXPos, maxXPos);
        float zPos = Random.Range(minZPos, maxZPos);
        var newPos = new Vector3(xPos, initialSwitchPos.y, zPos);
        while(Physics.CheckSphere(newPos, 5.0f, LayerMask.NameToLayer("Obstacle")))
        {
            xPos = Random.Range(minXPos, maxXPos);
            zPos = Random.Range(minZPos, maxZPos);
            newPos = new Vector3(xPos, initialSwitchPos.y, zPos);
        }
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
            pega.SetActive(false);
            corre.SetActive(false);
            pinataDeath.Play();
            defeat.Play();
            Time.timeScale = 0;
        }
    }

    private void PlayOofPinata()
    {
        oofPinata.Play();
    }    

    private void PlayOofGoblin()
    {
        oofGoblin.Play();
    } 

    private void PlayHealing()
    {
        healing.Play();
    } 

    private void CheckWin(int currentEnemyHealth)
    {
        if (currentEnemyHealth <= 0)
        {
            pega.SetActive(false);
            corre.SetActive(false);
            goblinDeath.Play();
            victory.Play();
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
        pega.SetActive(false);
        corre.SetActive(true);
        goblinSounds.SetActive(true);
        switchTimeUpEvent?.Invoke();
    }
    IEnumerator SpawnLollipop(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        float xPos = Random.Range(minXPos, maxXPos);
        float zPos = Random.Range(minZPos, maxZPos);
        var newPos = new Vector3(xPos, initialSwitchPos.y, zPos);
        while(Physics.CheckSphere(newPos, 5.0f, LayerMask.NameToLayer("Obstacle")))
        {
            xPos = Random.Range(minXPos, maxXPos);
            zPos = Random.Range(minZPos, maxZPos);
            newPos = new Vector3(xPos, initialSwitchPos.y, zPos);
        }
        var lollipop = GameObject.Instantiate(lollipopPrefab);
        lollipop.transform.position = newPos;
        StartLollipopSpawn();
    }
}
