using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CCTV : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float view;
    [SerializeField] private Transform player;
    [SerializeField] private Transform maxDistance;

    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 to;
    [SerializeField] private float oscillationSpeed;

    [SerializeField] private float fireRate;
    private float timeToNextFire;
    [SerializeField] private float shootingStartUp = 1f;
    private float shootingTimeElapsed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletForce;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private LayerMask playerMask;

    private Vector3 direction;
    [SerializeField] private GameObject light;
    private bool isEnabled;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 diff = direction;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        maxDistance.SetParent(light.transform);
        maxDistance.localPosition = new Vector3(0, 10.5f, 0);
        isEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled || GameManager.instance.gameEnded)
            return;

        bool detected = Detect();
        if (detected && !GameManager.instance.playerFound)
        {
            GameManager.instance.AlertEnemies();
        }
        direction = maxDistance.position - light.transform.position;
        Debug.DrawRay(light.transform.position, direction);

        if (detected)
        {
            //look at player
            Vector3 diff = player.position - light.transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.GetChild(0).eulerAngles = new Vector3(0, 0, rot_z - 90);
        }
        else
        {
            float t = Mathf.PingPong(Time.time * oscillationSpeed * 2.0f, 1.0f);

            transform.GetChild(0).eulerAngles = Vector3.Lerp(from, to, t); 
        }
    }

    private bool Detect()
    {
        if (GameManager.instance.lightsOff)
            return false;

        float distance = Vector2.Distance(player.position, light.transform.position);
        if(distance < radius)
        {
            Vector3 dir = (player.position - light.transform.position);
            if (Vector2.Angle(direction, dir) < view / 2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(light.transform.position, dir, radius, playerMask);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void Disable()
    {
        StartCoroutine(DisableEffect());
    }

    public void Enable()
    {
        light.GetComponent<Light2D>().enabled = true;
        GetComponent<AudioSource>().Play();
        this.enabled = true;
        isEnabled = true;
    }

    IEnumerator DisableEffect()
    {
        isEnabled = false;
        GetComponent<AudioSource>().Stop();

        light.GetComponent<Light2D>().enabled = false;
        yield return new WaitForSeconds(.3f);
        light.GetComponent<Light2D>().enabled = true;
        yield return new WaitForSeconds(.5f);
        light.GetComponent<Light2D>().enabled = false;
        yield return new WaitForSeconds(.3f);
        light.GetComponent<Light2D>().enabled = true;
        yield return new WaitForSeconds(.5f);
        light.GetComponent<Light2D>().enabled = false;

        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
