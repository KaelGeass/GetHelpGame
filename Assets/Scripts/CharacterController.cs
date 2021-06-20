using System;
using System.Collections;
using UnityEngine;
public class CharacterController : MonoBehaviour
{
    public Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    public AudioSource audioSource;
    CapsuleCollider2D groundCheck;
    public SpriteRenderer sprite;
    public SpriteRenderer balaoFala;
    public SpriteRenderer balaoPensamento;
    public SpriteRenderer leftIcon;
    public SpriteRenderer centerIconFront;
    public SpriteRenderer centerIconBack;
    public SpriteRenderer rightIcon;
    public SpriteRenderer topHeadItem;
    [SerializeField]
    bool isGrounded = false;
    public Assets assets;
    float horizontalVelocityMultiplayer = 0;
    [SerializeField] public float horizontalSpeed = 6f;
    float verticalSpeed = 16f;
    public Animator animator;
    public Transform leftPosition;
    public Transform rightPosition;
    public float absoluteHorizontalVelocity;
    public Transform originalCenterIconFrontTransform;
    public Transform originalCenterIconBackTransform;
    public Transform originalLeftIconTransform;
    public Transform originalRightIconTransform;
    GameManager gameManager;
    GameObject mainCamera;
    CameraFollow cameraFollow;
    Vector3 growIconStep = new Vector3(0.02f, 0.02f);

    void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraFollow = mainCamera.GetComponent<CameraFollow>();
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        groundCheck = GetComponentInChildren<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        this.transform.position = SetInicialPosition().position;
        originalCenterIconFrontTransform = centerIconFront.transform;
        originalCenterIconBackTransform = centerIconBack.transform;
        originalLeftIconTransform = leftIcon.transform;
        originalRightIconTransform = rightIcon.transform;
    }

    void Update(){
        absoluteHorizontalVelocity = Mathf.Abs(rb.velocity.x);
        if (gameManager.canMove){
            horizontalVelocityMultiplayer = Input.GetAxisRaw("Horizontal");
        }
        else {
            horizontalVelocityMultiplayer = 0;
        }
        if (rb.velocity.x > 0.1) {
            sprite.flipX = false;
        }
        else if(rb.velocity.x < -0.1) {
            sprite.flipX = true;
        }
        animator.SetFloat("Speed", absoluteHorizontalVelocity);
        animator.SetBool("isJumping", !isGrounded);
    }
    
    private void OnCollisionStay2D(Collision2D other) {
        if (groundCheck.IsTouching(other.collider)) {
            isGrounded = true;
        }
    }

    void FixedUpdate()
    {   if (gameManager.canMove){ 
            if (Mathf.Abs(rb.velocity.y) > 1) {
                isGrounded = false;
            }
            if (Input.GetKey(KeyCode.UpArrow) && isGrounded && gameManager.canJump){
                if (!audioSource.isPlaying) {
                    audioSource.pitch = SoundEffects.GetPitchVariation(0.9f, 1.1f);
                    audioSource.PlayOneShot(gameManager.soundEffects.jump, 0.4f);
                }
                rb.velocity = new Vector2 (horizontalSpeed * horizontalVelocityMultiplayer, verticalSpeed);
            }
            else {
                rb.velocity = new Vector2 (horizontalSpeed * horizontalVelocityMultiplayer, rb.velocity.y);
            } 
            if (absoluteHorizontalVelocity > 0.3*horizontalSpeed && !audioSource.isPlaying && gameManager.currentLevel == Level.Menu){
                audioSource.Play();
            }
            if (rb.velocity.y > 20){
                rb.velocity = new Vector2 (rb.velocity.x, 20);
            }
        }
    }

    public IEnumerator GotItem(Sprite sprite, GameObject gameObjectToDisable = null) {
        gameManager.canMove = false;
        rb.velocity = Vector2.zero;
        topHeadItem.sprite = sprite; 
        audioSource.pitch = 1;
        audioSource.PlayOneShot(gameManager.soundEffects.fanfare);
        animator.Play("Fanfare");
        yield return new WaitForSeconds(3f);
        animator.Play("Player_Iddle");
        topHeadItem.sprite = null;
        gameManager.canMove = true;
        if (gameObjectToDisable != null) { gameObjectToDisable.SetActive(false); }
        yield return null;
    }

    IEnumerator BeginGameEvent(){
        gameManager.canMove = false;
        cameraFollow.animator.Play("VerySlowCrossFadeEnd");
        animator.Play("Player_look_down");
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(Walk(1.5f , -2));
        rb.velocity = new Vector2(1f, 0);
        yield return ShowThreeIconsProtagonistThinkingBaloon(assets.corda, assets.plus, assets.balde);
        yield return WaitButtomPress();
        yield return StartCoroutine(HideProtagonistIcons());
        gameManager.canMove = true;
        yield return null;
    }

    public static IEnumerator WaitButtomPress() {
        while (!Input.anyKeyDown)
        yield return null;
    } 
    
    public IEnumerator Walk(float secondsWalking, float xSpeed) {
        animator.Play("Player_Iddle");
        if (xSpeed > 0)
            sprite.flipX = false;
        else
            sprite.flipX = true;

        float walkTime = 0;
        Vector2 speedVector = new Vector2(xSpeed,0);
        while (walkTime < secondsWalking)
       {
            walkTime += Time.deltaTime / secondsWalking;
            rb.velocity = speedVector;
            yield return null;
      }
        yield return null;
    }

    IEnumerator ShowOneIconProtagonistThinkingBaloon(Sprite centerSprite) {
        this.centerIconFront.sprite = centerSprite;
        StartCoroutine(GrowIcon(balaoPensamento,true, true));
        yield return StartCoroutine(GrowIcon(centerIconFront, true, false));
        yield return new WaitForSeconds(1);
    }

        IEnumerator ShowThreeIconsProtagonistThinkingBaloon(Sprite leftSprite, Sprite centerSprite, Sprite rightSprite) {
        centerIconFront.sprite = centerSprite;
        rightIcon.sprite = rightSprite;
        leftIcon.sprite = leftSprite;
        StartCoroutine(GrowIcon(balaoPensamento,true, true));
        StartCoroutine(GrowIcon(leftIcon, false, false));
        StartCoroutine(GrowIcon(rightIcon, false, false));
        yield return StartCoroutine(GrowIcon(centerIconFront, false, false));
        yield return new WaitForSeconds(1);
    }

    IEnumerator GrowIcon(SpriteRenderer icon, bool isBig, bool isFast){
        int limit = isBig ? 22 : 14;
        float  stepTime = isFast ? 0.002f : 0.005f; 
        icon.transform.localScale = Vector3.zero;
        for(int i = 0; i < limit; i+=2) {
            icon.transform.localScale += growIconStep;
            yield return new WaitForSeconds(stepTime);
        }   
        yield return null;
    }

    IEnumerator VanishIcon(SpriteRenderer icon) {
        for(int i = 0; icon.transform.localScale.x > 0; i+=2){
            icon.transform.localScale -= growIconStep;
            yield return new WaitForSeconds(0.002f);
        }   
        yield return null;
    }

    IEnumerator HideProtagonistIcons(){
        centerIconBack.transform.position = originalCenterIconBackTransform.position;
        centerIconBack.transform.rotation = originalCenterIconBackTransform.rotation;
        centerIconBack.transform.localScale = Vector3.zero;
        centerIconFront.transform.position = originalCenterIconFrontTransform.position;
        centerIconFront.transform.rotation = originalCenterIconFrontTransform.rotation;
        centerIconFront.transform.localScale = Vector3.zero;
        rightIcon.transform.position = originalRightIconTransform.position;
        rightIcon.transform.rotation = originalRightIconTransform.rotation;
        rightIcon.transform.localScale = Vector3.zero;
        leftIcon.transform.position = originalLeftIconTransform.position;
        leftIcon.transform.rotation = originalLeftIconTransform.rotation;
        leftIcon.transform.localScale = Vector3.zero;
        yield return StartCoroutine(VanishIcon(balaoPensamento));
    }

    private Transform SetInicialPosition(){
        gameManager.canJump = false;
        horizontalSpeed = 6;
        switch(gameManager.currentLevel) {
            case Level.Menu:
                return leftPosition;
            case Level.Cemiterio:
            if (gameManager.lastLevel == Level.Menu){
                StartCoroutine(BeginGameEvent());
                return rightPosition;
            }
            else {
                return leftPosition;
            }
            case Level.Cidade:
            if (gameManager.lastLevel == Level.Floresta){
                return leftPosition;
            }
            else {
                sprite.flipX = true;
                return rightPosition;
            }
            case Level.Floresta:
            horizontalSpeed = 9;
            gameManager.lastCheckpoint.position = leftPosition.transform.position;
            gameManager.lastCheckpoint.isFlipped = false;
            gameManager.canJump = true;
            if (gameManager.lastLevel == Level.Acampamento){
                return leftPosition;
            }
            else {
            gameManager.lastCheckpoint.position = rightPosition.transform.position;
            gameManager.lastCheckpoint.isFlipped = true;
                sprite.flipX = true;
                return rightPosition;
            }
            case Level.Acampamento:
            sprite.flipX = true;
                return rightPosition;
            default: 
                return this.transform;
        }
    }
}