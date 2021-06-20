using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public bool follow = true;
    public float smoothSpeed = 0.125f;
    public int offset = -5;
    Vector3 desiredPosition = new Vector3();
    Vector3 smoothedPosition = new Vector3();
    public Animator animator;

    public float leftCameraLimit;
    public float rightCameraLimit;

    public float topCameraLimit;
    public float botCameraLimit;

    void Start() {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, leftCameraLimit, rightCameraLimit), transform.position.y, offset);
        desiredPosition = new Vector3(target.position.x, transform.position.y, offset);
        if (GameManager.Instance.currentLevel == Level.Floresta){
            smoothSpeed = 6;
        }
        else {
            smoothSpeed = 4;
        }
    }

    void FixedUpdate() {
        if (GameManager.Instance.currentLevel == Level.Floresta){
            desiredPosition = new Vector3(
                Mathf.Clamp(target.position.x, leftCameraLimit, rightCameraLimit), 
                Mathf.Clamp(target.position.y, botCameraLimit, topCameraLimit), 
                offset
            );
        } else {
            desiredPosition = new Vector3(
                Mathf.Clamp(target.position.x, leftCameraLimit, rightCameraLimit), 
                this.transform.position.y, 
                offset
            );
        }
        if (follow) {
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}