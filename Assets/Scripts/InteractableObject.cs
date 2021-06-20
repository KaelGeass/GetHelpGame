using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool canInteract = false;
    public Assets assets;
    public Animator animator;
    public SpriteRenderer personagem;
    public SpriteRenderer balaoFala;
    public SpriteRenderer leftIcon;
    public SpriteRenderer centerIcon;
    public SpriteRenderer rightIcon;
    public Interactables im;
    public InteractableState state = InteractableState.Request;
    public InteractablesAnim approachAnim;
    public InteractablesAnim iddleAnim;
    GameObject mainCharacter;
    CharacterController protagonist;
    Transform originalCenterIconTransform;
    Transform originalLeftIconTransform;
    Transform originalRightIconTransform;
    Vector3 growIconStep = new Vector3(0.02f, 0.02f);
    GameManager gameManager;

    void Start(){
        mainCharacter = GameObject.FindGameObjectWithTag("Protagonist");
        protagonist = mainCharacter.GetComponent<CharacterController>();
        originalCenterIconTransform = centerIcon.transform;
        originalLeftIconTransform = leftIcon.transform;
        originalRightIconTransform = rightIcon.transform;
        gameManager = GameManager.Instance;
        
        state = gameManager.GetInteractableState(im);
         if (im == Interactables.Kid && state != InteractableState.Finished) {
                iddleAnim = InteractablesAnim.JumpLoop;
            }
        else {
            iddleAnim = InteractablesAnim.Idle;
        }
        animator.Play(iddleAnim.ToString());
    }

    void Update() {
        if (canInteract) {
            state = gameManager.GetInteractableState(im);
            if (Input.GetKeyDown(KeyCode.Space)) {
                canInteract = false;
                gameManager.canMove = false;
                protagonist.rb.velocity = Vector2.zero;
                switch(im) {
                    case Interactables.Machine:
                    StartCoroutine(MachineEvents());
                    break;
                    case Interactables.Kid:
                    StartCoroutine(KidEvents());
                    break;
                    case Interactables.Fisher:
                    StartCoroutine(FisherEvents());
                    break;
                    case Interactables.Seller:
                    StartCoroutine(SellerEvents());
                    break;
                    case Interactables.Whatever:
                    StartCoroutine(WhateverEvents());
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        state = gameManager.GetInteractableState(im);
        if (im == Interactables.Machine && state == InteractableState.Finished) {
            return;
        }
        animator.Play(approachAnim.ToString());
        canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        state = gameManager.GetInteractableState(im);
        if (im == Interactables.Kid && state != InteractableState.Finished) {
            animator.Play(InteractablesAnim.JumpLoop.ToString());
        }
        canInteract = false;
    }

    IEnumerator MachineEvents() {
        switch (state) {
            case InteractableState.Request:
                yield return StartCoroutine(ShowThreeIconsCharacterBaloon(assets.moedas, assets.trade, assets.doce));
                StartCoroutine(HideCharacterIcons());
                protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.8f, 1.2f);
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.wrong, 0.4f);
                yield return ShowFrontBackProtagonistBaloon(assets.x, assets.moedas);
                StartCoroutine(HideProtagonistIcons());
                break;
            case InteractableState.Trade:
                yield return StartCoroutine(ShowThreeIconsCharacterBaloon(assets.moedas, assets.trade, assets.doce));
                StartCoroutine(HideCharacterIcons());
                protagonist.audioSource.pitch = 1;
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.coin, 0.4f);
                gameManager.TradeItem(gameManager.doce, gameManager.moedas);
                yield return new WaitForSeconds(1f);
                yield return StartCoroutine(protagonist.GotItem(assets.doce));
                break;
            case InteractableState.Finished:
                yield return StartCoroutine(ShowThreeIconsCharacterBaloon(assets.moedas, assets.trade, assets.doce));
                StartCoroutine(HideCharacterIcons());
                protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.8f, 1.2f);
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.wrong, 0.4f);
                yield return ShowFrontBackProtagonistBaloon(assets.x, assets.moedas);
                StartCoroutine(HideProtagonistIcons());
                break;
            default: 
                break;
        }
        EndEvent();
        yield return null;
    }
    IEnumerator KidEvents() {
        switch (state) {
            case InteractableState.Request:
                yield return ShowTwoIconsProtagonistBaloon(assets.corda, assets.question);
                StartCoroutine(HideProtagonistIcons());
                protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.8f, 1.2f);
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.wrong, 0.4f);
                yield return StartCoroutine(ShowOneIconCharacterBaloon(assets.angry));
                StartCoroutine(HideCharacterIcons());
                yield return ShowOneIconProtagonistBaloon(assets.sad);
                StartCoroutine(HideProtagonistIcons());
            break;
            case InteractableState.Trade:
               yield return ShowThreeIconsProtagonistBaloon(assets.corda, assets.trade, assets.doce);
                StartCoroutine(HideProtagonistIcons());
                protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.8f, 1.2f);
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.right, 0.4f);
                yield return StartCoroutine(ShowOneIconCharacterBaloon(assets.happy));
                StartCoroutine(HideCharacterIcons());
                gameManager.TradeItem(gameManager.corda, gameManager.doce);
                yield return StartCoroutine(protagonist.GotItem(assets.corda));
            break;
            case InteractableState.Finished:
            protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.8f, 1.2f);
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.right, 0.4f);
                yield return StartCoroutine(ShowOneIconCharacterBaloon(assets.happy));
                StartCoroutine(HideCharacterIcons());
            break;
            default:
                yield return null;
            break;
        }
        EndEvent();
        yield return null;
    }
    IEnumerator FisherEvents() {
        switch (state) {
            case InteractableState.Request:
                gameManager.isqueiroVisivel = true;
                yield return ShowTwoIconsProtagonistBaloon(assets.balde, assets.question);
                StartCoroutine(HideProtagonistIcons());
                yield return StartCoroutine(ShowOneIconCharacterBaloon(assets.sad));
                yield return StartCoroutine(ShowThreeIconsCharacterBaloon(assets.isqueiro, assets.trade, assets.balde));
                StartCoroutine(HideCharacterIcons());
                protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.8f, 1.2f);
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.wrong, 0.4f);
                yield return ShowFrontBackProtagonistBaloon(assets.x, assets.isqueiro);
                StartCoroutine(HideProtagonistIcons());
                yield return StartCoroutine(ShowThreeIconsCharacterBaloon(assets.isqueiro, assets.arrow, assets.cemiterio));
                StartCoroutine(HideCharacterIcons());
            break;
            case InteractableState.Trade:
                yield return StartCoroutine(ShowThreeIconsCharacterBaloon(assets.isqueiro, assets.trade, assets.balde));
                yield return StartCoroutine(HideCharacterIcons());
                protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.8f, 1.2f);
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.right, 0.4f);
                yield return StartCoroutine(ShowOneIconCharacterBaloon(assets.happy));
                StartCoroutine(HideCharacterIcons());
                gameManager.TradeItem(gameManager.balde, gameManager.isqueiro);
                yield return StartCoroutine(protagonist.GotItem(assets.balde));
            break;
            case InteractableState.Finished:
                yield return StartCoroutine(IgnorePlayer());
            break;
            default:
                yield return null;
            break;
        }
        EndEvent();
        yield return null;
    }
    IEnumerator SellerEvents() {
        switch (state) {
            case InteractableState.Request:
                yield return StartCoroutine(ShowThreeIconsCharacterBaloon(assets.uvas, assets.trade, assets.moedas));
                StartCoroutine(HideCharacterIcons());
                protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.8f, 1.2f);
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.wrong, 0.4f);
                yield return ShowFrontBackProtagonistBaloon(assets.x, assets.uvas);
                StartCoroutine(HideProtagonistIcons());
                yield return StartCoroutine(ShowThreeIconsCharacterBaloon(assets.uvas, assets.arrow, assets.floresta));
                StartCoroutine(HideCharacterIcons());
            break;
            case InteractableState.Trade:
                yield return StartCoroutine(ShowThreeIconsCharacterBaloon(assets.uvas, assets.trade, assets.moedas));
                yield return StartCoroutine(HideCharacterIcons());
                protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.8f, 1.2f);
                protagonist.audioSource.PlayOneShot(gameManager.soundEffects.right, 0.4f);
                yield return StartCoroutine(ShowOneIconCharacterBaloon(assets.happy));
                StartCoroutine(HideCharacterIcons());
                yield return StartCoroutine(protagonist.GotItem(assets.moedas));
                gameManager.TradeItem(gameManager.moedas, gameManager.uvas);
            break;
            case InteractableState.Finished:
                yield return StartCoroutine(IgnorePlayer());
            break;
            default:
                yield return null;
            break;
        }
        EndEvent();
        yield return null;
    }

     IEnumerator WhateverEvents() {
        yield return StartCoroutine(IgnorePlayer());
        EndEvent();
        yield return null;
    }

    IEnumerator IgnorePlayer(){
        yield return StartCoroutine(ShowOneIconCharacterBaloonIgnore(assets.ignored));
        protagonist.audioSource.pitch = SoundEffects.GetPitchVariation(0.7f, 1.2f);
        protagonist.audioSource.PlayOneShot(gameManager.soundEffects.wrong, 0.4f);
        yield return new WaitForSeconds(0.05f);
        centerIcon.transform.Rotate(new Vector3(0,0,150));
        yield return new WaitForSeconds(0.1f);
        centerIcon.transform.Rotate(new Vector3(0,0,150));
        yield return new WaitForSeconds(0.1f);
        centerIcon.transform.Rotate(new Vector3(0,0,150));
        yield return new WaitForSeconds(0.1f);
        centerIcon.transform.Rotate(new Vector3(0,0,150));
        yield return new WaitForSeconds(0.1f);
        centerIcon.transform.Rotate(new Vector3(0,0,150));
        yield return new WaitForSeconds(0.1f);
        centerIcon.transform.Rotate(new Vector3(0,0,150));
        yield return new WaitForSeconds(0.1f);
        centerIcon.transform.Rotate(new Vector3(0,0,150));
        yield return new WaitForSeconds(0.1f);
        centerIcon.transform.Rotate(new Vector3(0,0,150));
        yield return new WaitForSeconds(0.1f);
    }

    public static IEnumerator WaitSpacePress() {
        while (!Input.anyKeyDown)
        yield return null;
    } 

    IEnumerator GrowIcon(SpriteRenderer icon, bool isBig, bool isFast){
        int limit = isBig ? 22 : 14;
        float  stepTime = isFast ? 0.002f : 0.005f; 
        icon.transform.localScale = Vector3.zero;
        for(int i = 0; i < limit; i+=2){
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

    IEnumerator HideCharacterIcons(){
        centerIcon.transform.position = originalCenterIconTransform.position;
        centerIcon.transform.rotation = originalCenterIconTransform.rotation;
        centerIcon.transform.localScale = Vector3.zero;

        rightIcon.transform.position = originalRightIconTransform.position;
        rightIcon.transform.rotation = originalRightIconTransform.rotation;
        rightIcon.transform.localScale = Vector3.zero;
        
        leftIcon.transform.position = originalLeftIconTransform.position;
        leftIcon.transform.rotation = originalLeftIconTransform.rotation;
        leftIcon.transform.localScale = Vector3.zero;
        yield return StartCoroutine(VanishIcon(balaoFala));
    }

    IEnumerator HideProtagonistIcons(){
        protagonist.centerIconBack.transform.position = protagonist.originalCenterIconBackTransform.position;
        protagonist.centerIconBack.transform.rotation = protagonist.originalCenterIconBackTransform.rotation;
        protagonist.centerIconBack.transform.localScale = Vector3.zero;

        protagonist.centerIconFront.transform.position = protagonist.originalCenterIconFrontTransform.position;
        protagonist.centerIconFront.transform.rotation = protagonist.originalCenterIconFrontTransform.rotation;
        protagonist.centerIconFront.transform.localScale = Vector3.zero;

        protagonist.rightIcon.transform.position = protagonist.originalRightIconTransform.position;
        protagonist.rightIcon.transform.rotation = protagonist.originalRightIconTransform.rotation;
        protagonist.rightIcon.transform.localScale = Vector3.zero;

        protagonist.leftIcon.transform.position = protagonist.originalLeftIconTransform.position;
        protagonist.leftIcon.transform.rotation = protagonist.originalLeftIconTransform.rotation;
        protagonist.leftIcon.transform.localScale = Vector3.zero;
        yield return StartCoroutine(VanishIcon(protagonist.balaoFala));
    }

    //CHARACTER
    IEnumerator ShowOneIconCharacterBaloonIgnore(Sprite centerSprite) {
        this.centerIcon.sprite = centerSprite;
        StartCoroutine(GrowIcon(balaoFala,true, true));
        yield return StartCoroutine(GrowIcon(centerIcon, true, false));
        
    }
    IEnumerator ShowOneIconCharacterBaloon(Sprite centerSprite) {
        this.centerIcon.sprite = centerSprite;
        StartCoroutine(GrowIcon(balaoFala,true, true));
        yield return StartCoroutine(GrowIcon(centerIcon, true, false));
        yield return StartCoroutine(WaitSpacePress());
    }

    IEnumerator ShowTwoIconsCharacterBaloon(Sprite centerSprite, Sprite rightSprite) {
        this.centerIcon.sprite = centerSprite;
        this.rightIcon.sprite = rightSprite;
        StartCoroutine(GrowIcon(balaoFala,true, true));
        StartCoroutine(GrowIcon(rightIcon, false, false));
        yield return StartCoroutine(GrowIcon(centerIcon, true, false));
        yield return StartCoroutine(WaitSpacePress());
    }

    IEnumerator ShowThreeIconsCharacterBaloon(Sprite leftSprite, Sprite centerSprite, Sprite rightSprite) {
        this.centerIcon.sprite = centerSprite;
        this.rightIcon.sprite = rightSprite;
        this.leftIcon.sprite = leftSprite;
        StartCoroutine(GrowIcon(balaoFala,true, true));
        StartCoroutine(GrowIcon(leftIcon, false, false));
        StartCoroutine(GrowIcon(rightIcon, false, false));
        yield return StartCoroutine(GrowIcon(centerIcon, false, false));
        yield return StartCoroutine(WaitSpacePress());
    }

    //PROTAGONIST

    IEnumerator ShowFrontBackProtagonistBaloon(Sprite centerFrontSprite, Sprite centerBackSprite) {
        this.protagonist.centerIconBack.sprite = centerBackSprite;
        this.protagonist.centerIconFront.sprite = centerFrontSprite;
        StartCoroutine(GrowIcon(protagonist.balaoFala, true, true));
        StartCoroutine(GrowIcon(protagonist.centerIconBack, true, false));
        yield return StartCoroutine(GrowIcon(protagonist.centerIconFront, true, false));
        protagonist.centerIconFront.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        protagonist.centerIconFront.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        protagonist.centerIconFront.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        protagonist.centerIconFront.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        protagonist.centerIconFront.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        protagonist.centerIconFront.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(WaitSpacePress());
    }

    IEnumerator ShowOneIconProtagonistBaloon(Sprite centerSprite) {
        this.protagonist.centerIconFront.sprite = centerSprite;
        StartCoroutine(GrowIcon(protagonist.balaoFala,true, true));
        yield return StartCoroutine(GrowIcon(protagonist.centerIconFront, true, false));
        yield return StartCoroutine(WaitSpacePress());
    }

    IEnumerator ShowTwoIconsProtagonistBaloon(Sprite centerSprite, Sprite rightSprite) {
        this.protagonist.centerIconFront.sprite = centerSprite;
        this.protagonist.rightIcon.sprite = rightSprite;
        StartCoroutine(GrowIcon(protagonist.balaoFala,true, true));
        StartCoroutine(GrowIcon(protagonist.rightIcon, false, false));
        yield return StartCoroutine(GrowIcon(protagonist.centerIconFront, true, false));
        yield return StartCoroutine(WaitSpacePress());
    }

    IEnumerator ShowThreeIconsProtagonistBaloon(Sprite leftSprite, Sprite centerSprite, Sprite rightSprite) {
        this.protagonist.centerIconFront.sprite = centerSprite;
        this.protagonist.rightIcon.sprite = rightSprite;
        this.protagonist.leftIcon.sprite = leftSprite;
        StartCoroutine(GrowIcon(protagonist.balaoFala,true, true));
        StartCoroutine(GrowIcon(protagonist.leftIcon, false, false));
        StartCoroutine(GrowIcon(protagonist.rightIcon, false, false));
        yield return StartCoroutine(GrowIcon(protagonist.centerIconFront, false, false));
        yield return StartCoroutine(WaitSpacePress());
    }

    void EndEvent() {
        StartCoroutine(HideProtagonistIcons());
        StartCoroutine(HideCharacterIcons());
        StartCoroutine(CanInteractDelay());
    }

    IEnumerator CanInteractDelay(){
        yield return new WaitForSeconds(0.2f);
        gameManager.canMove = true;
        canInteract = true;
        yield return null;
    }

    public enum Interactables {
        Machine, 
        Kid, 
        Fisher, 
        Seller, 
        Whatever,
    }

    public enum InteractablesAnim {
        Idle, 
        Jump, 
        JumpRotate,
        JumpLoop,
        iddle_mulher,
        iddle_homem,
        iddle_homem2,
        iddle_guy,
        iddle_fisher,
        iddle_blonde
    }

    public enum InteractableState {
        Request, 
        Trade, 
        Finished,
    }
}