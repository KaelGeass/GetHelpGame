using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetIsqueiro : MonoBehaviour
{
    public SpriteRenderer isqueiro;
    public CharacterController protagonist;
    public bool isColliding = false;
    
    void Start(){
        if (!GameManager.Instance.isqueiroVisivel) {
         this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        isColliding = true;
    }
    private void OnTriggerExit2D(Collider2D other) {
        isColliding = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && isColliding == true && !GameManager.Instance.inventoryItems.Contains(GameManager.Instance.isqueiro)) {
            isColliding = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
            GameManager.Instance.TradeItem(GameManager.Instance.isqueiro);
            StartCoroutine(protagonist.GotItem(GameManager.Instance.isqueiro.sprite, this.gameObject));
            GameManager.Instance.isqueiroVisivel = false;
        }
    }
}
