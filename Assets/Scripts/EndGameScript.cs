using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    public Camera mainCamera;
    public CharacterController protagonist;
    public bool eventStarted = false;
    public Transform dog;
    public SpriteRenderer dogSprite;
    public Animator dogAnimator;
    public Transform positionToTakeBalde;
    GameManager gameManager;
    CameraFollow cameraFollow;
    public AudioClip musicaFinal;
    public GameObject HUD;
    public Animator hearts;

    const string ANIMATION_IDDLE = "IddleDog";
    const string ANIMATION_HAPPY = "Happy";
    const string ANIMATION_HEARTS = "Hearts";
    const string ANIMATION_HUG = "Hug";
    const string ANIMATION_INSIDE_BUCKET = "Up";
    const string ANIMATION_STAND = "Stand";
    const string ANIMATION_FINISH_POSE = "FinalPose";

    void Start()
    {
        gameManager = GameManager.Instance;
        cameraFollow = mainCamera.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!eventStarted) {
            eventStarted = true;
            if (gameManager.inventoryItems.Contains(gameManager.corda) &&
                gameManager.inventoryItems.Contains(gameManager.balde)) {
                StartCoroutine(EndGameEvent());
            }
        }
    }

    IEnumerator EndGameEvent() {
        gameManager.canMove = false;
        yield return StartCoroutine(PartOne());
        yield return StartCoroutine(PartTwo());
        yield return StartCoroutine(PartThree());
        yield return StartCoroutine(PartFour());
        GameManager.Instance.Reset();
        SceneManager.LoadScene("Menu");
        yield return null;
    }

    IEnumerator PartOne() {
        yield return new WaitForSeconds(1);
        cameraFollow.animator.Play("VerySlowCrossFadeStart");
        yield return StartCoroutine(protagonist.Walk(2f , 2));
        yield return new WaitForSeconds(1);
        protagonist.transform.position = positionToTakeBalde.position;
        yield return new WaitForSeconds(1);
        cameraFollow.animator.Play("VerySlowCrossFadeEnd");
        HUD.SetActive(false);
        yield return new WaitForSeconds(2);
        yield return null;
    }
    IEnumerator PartTwo() {
        cameraFollow.follow = false;
        yield return new WaitForSeconds(1);
        protagonist.audioSource.pitch = 1.2f;
        protagonist.audioSource.PlayOneShot(gameManager.soundEffects.bark, 0.4f);
        yield return new WaitForSeconds(0.4f);
        protagonist.audioSource.pitch = 1.35f;
        protagonist.audioSource.PlayOneShot(gameManager.soundEffects.bark, 0.4f);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(CameraMovement(2f , new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y -7, -5f)));
        cameraFollow.animator.Play("VerySlowCrossFadeStart");
        gameManager.musicPlayer.PlayOneShot(musicaFinal, 0.7f);
        gameManager.musicPlayer.loop = true;
        dogAnimator.Play(ANIMATION_STAND);
        yield return new WaitForSeconds(3);
        yield return null;
    }

    IEnumerator PartThree() {
        dogAnimator.Play(ANIMATION_INSIDE_BUCKET);
        cameraFollow.animator.Play("VerySlowCrossFadeEnd");
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(MoveDog(1.2f, new Vector3(dog.transform.position.x, -5f, dog.transform.position.z)));
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(MoveDog(1.2f, new Vector3(dog.transform.position.x, -3f, dog.transform.position.z)));
        yield return new WaitForSeconds(1.2f);
        yield return StartCoroutine(MoveDog(1.2f, new Vector3(dog.transform.position.x, -1f, dog.transform.position.z)));
        yield return new WaitForSeconds(1.5f);
        cameraFollow.animator.Play("VerySlowCrossFadeStart");
        yield return StartCoroutine(MoveDog(1.2f, new Vector3(dog.transform.position.x, 1f, dog.transform.position.z)));
        yield return null;
    }

        IEnumerator PartFour() {
        dog.gameObject.SetActive(false);
        protagonist.audioSource.pitch = 1.55f;
        protagonist.audioSource.PlayOneShot(gameManager.soundEffects.bark, 0.4f);
        yield return new WaitForSeconds(0.4f);
        protagonist.audioSource.PlayOneShot(gameManager.soundEffects.bark, 0.4f);
        protagonist.transform.position = positionToTakeBalde.transform.position;
        cameraFollow.transform.position = new Vector3(protagonist.transform.position.x, protagonist.transform.position.y, -2);
        cameraFollow.animator.Play("CrossFadeEnd");
        protagonist.animator.Play(ANIMATION_HUG);
        hearts.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        protagonist.animator.Play(ANIMATION_FINISH_POSE);
        yield return new WaitForSeconds(3);
        cameraFollow.animator.Play("CrossFadeStart");
        StartCoroutine(gameManager.soundEffects.FadeOutStart(gameManager.musicPlayer, 2f));
        yield return new WaitForSeconds(3);
        yield return null;
    }

    public IEnumerator MoveDog(float duration, Vector3 distance) {
        float moveTime = 0;
        while (moveTime <= duration)
        {
            moveTime += Time.deltaTime / duration;
            dog.transform.position = Vector3.Lerp(dog.transform.position, distance, moveTime * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
     IEnumerator CameraMovement(float duration, Vector3 position) {
        float moveTime = 0;
        while (moveTime + 0.1f <= duration)
        {
            moveTime += Time.deltaTime / duration;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, position, moveTime * Time.deltaTime);
            yield return null;
        }
        yield return null;
     }
}
