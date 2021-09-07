using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bussTimer
{
    

    public void countDown(infoTime _infoTime,float timer)
    {
        _infoTime._txtTime.text = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(_infoTime._gameTime - _infoTime._timer - Mathf.FloorToInt(timer / 60) * 60));
        _infoTime._slider.value = _infoTime._gameTime - _infoTime._timer;

    }
    
}


//https://www.youtube.com/watch?v=Qxwqd2kMHbI