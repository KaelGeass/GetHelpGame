using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    public float velocityValue = 2;
    public float initialPositionX;
    public Vector2 velocityVector;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    public int distanceToWalk = 5;
    
    void Awake()
    {
        initialPositionX = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        velocityVector = new Vector2(velocityValue, 0);
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (this.transform.position.x > initialPositionX + distanceToWalk || 
            this.transform.position.x < initialPositionX - distanceToWalk) {
            velocityVector *= -1;
            if (rb.velocity.x < 0 ){
                sprite.flipX = false;
            }
            else {
                sprite.flipX = true;
            }
        }
        rb.velocity = velocityVector;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Protagonist"){
            Death(other.gameObject.GetComponent<CharacterController>());
        }        
    }
}
