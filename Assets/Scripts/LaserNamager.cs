using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserNamager : MonoBehaviour
{
    public static LaserNamager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    [SerializeField] private Lasers[] lasers;
    [SerializeField] private float laserRestartTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableLasers()
    {
        foreach (Lasers laser in lasers)
        {
            laser.enabled = false;
            laser.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            laser.transform.GetChild(0).gameObject.SetActive(false);
            laser.transform.GetChild(2).gameObject.SetActive(false);
        }

        AudioManager.instance.PlaySound("LaserD");

        StartCoroutine(RestartLasers());
    }

    IEnumerator RestartLasers()
    {
        yield return new WaitForSeconds(laserRestartTime);

        foreach (Lasers laser in lasers)
        {
            laser.enabled = true;
            laser.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            laser.transform.GetChild(0).gameObject.SetActive(true);
            laser.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public IEnumerator EnableLaser(Lasers laser, float time)
    {
        yield return new WaitForSeconds(time);

        laser.gameObject.SetActive(true);
    }

    public float RetirnLaserTime()
    {
        return laserRestartTime;
    }
}
