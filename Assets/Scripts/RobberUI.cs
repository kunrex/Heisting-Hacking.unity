using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobberUI : MonoBehaviour
{
    [SerializeField] private GameObject Tick;
    [SerializeField] private GameObject X;

    public static RobberUI instance;
    [SerializeField] private Text timeText;

    [SerializeField] private GameObject messageTextGO;
    [SerializeField] private Text messageText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int min = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
        int sec = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);
        timeText.GetComponent<Text>().text = min.ToString("00") + ":" + sec.ToString("00");
    }

    public void JewelStolen()
    {
        X.SetActive(false);
        Tick.SetActive(true);
    }

    public bool isSolvingPuzzle()
    {
        if (transform.GetChild(0).gameObject.activeSelf || transform.GetChild(1).gameObject.activeSelf)
            return true;
        else
            return false;
    }

    public void DisablePuzzleSolver()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).GetComponent<NumberPuzzle>().Disable();
            return;
        }

        if (transform.GetChild(1).gameObject.activeSelf)
        {
            transform.GetChild(1).GetChild(0).GetComponent<DragHere>().Disable();
            return;
        }
    }

    public void SendMessageToPlayer(string msg)
    {
        messageTextGO.SetActive(true);
        messageText.text = msg;

        StartCoroutine(DisableMessage());
    }

    IEnumerator DisableMessage()
    {
        yield return new WaitForSeconds(5f);

        messageTextGO.SetActive(false);
    }
}
