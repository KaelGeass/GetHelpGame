using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected void Death(CharacterController player) {
        GameManager.Instance.audioPlayer.pitch = SoundEffects.GetPitchVariation(0.9f, 1.1f);
        GameManager.Instance.audioPlayer.PlayOneShot(GameManager.Instance.soundEffects.damage);
        player.transform.position = GameManager.Instance.lastCheckpoint.position;
        player.sprite.flipX = GameManager.Instance.lastCheckpoint.isFlipped;
    }
}
