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
    private Switch switchButton;

    [SerializeField]
    private float maxXPos = 7.22f;
    [SerializeField]
    private float minXPos = -8.85f;
    [SerializeField]
    private float maxZPos = 5.5f;
    [SerializeField]
    private float minZPos = -10.41f;
    private Vector3 initialSwitchPos;

    public delegate void SwitchTimeUp();
    public event SwitchTimeUp switchTimeUpEvent;
    private void OnDisable()
    {
        switchButton.switchActivatedEvent -= SwitchActivated;
    }

    private void OnEnable()
    {
        switchButton.switchActivatedEvent += SwitchActivated;
    }
    void Start()
    {
        initialSwitchPos = switchButton.transform.position;
    }

    // Update is called once per frame
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
}
