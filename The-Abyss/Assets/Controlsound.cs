using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlsound : MonoBehaviour
{

    public static Controlsound Instance;

    private AudioSource audioSource;
  
  private void awake(){
    if (Instance == null){
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }else{
        Destroy(gameObject);
    }

    audioSource = GetComponent<AudioSource>();
  }

public void EjecutarSonido (AudioClip sonido)
{
    audioSource.PlayOneShot(sonido);
}

}
