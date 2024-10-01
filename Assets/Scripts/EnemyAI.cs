using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Transform player;
    [SerializeField] private float speed;
    [SerializeField] private float nextWayPointDistance;
    [SerializeField] private bool isDetecting;
    [SerializeField] private LayerMask mask;

    private Path path;
    private int currentWayPoint;
    private bool hasReachedEnd;
    private Seeker seeker;
    private Rigidbody2D rigidbody;
    [SerializeField] private Transform rayCastPos;
    [SerializeField] private int view;
    [SerializeField] private int viewDistance;
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private float detectionRange;
    [SerializeField] private SpriteRenderer renderer;
    private bool unCheckPlayerDetection;
    private float timeFromLastStop = 0;
    [SerializeField] private float timeBeforeMovements;
    [SerializeField] private GameObject alertObject;
    [SerializeField] private GameObject lockObject;

    [SerializeField] private Transform[] wayPoints;
    int currentIndex;
    [SerializeField] private float minRotation;
    private Animator animator;
    private Vector2 prevPos;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        target = wayPoints[currentIndex];
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void UpdatePath()
    {
        hasReachedEnd = false;
        if(seeker.IsDone())
            seeker.StartPath(rigidbody.position, target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null || (GameManager.instance.lightsOff && isDetecting))
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            hasReachedEnd = true;
            if (isDetecting)
                target = player;
            else
            {
                currentIndex++;
                if (currentIndex == wayPoints.Length)
                    currentIndex= 0;

                target = wayPoints[currentIndex];
            }

            return;
        }
        else
            hasReachedEnd = false;

        Vector2 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;

        Vector2 force = dir * speed * Time.deltaTime;

        rigidbody.AddForce(force);

        float distance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWayPointDistance)
            currentWayPoint++;

        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        rigidbody.MoveRotation((rot_z - 90));

        if (prevPos != rigidbody.position)
            animator.SetBool("Move", true);
        else
            animator.SetBool("Move", false);

        prevPos = rigidbody.position;
    }

    private void Update()
    {
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
                                StartCoroutine(ALERTPLAYER());
                            }
                            else
                            {
                                RobberUI.instance.SendMessageToPlayer("A guard has spooted you, but comms were off so your position is only known to him");
                                GetComponent<AudioSource>().Play();//play alert sound
                            }

                            target = player;
                            transform.GetChild(2).gameObject.SetActive(false);
                            StartCoroutine(ALERT());
                        }
                    }
                }
            }
        }
    }

    IEnumerator ALERTPLAYER()
    {
        GetComponent<AudioSource>().Play();//play alert sound
        yield return new WaitForSeconds(1f);

        GameManager.instance.AlertEnemies();
    }

    public void LocatedPlayer()
    {
        isDetecting = true;
        unCheckPlayerDetection = true;

        target = player;
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
        speed = val;
    }

    IEnumerator ALERT()
    {
        alertObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        alertObject.SetActive(false);
        lockObject.SetActive(true);
    }
}
