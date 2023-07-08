using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthIcon : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite fullSprite;
    [SerializeField]
    private Sprite damagedSprite;

    public void SetSprite(bool damaged)
    {
        image.sprite = damaged? damagedSprite: fullSprite;
    }
    
}
