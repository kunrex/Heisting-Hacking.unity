using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject[] Enemies;
    [SerializeField] private float stealWindow;
    [SerializeField] private float playerSpeedWIthJewel;

    [SerializeField] private PlayerMovement movement;
    [SerializeField] private float alertedEnemiesSpeed;
    [SerializeField] private GameObject gamePausedCanvas;
    [SerializeField] private GameObject gameEndedCanvas;
    [SerializeField] private int gamePausedTextIndex;
    [SerializeField] private string playerWonText;
    [SerializeField] private string playerLostText_caught;
    [SerializeField] private string playerLostText_died;
    [SerializeField] private string playerLostText_left;
    [SerializeField] private float lightsReChargeTime;
    [SerializeField] private float commsReChargeTime;
    [SerializeField] private float cctvReChargeTime;
    [SerializeField] private float disabledLightIntenity;
    public bool lightsOff;
    public bool commsOff;
    public bool CCTVOff;
    public bool playerFound;
    public bool jewelStolen = false;

    [SerializeField] private Hacker hacker;
    [SerializeField] private Camera[] CCTVs;
    [SerializeField] private CCTVDisabler[] CCTVDisablers;
    [SerializeField] private LaserDisabler[] laserDisablers;
    [SerializeField] private Jewel jewel;

    [SerializeField] private Light2D[] lights;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject instructionsToSteal;
    public bool gameEnded { get; private set; } = false;
    [SerializeField] private string AlarmSound;

    [SerializeField] private float reLightAlarmTimer;
    [SerializeField] private Color RED;
    [SerializeField] private Color NORMAL;
    private float timeToNextRed;
    bool isRed = false;
    [SerializeField] private int levelNumber;
    [SerializeField] private Animator transition;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        gameEnded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad >= 600)
            GameEnded(5);

        foreach(CCTVDisabler cctv in CCTVDisablers)
        {
            if (cctv.near)
            {
                instructions.SetActive(true);
                return;
            }
        }

        foreach (LaserDisabler laser in laserDisablers)
        {
            if (laser.near)
            {
                instructions.SetActive(true);
                return;
            }
        }

        if(jewel.near)
        {
            instructionsToSteal.SetActive(true);
            return;
        }

        instructionsToSteal.SetActive(false);
        instructions.SetActive(false);

        if(playerFound || jewelStolen)
        {
            if(Time.timeSinceLevelLoad >= timeToNextRed)
            {
                timeToNextRed = Time.timeSinceLevelLoad + 1 / reLightAlarmTimer;
                SetColor();
            }
        }
    }

    public float ReturnCCTVTime()
    {
        return cctvReChargeTime;
    }

    public float ReturnComsTime()
    {
        return commsReChargeTime;
    }

    public float ReturnLightsTime()
    {
        return lightsReChargeTime;
    }

    public float ReturnLaserTime()
    {
        return LaserNamager.instance.RetirnLaserTime();
    }

    private void SetColor()
    {
        if (isRed)
        {
            foreach (Light2D light in lights)
                light.color = RED;
        }
        else
        {
            foreach (Light2D light in lights)
                light.color = NORMAL;
        }

        isRed = !isRed;
    }

    public void AlertEnemies()
    {
        if (playerFound || jewelStolen)
            return;

        foreach (GameObject Enemy in Enemies)
        {
            Enemy.GetComponent<Enemy>().LocatedPlayer();
            Enemy.GetComponent<Enemy>().SetSpeed(alertedEnemiesSpeed);
        }

        playerFound = true;
        RobberUI.instance.SendMessageToPlayer("Your location has been revealed, The guards are coming for you");
        AudioManager.instance.PlaySound(AlarmSound);
        AudioManager.instance.StopSound("Music");
    }

    public void GameEnded(int result)
    {
        gameEndedCanvas.SetActive(true);

        switch(result)
        {
            case 1:
                gameEndedCanvas.transform.GetChild(gamePausedTextIndex + 1).GetComponent<Text>().text = playerWonText;
                TimeManager.instance.SetTime(levelNumber, Time.timeSinceLevelLoad);
                break;
            case 2:
                gameEndedCanvas.transform.GetChild(gamePausedTextIndex + 1).GetComponent<Text>().text = playerLostText_caught;
                break;
            case 3:
                gameEndedCanvas.transform.GetChild(gamePausedTextIndex + 1).GetComponent<Text>().text = playerLostText_died;
                break;
            case 4:
                gameEndedCanvas.transform.GetChild(gamePausedTextIndex + 1).GetComponent<Text>().text = playerLostText_left;
                break;
            case 5:
                gameEndedCanvas.transform.GetChild(gamePausedTextIndex + 1).GetComponent<Text>().text = "Time Limit Crossed";
                break;
        }

        gameEnded = true;
        AudioManager.instance.StopSoundAll();
        Time.timeScale = 0;
    }

    public IEnumerator JewelStolen()
    {
        movement.SetSpeed(playerSpeedWIthJewel);
        yield return new WaitForSeconds(stealWindow);
        Debug.Log("stolen");

        foreach (GameObject Enemy in Enemies)
        {
            Enemy.GetComponent<Enemy>().LocatedPlayer();
            Enemy.GetComponent<Enemy>().SetSpeed(alertedEnemiesSpeed);
        }
        //todo someffect

        RobberUI.instance.SendMessageToPlayer("Your location has been revealed and the guards are coming for you. Your speed has been decreased");
        jewelStolen = true;
        RobberUI.instance.JewelStolen();
        AudioManager.instance.PlaySound(AlarmSound);
        AudioManager.instance.StopSound("Music");
    }

    public void Retry()
    {
        Time.timeScale = 1;
        StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex));
    }

    public void Exit()
    {
        Time.timeScale = 1;
        StartCoroutine(Load(0));
    }

    IEnumerator Load(int index)
    {
        transition.SetTrigger("T");

        yield return new WaitForSeconds(.6f);
        SceneManager.LoadScene(index);
    }

    public void PauseGame()
    {
        gamePausedCanvas.SetActive(true);
        foreach(GameObject enemy in Enemies)
            enemy.GetComponent<AudioSource>().Stop();

        Time.timeScale = 0;
    }

    public void Resume()
    {
        gamePausedCanvas.SetActive(false);
        foreach (GameObject enemy in Enemies)
            enemy.GetComponent<AudioSource>().Stop();
        Time.timeScale = 1;
    }

    public IEnumerator EnableLights()
    {
        yield return new WaitForSeconds(lightsReChargeTime);

        foreach(Light2D light in lights)
        {
            light.intensity = 1f;
        }
        lightsOff = false;
    }

    public void DisableLights()
    {
        foreach (Light2D light in lights)
        {
            light.intensity = disabledLightIntenity;
        }

        lightsOff = true;
        StartCoroutine(EnableLights());
    }

    public IEnumerator EnableComms()
    {
        yield return new WaitForSeconds(commsReChargeTime);

        commsOff = false;
    }

    public void DisableComms()
    {
        commsOff = true;
        StartCoroutine(EnableComms());
    }

    public IEnumerator EnableCCTV()
    {
        yield return new WaitForSeconds(cctvReChargeTime);

        foreach (Camera cam in CCTVs)
        {
            cam.enabled = true;
            cam.GetComponent<CCTV>().Enable();
        }
        CCTVOff = false;
        hacker.EnablCCTVs();
    }

    public void DisableCCTV()
    {
        foreach (Camera cam in CCTVs)
        {
            cam.enabled = false;
            cam.GetComponent<CCTV>().Disable();
        }
        CCTVOff = true;
        hacker.DisableCCTVs();

        StartCoroutine(EnableCCTV());
    }

    public IEnumerator enableCCTV(CCTV tvToEnable, float time)
    {
        yield return new WaitForSeconds(time);

        tvToEnable.Enable();
    }

    public void StartSomeCoroutine(IEnumerator cor)
    {
        StartCoroutine(cor);
    }
}
