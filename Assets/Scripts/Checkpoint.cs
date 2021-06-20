using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint
{
    public Vector2 position;
    public bool isFlipped = false;
    public Checkpoint(Vector2 position, bool isFlipped){
        this.position = position;
        this.isFlipped = isFlipped;
    }
}
