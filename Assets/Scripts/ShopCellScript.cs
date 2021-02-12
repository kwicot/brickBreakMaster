using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCellScript : MonoBehaviour
{
    public int ID;
    public string Name;
    public string Discriptions;
    public float Size;
    public float Speed;
    public float Price;
    Text text;
    Image icon;
    public Sprite sprite;
    void Start()
    {
        text = GetComponentInChildren<Text>();
        icon = GetComponentInChildren<Image>();
        icon.sprite = sprite;
    }

    void Update()
    {
        
    }

    public void SetText(string otherText)
    {
        text.text = otherText;
    }
}
