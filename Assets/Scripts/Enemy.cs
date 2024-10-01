using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AIDestinationSetter setter;
    [SerializeField] private AIPath path;
    [SerializeField] private Seeker seeker;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] wayPoints;
    private int currentIndex;
    private Transform target;
    [SerializeField] private bool isDetecting;
    [SerializeField] private float viewDistance;
    [SerializeField] private float view;
    [SerializeField] private Transform rayCastPos;
    [SerializeField] private LayerMask mask;

    [SerializeField] private GameObject alertObject;
    [SerializeField] private GameObject lockObject;
    private Animator animator;
    Vector2 prevPos;
    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        setter = setter == null ? GetComponent<AIDestinationSetter>() : setter;
        path = path == null ? GetComponent<AIPath>() : path;
        seeker = seeker == null ? GetComponent<Seeker>() : seeker;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        setter.target = wayPoints[currentIndex];
        animator.SetBool("Move", true);
    }

    IEnumerator ReStart()
    {
        //path
        yield return new WaitForSeconds(1f);

        path.SearchPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (path.reachedDestination)
        {
            if (setter.target != player)
            {
                currentIndex++;
                if (currentIndex == wayPoints.Length)
                    currentIndex = 0;

                setter.target = wayPoints[currentIndex];
                path.destination = setter.target.position;
                StartCoroutine(ReStart());
            }
        }

        if (!isDetecting)
        {
            if (GameManager.instance.lightsOff)
            {
                transform.GetChild(2).gameObject.SetActive(false);
                return;
            }

            transform.GetChild(2).gameObject.SetActive(true);

            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < viewDistance)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                if (Vector3.Angle(transform.up, dir) < view / 2f)
                {
                    RaycastHit2D hit = Physics2D.Raycast(rayCastPos.position, dir, viewDistance, mask);
                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            Debug.Log(hit.collider.name);
                            isDetecting = true;
                            if (!GameManager.instance.commsOff)
                            {
                                GameManager.instance.AlertEnemies();
                                GetComponent<AudioSource>().Play();//play alert sound
                            }
                            else
                                AudioManager.instance.PlaySound("Alert");

                            setter.target = player;
                            transform.GetChild(2).gameObject.SetActive(false);
                            StartCoroutine(ALERT());
                        }
                    }
                }
            }
        }

        prevPos = (Vector2)transform.position;
    }

    public void LocatedPlayer()
    {
        isDetecting = true;

        setter.target = player;
        transform.GetChild(2).gameObject.SetActive(false);
        StartCoroutine(ALERT());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 diff = collision.transform.position - player.position;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.GetChild(0).eulerAngles = new Vector3(0, 0, rot_z - 180);

            GameManager.instance.GameEnded(2);
        }
    }

    public void SetSpeed(float val)
    {
        path.maxSpeed = val;
    }

    IEnumerator ALERT()
    {
        alertObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        alertObject.SetActive(false);
        lockObject.SetActive(true);
    }

}
