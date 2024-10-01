using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasers : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private AudioSource zapSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.enabled == false)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            zapSound.Play();
            GameManager.instance.GameEnded(3);
        }
    }
}
