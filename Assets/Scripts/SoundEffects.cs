using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip bark;
    public AudioClip coin;
    public AudioClip damage;
    public AudioClip fanfare;
    public AudioClip getItem;
    public AudioClip giveItem;
    public AudioClip jump;
    public AudioClip jumpAnim;
    public AudioClip right;
    public AudioClip transition;
    public AudioClip wrong;

    public AudioClip CemiterioTheme;
    public AudioClip MenuTheme;
    public AudioClip FlorestaTheme;
    public AudioClip AcampamentoTheme;
    public AudioClip CidadeTheme;

    public AnimationCurve animationCurve;


    public static float GetPitchVariation(float min, float max){
        return Random.Range(min, max);
    }

    void Start(){
        bark = Resources.Load<AudioClip>("Sounds/SFX/Bark");
        coin = Resources.Load<AudioClip>("Sounds/SFX/Coin");
        damage = Resources.Load<AudioClip>("Sounds/SFX/Damage");
        getItem = Resources.Load<AudioClip>("Sounds/SFX/GetItem");
        giveItem = Resources.Load<AudioClip>("Sounds/SFX/GiveItem");
        jump = Resources.Load<AudioClip>("Sounds/SFX/Jump");
        jumpAnim = Resources.Load<AudioClip>("Sounds/SFX/JumpAnim");
        right = Resources.Load<AudioClip>("Sounds/SFX/Right");
        transition = Resources.Load<AudioClip>("Sounds/SFX/Transition");
        wrong = Resources.Load<AudioClip>("Sounds/SFX/Wrong");
        fanfare = Resources.Load<AudioClip>("Sounds/SFX/Fanfare");

        CemiterioTheme = Resources.Load<AudioClip>("Sounds/Wind");
        MenuTheme = Resources.Load<AudioClip>("Sounds/MenuTheme");
        FlorestaTheme = Resources.Load<AudioClip>("Sounds/Forest");
        AcampamentoTheme = Resources.Load<AudioClip>("Sounds/Acampamento");
        CidadeTheme = Resources.Load<AudioClip>("Sounds/CityTheme");
    }

    public IEnumerator FadeOutStart(AudioSource audioSource, float fadeDuration = 3f)
    {
        if ( audioSource.isPlaying)
        {
            float timer = 0f;
            float startVolume = audioSource.volume;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, animationCurve.Evaluate(timer / fadeDuration));
                yield return null;
            }
        }
        yield break;
    }
}
