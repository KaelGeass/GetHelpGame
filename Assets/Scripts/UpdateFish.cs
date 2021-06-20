using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpdateFish : MonoBehaviour
{
    public Animator fishAnimator;

    void Update() {
        UpdateFishAnim();
    }

    public void UpdateFishAnim(){
        if (GameManager.Instance.tradedItems.Contains(GameManager.Instance.isqueiro)) {
            fishAnimator.SetBool("HasBalde", false);
        }
    }
}
