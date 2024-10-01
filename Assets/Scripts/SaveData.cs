using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData 
{
    public float levelOneTime;
    public float levelTwoTime;
    public float levelThreeTime;

    public SaveData(float t1, float t2, float t3)
    {
        levelOneTime = t1;
        levelTwoTime = t2;
        levelThreeTime = t3;
    }
}
