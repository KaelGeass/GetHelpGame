using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofEnemy : Enemy
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Protagonist"){
            Death(other.gameObject.GetComponent<CharacterController>());
        }        
    }
}
