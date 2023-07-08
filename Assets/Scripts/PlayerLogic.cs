using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private GameObject bat;
    private bool hasBat = false;
    private int life = 3;
    private CharacterController controller = null;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        bat.SetActive(hasBat);
        Move();
    }

    private void Move()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Switch")
        {
            hasBat = true;
        }
    }
}
