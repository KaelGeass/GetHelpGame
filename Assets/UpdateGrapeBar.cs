using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGrapeBar : MonoBehaviour
{
    public Image fill;
    void Start() {
        if (GameManager.Instance.currentLevel != Level.Floresta){
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        fill.transform.localScale = new Vector2(1, 0.1f * GameManager.Instance.grapesGottem.Count);
    }
}
