using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        GameManager.Instance.audioPlayer.PlayOneShot(GameManager.Instance.soundEffects.jumpAnim);
    }
}
