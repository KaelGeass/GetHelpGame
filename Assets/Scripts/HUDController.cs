using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image item1;
    public Image item2;

    void Start()
    {
        GameManager.Instance.OnVariableChange += UpdateHUD;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        if(GameManager.Instance.inventoryItems.Count > 0) {
            item1.overrideSprite = GameManager.Instance.inventoryItems[0].sprite;
        }
            
            
        if(GameManager.Instance.inventoryItems.Count > 1) {
            item2.overrideSprite = GameManager.Instance.inventoryItems[1].sprite;
        }
    }

    private void OnDestroy() {
        GameManager.Instance.OnVariableChange -= UpdateHUD;
    }
}
