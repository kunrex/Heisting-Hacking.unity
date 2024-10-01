using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
            return;

        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.GameEnded(3);
        }
        Destroy(gameObject);
    }
}
