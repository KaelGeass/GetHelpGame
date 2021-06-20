using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyEnemy : Enemy
{
    public float velocityValueX = 3;
    public float velocityValueY = 4;
    float verticalDirection = 1;
    public float initialPositionX;
    public float initialPositionY;
    public Vector2 velocityVector;
    Rigidbody2D rb;
    SpriteRenderer sprite;

 void Awake()
    {
        initialPositionX = transform.position.x;
        initialPositionY = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
        velocityVector = new Vector2(velocityValueX, velocityValueY);
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        //HORIZONTAL
        if (this.transform.position.x > initialPositionX + 7 || 
            this.transform.position.x < initialPositionX - 7) {
            velocityValueX *= -1;
        }
        
        //VERTICAL
        if (this.transform.position.x < initialPositionX + 3 && 
            this.transform.position.x > initialPositionX - 3) {
            if (rb.velocity.x > 0 && this.transform.position.x > initialPositionX ||
                rb.velocity.x < 0 && this.transform.position.x < initialPositionX) {
                verticalDirection = 1;
            }
            else if(rb.velocity.x < 0 && this.transform.position.x > initialPositionX ||
                rb.velocity.x > 0 && this.transform.position.x < initialPositionX) {
                verticalDirection = -1;
            }
        }
        else {
            verticalDirection = 0;
        }        
        rb.velocity = new Vector2(velocityValueX, velocityValueY * verticalDirection);

            if (rb.velocity.x < 0 ){
                sprite.flipX = false;
            }
            else {
                sprite.flipX = true;
            }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Protagonist"){
            Death(other.gameObject.GetComponent<CharacterController>());
        }        
    }
}
