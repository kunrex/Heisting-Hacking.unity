using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private float maxMoveDistance;

    private float X2;
    private float Y2;
    [SerializeField] private AudioSource walkingSource;

    private void Start()
    {
        rigidbody = rigidbody == null ? GetComponent<Rigidbody2D>() : rigidbody;
        spriteRenderer = spriteRenderer == null ? GetComponent<SpriteRenderer>() : spriteRenderer;
        animator = animator == null ? GetComponent<Animator>() : animator;

        animator.SetFloat("X", horizontalInput);
        animator.SetFloat("Y", verticalInput);

        X2 = 0;
        Y2 = 1;

        animator.SetFloat("X2", 0);
        animator.SetFloat("Y2", 1);
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.instance.PauseGame();
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + new Vector2(horizontalInput, verticalInput) * speed * Time.deltaTime);

        animator.SetFloat("X", horizontalInput);
        animator.SetFloat("Y", verticalInput);

        animator.SetFloat("X2", X2);
        animator.SetFloat("Y2", Y2);

        if (horizontalInput != 0 || verticalInput != 0)
        {
            X2 = horizontalInput;
            Y2 = verticalInput;

            if (!walkingSource.isPlaying)
                walkingSource.Play();

            if (RobberUI.instance.isSolvingPuzzle())
            {
                RobberUI.instance.DisablePuzzleSolver();
            }
        }
        else
            walkingSource.Stop();
    }

    public void SetSpeed(float val)
    {
        speed = val;
    }
}
