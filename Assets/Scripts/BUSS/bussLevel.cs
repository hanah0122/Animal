using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bussLevel
{
    public bussLevel()
    {

    }
    public string txtLevel(int level)
    {
        level += 1;
        return "Level: " + level.ToString();
    }
}
