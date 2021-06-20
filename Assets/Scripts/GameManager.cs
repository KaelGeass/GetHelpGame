using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isFireOn = false;
    public bool isqueiroVisivel = false;
    public bool canJump = false;
    public bool canMove = true;
    [SerializeField]
    public Level currentLevel = Level.Menu;
    [SerializeField]
    public Level lastLevel = Level.Menu;
    [SerializeField]
    public List<Item> inventoryItems = new List<Item>();
    public List<Item> tradedItems = new List<Item>();
    public Item corda;
    public Item balde;
    public Item doce;
    public Item isqueiro;
    public Item moedas;
    public Item uvas;
    public SoundEffects soundEffects;
    public AudioSource audioPlayer;
    public AudioSource musicPlayer;
    public Checkpoint lastCheckpoint = new Checkpoint(Vector2.zero, false);
    public List<GrapeNumber> grapesGottem = new List<GrapeNumber>();


    public delegate void OnVariableChangeDelegate();
    public event OnVariableChangeDelegate OnVariableChange;

    public delegate void OnIsqueiroTakenDelegate();
    public event OnIsqueiroTakenDelegate OnIsqueiroTaken;

    public static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) {
                GameObject gameObject = new GameObject("GameManager");
                _instance = gameObject.AddComponent<GameManager>();
                gameObject.tag = "GameManager";
            }
            return _instance;
        }
    }
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        soundEffects = gameObject.AddComponent<SoundEffects>();
        audioPlayer = gameObject.AddComponent<AudioSource>();
        musicPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.loop = true;

        corda = new Item("corda", Resources.Load<Sprite>("Itens/Corda"));
        balde = new Item("balde", Resources.Load<Sprite>("Itens/Balde"));
        doce = new Item("doce", Resources.Load<Sprite>("Itens/Doce"));
        isqueiro = new Item("isqueiro", Resources.Load<Sprite>("Itens/Isqueiro"));
        moedas = new Item("moedas", Resources.Load<Sprite>("Itens/Moedas"));
        uvas = new Item("uvas", Resources.Load<Sprite>("Itens/Uvas"));

        if(soundEffects.animationCurve == null){
            soundEffects.animationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
            soundEffects.animationCurve.preWrapMode = WrapMode.Clamp;
            soundEffects.animationCurve.postWrapMode = WrapMode.Clamp;
        }
    }

    public void Reset(){
        isFireOn = false;
        isqueiroVisivel = false;
        canJump = false;
        canMove = true;
        currentLevel = Level.Menu;
        lastLevel = Level.Menu;
        inventoryItems.Clear();
        tradedItems.Clear();
        grapesGottem.Clear();
        lastCheckpoint = new Checkpoint(Vector2.zero, false);
    }

    public void TradeItem( Item toTake, Item toGive = null) {
        if (toGive != null){
            inventoryItems.Remove(toGive);
            tradedItems.Add(toGive);
        }
        inventoryItems.Add(toTake);
        if(toGive == isqueiro){
            isFireOn = true;
            OnIsqueiroTaken();
        }
        OnVariableChange();
    }

    public InteractableObject.InteractableState GetInteractableState(InteractableObject.Interactables interactable){
         switch(interactable) {
            case InteractableObject.Interactables.Machine:
                if (inventoryItems.Contains(moedas)) {
                    return InteractableObject.InteractableState.Trade;
                } else if (tradedItems.Contains(moedas)) {
                    return InteractableObject.InteractableState.Finished;
                } else {
                    return InteractableObject.InteractableState.Request;
                }
            case InteractableObject.Interactables.Kid:
                if (inventoryItems.Contains(doce)) {
                    return InteractableObject.InteractableState.Trade;
                } else if (tradedItems.Contains(doce)) {
                    return InteractableObject.InteractableState.Finished;
                } else {
                    return InteractableObject.InteractableState.Request;
                }
            case InteractableObject.Interactables.Fisher:
                if (inventoryItems.Contains(isqueiro)) {
                    return InteractableObject.InteractableState.Trade;
                } else if (tradedItems.Contains(isqueiro)) {
                    return InteractableObject.InteractableState.Finished;
                } else {
                    return InteractableObject.InteractableState.Request;
                }
            case InteractableObject.Interactables.Seller:
                if (inventoryItems.Contains(uvas)) {
                    return InteractableObject.InteractableState.Trade;
                } else if (tradedItems.Contains(uvas)) {
                    return InteractableObject.InteractableState.Finished;
                } else {
                    return InteractableObject.InteractableState.Request;
                }
            case InteractableObject.Interactables.Whatever:
            default:
            return InteractableObject.InteractableState.Request;
         }
    }
}