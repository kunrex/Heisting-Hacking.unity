using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jewel : MonoBehaviour
{
    [SerializeField] private float stealTime;
    private float hasBeenStealingFor;
    [SerializeField] private Image fillImage;

    public bool near = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (near && Input.GetKey(KeyCode.E))
        {
            hasBeenStealingFor += Time.deltaTime;
            fillImage.fillAmount = hasBeenStealingFor / stealTime;
            if (hasBeenStealingFor >= stealTime)
            {
                GameManager.instance.StartCoroutine(GameManager.instance.JewelStolen());
                near = false;
                gameObject.SetActive(false);
                RobberUI.instance.SendMessageToPlayer("You have the Jewel and have a window of 5 seconds. get out of there!");
            }
        }
        else
        {
            hasBeenStealingFor = 0;
            fillImage.fillAmount = hasBeenStealingFor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            near = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hasBeenStealingFor = 0;
        fillImage.fillAmount = hasBeenStealingFor;
        near = false;
    }
}
