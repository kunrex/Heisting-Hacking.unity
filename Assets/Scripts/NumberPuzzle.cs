using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberPuzzle : MonoBehaviour
{
    private string numberSequence = "";
    private string currentSequence;
    private float numberPuzzleTimer;
    [SerializeField] private float numberPuzzleTimeLimit = 10f;
    [SerializeField] private Text numberPuzzleText;
    [SerializeField] private Text numberPuzzleTextTyped;
    [SerializeField] private Text timeLeftText;
    [SerializeField] private float hideNumbersTime;
    [SerializeField] private float reEnableLaserTime;
    [SerializeField] private float reChargeTime;
    private bool isSolving = true;
    private Lasers laser;
    public bool reCharging = false;

    public void Addnumber(int number)
    {
        if (!isSolving)
            return;

        currentSequence += number;
        numberPuzzleTextTyped.text = numberPuzzleTextTyped.text + " *";
        if (currentSequence == numberSequence)
        {
            isSolving = false;
            numberSequence = "";
            currentSequence = "";
            numberPuzzleTimer = 0;

            reCharging = true;
            laser.gameObject.SetActive(false);
            LaserNamager.instance.StartCoroutine(LaserNamager.instance.EnableLaser(laser, reEnableLaserTime));
            GameManager.instance.StartCoroutine(ReCharge());
            AudioManager.instance.PlaySound("LaserD");
            RobberUI.instance.SendMessageToPlayer("1 laser disabled");
            gameObject.SetActive(false);
        }
        else if ((currentSequence.Length + 1) >= (numberSequence.Length + 1))
        {
            isSolving = false;
            numberSequence = "";
            currentSequence = "";
            numberPuzzleTimer = 0;

            reCharging = true;

            gameObject.SetActive(false);
            GameManager.instance.StartCoroutine(ReCharge());
        }
    }

    public void Disable()
    {
        isSolving = false;
        numberSequence = "";
        currentSequence = "";
        numberPuzzleTimer = 0;

        reCharging = true;

        gameObject.SetActive(false);
        GameManager.instance.StartCoroutine(ReCharge());
    }

    public void TryHack(Lasers _laser)
    {
        gameObject.SetActive(true);
        laser = _laser;
        GenerateSequence();
    }

    private void GenerateSequence()
    {
        gameObject.SetActive(true);
        numberSequence = "";

        for (int i = 0; i < 6; i++)
        {
            int number = Random.Range(0, 9);
            numberSequence += number;
        }
        numberPuzzleText.text = numberSequence;
        isSolving = true;
        numberPuzzleTimer = 0;
        numberPuzzleTextTyped.text = "";
        StartCoroutine(HideNumbers());

        currentSequence = "";
    }

    IEnumerator HideNumbers()
    {
        yield return new WaitForSeconds(hideNumbersTime);

        numberPuzzleText.text = "* * * * * *";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator ReCharge()
    {
        yield return new WaitForSeconds(reChargeTime);
        reCharging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSolving)
        {
            numberPuzzleTimer += Time.deltaTime;
            timeLeftText.text = "Time left - " + Mathf.RoundToInt(numberPuzzleTimeLimit - numberPuzzleTimer).ToString() + "s";
            if (numberPuzzleTimer >= numberPuzzleTimeLimit)
            {
                isSolving = false;
                numberSequence = "";

                currentSequence = "";
                numberPuzzleTimer = 0;
                gameObject.SetActive(false);
                reCharging = true;

                GameManager.instance.StartCoroutine(ReCharge());
            }
        }
    }
}
