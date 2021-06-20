using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour{

    public SpriteRenderer fogo;
    void Start()
    {
        GameManager.Instance.OnIsqueiroTaken += UpdateSprite;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if(GameManager.Instance.isFireOn){
            fogo.enabled = true;
        } else{
            fogo.enabled = false;
        }
    }

    private void OnDestroy() {
        GameManager.Instance.OnIsqueiroTaken -= UpdateSprite;
    }
}
