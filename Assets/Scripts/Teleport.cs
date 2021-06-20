using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public Level nextLevel;
    public Level currentLevel;
    public Animator transition;
    public AudioSource audioSource;
    GameManager gameManager;
    bool lockEvent = false;

    void Start(){
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Protagonist" && lockEvent == false) {
            StartCoroutine(gameManager.soundEffects.FadeOutStart(gameManager.musicPlayer, 2f));
            StartCoroutine(LoadNextLevel(nextLevel));
        }
    }

    IEnumerator LoadNextLevel(Level level){
        lockEvent = true;
        transition.SetTrigger("Start");
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        gameManager.audioPlayer.pitch = 1;
        gameManager.audioPlayer.PlayOneShot(gameManager.soundEffects.transition, 0.4f);
        yield return new WaitForSeconds(1f);
        gameManager.currentLevel = nextLevel;
        gameManager.lastLevel = currentLevel;
        switch(level){
            case Level.Cemiterio:
            SceneManager.LoadScene("Cemiterio");
            gameManager.musicPlayer.clip = gameManager.soundEffects.CemiterioTheme;
            break;
            case Level.Cidade:
            gameManager.musicPlayer.clip = gameManager.soundEffects.CidadeTheme;
            SceneManager.LoadScene("Cidade");
            break;
            case Level.Floresta:
            gameManager.musicPlayer.clip = gameManager.soundEffects.FlorestaTheme;
            SceneManager.LoadScene("Floresta");
            break;
            case Level.Acampamento:
            gameManager.musicPlayer.clip = gameManager.soundEffects.AcampamentoTheme;
            SceneManager.LoadScene("Acampamento");
            break;
            default:
            break;
        }
        gameManager.musicPlayer.volume = 0.8f;
        gameManager.musicPlayer.loop = true;
        gameManager.musicPlayer.Play();
        lockEvent = false;
    }
}

public enum Level {
    Cemiterio = 0,
    Cidade = 1,
    Floresta = 2, 
    Acampamento = 3,
    Menu = 4
}