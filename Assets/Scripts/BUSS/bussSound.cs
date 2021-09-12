using UnityEngine;

public class bussSound : MonoBehaviour
{
    public AudioSource Menu;
    public AudioSource Button;
    public AudioSource GameWin;
    public AudioSource GameLose;
    public AudioSource MoveTrue;
    public AudioSource MoveFlase;
    public AudioSource Begin;
    // public AudioSource ReLoad;
    public AudioSource Idear;
    public AudioSource Admob;
    void Start()
    {
        this.Menu.volume = 0.25f;
        this.Menu.Play();
    }
    public void SoundAdmob(bool play = true)
    {
        if (play)
            this.Admob.Play();
        else
            this.Admob.Stop();
    }
    public void SoundMenu(bool puase = true)
    {
        if (puase)
        {
            this.Menu.Pause();
        }
        else
        {
            this.Menu.UnPause();
        }
    }

    public void SoungBegin(bool play = true)
    {
        if (play)
            this.Begin.Play();
        else
            this.Begin.Stop();
    }

    // public void SoundReload(bool play = true)
    // {
    //     if (play)
    //         this.ReLoad.Play();
    //     else
    //         this.ReLoad.Stop();
    // }

    public void SoundIdea(bool play = true)
    {
        if (play)
            this.Idear.Play();
        else
            this.Idear.Stop();
    }

    public void SoundButton(bool play = true)
    {
        if (play)
            this.Button.Play();
        else
            this.Button.Stop();
    }

    public void SoundGameWin(bool play = true)
    {
        if (play)
            this.GameWin.Play();
        else
            this.GameWin.Stop();
    }

    public void SoundGameLose(bool play = true)
    {
        if (play)
            this.GameLose.Play();
        else
            this.GameLose.Stop();
    }

    public void SoundMoveTrue(bool play = true)
    {
        if (play)
            this.MoveTrue.Play();
        else
            this.MoveTrue.Stop();
    }

    public void SoundMoveFlase(bool play = true)
    {
        if (play)
            this.MoveFlase.Play();
        else
            this.MoveFlase.Stop();
    }

}
