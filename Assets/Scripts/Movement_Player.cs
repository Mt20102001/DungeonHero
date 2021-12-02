using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Player : MonoBehaviour
{
    #region Public Values
    public Animator animator;
    public Rigidbody2D rb;
    public float moveSpeed;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;
    public bool CanMove;

    #endregion

    #region Private Values
    private Vector2 movement;
    private bool ledgeDetected;
    private bool isDashing;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDast = -100f;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isDashing = false;
        CanMove = true;
    }

    // Update is called once per frame
    private void Update()
    {
        // Input to movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && CanMove)
        {
            TurnPlayer();
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }

        //Input to Dash
        CheckDash();
        if (Input.GetKeyDown(KeyCode.Space) && !this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            AttemptToDash();
        }
    }


    private void FixedUpdate()
    {
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && CanMove)
        {
            // Movement
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        // Direction
        #region Direction
        if (movement.x != 0 || movement.y != 0)
        {
            // Last movement X
            if (movement.x > 0)
            {
                animator.SetFloat("XPlayer", 1f);
            }
            else if (movement.x < 0)
            {
                animator.SetFloat("XPlayer", -1f);
            }
            else
            {
                animator.SetFloat("XPlayer", 0f);
            }

            // Last movement Y
            if (movement.y > 0)
            {
                animator.SetFloat("YPlayer", 1f);
            }
            else if (movement.y < 0)
            {
                animator.SetFloat("YPlayer", -1f);
            }
            else
            {
                animator.SetFloat("YPlayer", 0f);
            }
        }
        #endregion

    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDast = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = rb.transform.localScale.x;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                CanMove = false;
                rb.velocity = movement * dashSpeed;
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(rb.transform.localScale.sqrMagnitude - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = rb.transform.localScale.x;
                }
            }
            if (dashTimeLeft <= 0)
            {
                rb.velocity = Vector2.zero;
                isDashing = false;
                CanMove = true;
            }
        }
    }

    private void TurnPlayer()
    {
        if (animator.GetFloat("XPlayer") < 0)
        {
            rb.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (animator.GetFloat("XPlayer") > 0)
        {
            rb.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
