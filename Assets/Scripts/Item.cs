using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {
    public Item (string name, Sprite sprite){
        this.name = name;
        this.sprite = sprite;
    }
    public string name;
    public Sprite sprite;
}
