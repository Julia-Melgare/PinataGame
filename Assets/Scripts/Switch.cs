using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public delegate void SwitchActivated();
    public event SwitchActivated switchActivatedEvent;
    public void DisableSwitch()
    {
        switchActivatedEvent?.Invoke();
        this.gameObject.SetActive(false);
    }
}
