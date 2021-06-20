using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettableItem : MonoBehaviour {
    public GrapeNumber grapeNumber;
    private void Awake() {
        if (GameManager.Instance.grapesGottem.Contains(grapeNumber)) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!GameManager.Instance.grapesGottem.Contains(grapeNumber)){
            GameManager.Instance.audioPlayer.pitch = 1;
            GameManager.Instance.audioPlayer.PlayOneShot(GameManager.Instance.soundEffects.getItem, 0.4f);
            GameManager.Instance.grapesGottem.Add(grapeNumber);
            if (GameManager.Instance.grapesGottem.Count == 10) {
                gameObject.GetComponent<SpriteRenderer>().sprite = null;
                GameManager.Instance.TradeItem(GameManager.Instance.uvas);
                StartCoroutine(other.gameObject.GetComponentInParent<CharacterController>().GotItem(GameManager.Instance.uvas.sprite, this.gameObject));
            } else {
                gameObject.SetActive(false);
            }
        }        
    }
}

public enum GrapeNumber {
    zero, one, two, three, four, five, six, seven, eight, nine
}