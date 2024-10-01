using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public float levelOneTime { get; private set; }
    public float levelTwoTime { get; private set; }
    public float levelThreeTime { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadData()
    {
        SaveData data = SystemSave.loadPlayer();
        if(data != null)
        {
            levelOneTime = data.levelOneTime;
            levelTwoTime = data.levelTwoTime;
            levelThreeTime = data.levelThreeTime;

            levelOneTime = levelOneTime == 0 ? Mathf.Infinity : levelOneTime;
            levelTwoTime = levelTwoTime == 0 ? Mathf.Infinity : levelTwoTime;
            levelThreeTime = levelThreeTime == 0 ? Mathf.Infinity : levelThreeTime;
        }
        else
        {
            levelOneTime = Mathf.Infinity;
            levelTwoTime = Mathf.Infinity;
            levelThreeTime = Mathf.Infinity;
        }
    }

    private void OnApplicationQuit()
    {
        SystemSave.SavePLayer(levelOneTime, levelTwoTime, levelThreeTime);
    }

    public void SetTime(int index, float val)
    {
        switch(index)
        {
            case 1:
                if(val < levelOneTime && val != 0)
                    levelOneTime = val;
                break;
            case 2:
                if (val < levelTwoTime && val != 0)
                    levelTwoTime = val;
                break;
            case 3:
                if (val < levelThreeTime && val != 0)
                    levelThreeTime = val;
                break;
        }
    }
}
