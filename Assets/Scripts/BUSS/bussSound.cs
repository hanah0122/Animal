using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bussSound : MonoBehaviour
{
    public AudioSource sourceBG;
    void Start()
    {
        this.sourceBG.Play();
    }
    public void UnPause()
    {
        sourceBG.UnPause();
    }
    public void Pause()
    {
        sourceBG.Pause();
    }
}
