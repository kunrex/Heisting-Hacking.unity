using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelChooserPanel;
    [SerializeField] private GameObject SettingsPanel;

    [SerializeField] private Text levelOneTime;
    [SerializeField] private Text levelTwoTime;
    [SerializeField] private Text levelThreeTime;

    // Start is called before the first frame update
    void Start()
    {
        levelOneTime.text = TimeManager.instance.levelOneTime == Mathf.Infinity ? "Nan" : FormatToTime(TimeManager.instance.levelOneTime);
        levelTwoTime.text = TimeManager.instance.levelTwoTime == Mathf.Infinity ? "Nan" : FormatToTime(TimeManager.instance.levelTwoTime);
        levelThreeTime.text = TimeManager.instance.levelThreeTime == Mathf.Infinity ? "Nan" : FormatToTime(TimeManager.instance.levelThreeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string FormatToTime(float val)
    {
        int min = Mathf.FloorToInt(val / 60);
        int sec = Mathf.FloorToInt(val % 60);
        return min.ToString("00") + ":" + sec.ToString("00");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleSettingsTab()
    {
        LevelChooserPanel.SetActive(false);
        SettingsPanel.SetActive(false);

        SettingsPanel.SetActive(!SettingsPanel.activeSelf);
    }

    public void ToggleLevelChoosePanel()
    {
        LevelChooserPanel.SetActive(false);
        SettingsPanel.SetActive(false);

        LevelChooserPanel.SetActive(!LevelChooserPanel.activeSelf);
    }

    public void BackToHome()
    {
        LevelChooserPanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }
}
