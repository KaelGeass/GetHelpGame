using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void CloseGame(){
        Debug.Log("Quit Game called");
        Application.Quit();
    }

    public void PlayClip(AudioSource audioSource){
        
    }
}
