using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    [SerializeField] private GameObject instructionsLeave;
    [SerializeField] private GameObject doubleCheck;
    bool playerNear = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerMovement movement = collision.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                playerNear = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerNear = false;
    }

    private void Update()
    {
        if (playerNear)
        {
            Debug.Log("near");
            instructionsLeave.SetActive(true);
        }
        else
            instructionsLeave.SetActive(false);

        if (playerNear && Input.GetKeyDown(KeyCode.Return))
        {
            if (GameManager.instance.jewelStolen)
            {
                GameManager.instance.GameEnded(1);
            }
            else
            {
                doubleCheck.SetActive(true);
            }
        }
    }

    public void Leave()
    {
        GameManager.instance.GameEnded(4);
        instructionsLeave.SetActive(false);
        doubleCheck.SetActive(false);
    }

    public void Resume()
    {
        doubleCheck.SetActive(false);
    }
}
