using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hacker : MonoBehaviour
{
    [SerializeField] private float DisableLighstReChargeTime;
    [SerializeField] private float DisableCommsReChargeTime;
    [SerializeField] private float DisableCCTVReChargeTime;
    [SerializeField] private float DisableHacksreChargeTime;
    [SerializeField] private Button DisableLightsButton;
    [SerializeField] private Button DisableLaserButton;
    [SerializeField] private Button DisableCommsButton;
    [SerializeField] private Button DisableCCTVButton;
    [SerializeField] private GameObject blockPanel;
    private bool hasDisabledLasers = false;

    [SerializeField] private GameObject CCTVFootage;
    [SerializeField] private GameObject noSignalImage;
    [SerializeField] private GameObject numberPuzzle;
    [SerializeField] private Text numberPuzzleText;
    [SerializeField] private Text numberPuzzleTextTyped;
    [SerializeField] private Text timeLeftText;
    [SerializeField] private float maxRandomPeriod;
    [SerializeField] private float techGuyDisaperPeriod;
    [SerializeField] private float minRandomPeriod;
    [SerializeField] private float chanceToDiaspear;
    [SerializeField] private AudioSource staticSource;
    private string numberSequence = "";
    private string currentSequence;
    private float numberPuzzleTimer;
    [SerializeField] private float numberPuzzleTimeLimit = 10f;
    bool isSolving = false;
    bool isStaticCams = false;

    [SerializeField] private GameObject messageGO;
    [SerializeField] private GameObject messageGOText;

    public enum HackType
    {
        comms,
        lights,
        lasers,
        cctv
    }
    private HackType hackType;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(TechGuyRemove());
    }

    // Update is called once per frame
    void Update()
    {
        if(isSolving)
        {
            numberPuzzleTimer += Time.deltaTime;
            timeLeftText.text = "Time left - " + Mathf.RoundToInt(numberPuzzleTimeLimit - numberPuzzleTimer).ToString() + "s";
            if(numberPuzzleTimer >= numberPuzzleTimeLimit)
            {
                isSolving = false;
                numberSequence = "";
                currentSequence = "";
                numberPuzzleTimer = 0;

                SendMessageToHacker("Hack FAILED, hacking diabled for " + DisableHacksreChargeTime + " seconds");
                StartCoroutine(DisablEnableeHacks());
                Debug.Log("failure");
                numberPuzzle.SetActive(false);
            }
        }
    }

    public void PlayStatic()
    {
        if(isStaticCams && !staticSource.isPlaying)
            staticSource.Play();
    }

    public void StopStatic()
    {
        if (isStaticCams && staticSource.isPlaying)
            staticSource.Stop();
    }

    public void DisableLights()
    {
        GameManager.instance.DisableLights();

        DisableLightsButton.interactable = false;
        RobberUI.instance.SendMessageToPlayer("Lights disabled, coming back in " + GameManager.instance.ReturnLightsTime() + " seconds");
        StartCoroutine(DisablEnableeHacks());
    }

    public void DisableLasers()
    {
        DisableLaserButton.interactable = false;
        LaserNamager.instance.DisableLasers();

        RobberUI.instance.SendMessageToPlayer("Lasers disabled, coming back in " + GameManager.instance.ReturnLaserTime() + " seconds");
        StartCoroutine(DisablEnableeHacks());
    }

    public void DisableComms()
    {
        GameManager.instance.DisableComms();

        DisableCommsButton.interactable = false;
        RobberUI.instance.SendMessageToPlayer("Comms disabled, coming back in " + GameManager.instance.ReturnComsTime() + " seconds");
        StartCoroutine(DisablEnableeHacks());
    }

    public void DisableCCTV()
    {
        GameManager.instance.DisableCCTV();

        DisableCCTVButton.interactable = false;
        RobberUI.instance.SendMessageToPlayer("CCTVs disabled, coming back in " + GameManager.instance.ReturnCCTVTime() + " seconds");
        StartCoroutine(DisablEnableeHacks());
    }

    public void DisableCCTVs()
    {
        Debug.Log("disabled");
        CCTVFootage.SetActive(false);
        noSignalImage.SetActive(true);

        isStaticCams = true;
    }

    public void EnablCCTVs()
    {
        CCTVFootage.SetActive(true);
        noSignalImage.SetActive(false);

        staticSource.Stop();
        isStaticCams = false;
    }

    public void Addnumber(int number)
    {
        if (!isSolving)
            return;

        currentSequence += number;
        numberPuzzleTextTyped.text = numberPuzzleTextTyped.text + " *";
        if (currentSequence == numberSequence)
        {
            switch (hackType)
            {
                case HackType.comms:
                    DisableComms();
                    break;
                case HackType.lights:
                    DisableLights();
                    break;
                case HackType.lasers:
                    DisableLasers();
                    break;
                case HackType.cctv:
                    DisableCCTV();
                    break;
            }
            SendMessageToHacker("Hack Successful, hacking diabled for " + DisableHacksreChargeTime + " seconds");
            numberPuzzle.SetActive(false);
            isSolving = false;
        }
        else if((currentSequence.Length + 1) >= (numberSequence.Length + 1))
        {
            isSolving = false;
            numberSequence = "";
            currentSequence = "";
            numberPuzzleTimer = 0;

            numberPuzzle.SetActive(false);
            SendMessageToHacker("Hack FAILED, hacking diabled for " + DisableHacksreChargeTime + " seconds");
            StartCoroutine(DisablEnableeHacks());
        }
    }

    public void TryHack(int index)
    {
       switch(index)
        {
            case 0:
                hackType = HackType.comms;
                break;
            case 1:
                hackType = HackType.lights;
                break;
            case 2:
                hackType = HackType.lasers;
                break;
            case 3:
                hackType = HackType.cctv;
                break;
        }

        GenerateSequence();
    }

    private IEnumerator DisablEnableeHacks()
    {
        DisableLaserButton.interactable = false;
        DisableLightsButton.interactable = false;
        DisableCommsButton.interactable = false;
        DisableCCTVButton.interactable = false;

        yield return new WaitForSeconds(DisableHacksreChargeTime);

        DisableLaserButton.interactable = true;
        DisableLightsButton.interactable = true;
        DisableCommsButton.interactable = true;
        DisableCCTVButton.interactable = true;
    }

    private void GenerateSequence()
    {
        numberPuzzle.SetActive(true);
        numberSequence = "";

        for (int i = 0;i<6;i++)
        {
            int number = Random.Range(0, 9);
            numberSequence += number;
        }
        numberPuzzleText.text = numberSequence;
        isSolving = true;
        numberPuzzleTimer = 0;
        numberPuzzleTextTyped.text = "";

        currentSequence = "";
    }

    IEnumerator TechGuyRemove()
    {
        yield return new WaitForSeconds(Random.Range(minRandomPeriod, maxRandomPeriod) * 60);//convert to minutes
        blockPanel.SetActive(false);

        int chance = Random.Range(1, 100);
        if(chance >= chanceToDiaspear)
        {
            blockPanel.SetActive(true);
            yield return new WaitForSeconds(techGuyDisaperPeriod);
        }

        Debug.Log("called");
        StartCoroutine(TechGuyRemove());
    }

    private void SendMessageToHacker(string msg)
    {
        messageGO.SetActive(true);
        messageGOText.GetComponent<Text>().text = msg;

        StartCoroutine(DisableMessage());
    }

    IEnumerator DisableMessage()
    {
        yield return new WaitForSeconds(3f);

        messageGO.SetActive(false);
    }
}
