using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    private PlayerAnimation anim;
    private PlayerCollision coll;

    public float speed = 5;
    [Range(1, 10)] public float jumpVelocity = 7f;
    public float dashSpeed = 15;
    private float dashTimer;
    public float totalDashTime;
    public int jumpCount = 2;
    public bool isDasing = false;
    private float dashDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<PlayerAnimation>();
        coll = GetComponent<PlayerCollision>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 destination = new Vector2(x, y);
        anim.FlipDirection(x);

        if (coll.onGround) jumpCount = 2;

        Walk(destination);
        anim.SetHorizontalMovement(x, y, rb.velocity.y);
        if (Input.GetButtonDown("Jump") && jumpCount > 1)
        {
            anim.SetTrigger("jump");
            rb.velocity = Vector2.up * jumpVelocity;
            jumpCount--;
        }

        if (Input.GetButtonDown("Dash") && !isDasing)
        {
            Dash((int)x);
        }
        CheckDash();
        
    }

    private void Walk(Vector2 des)
    {
        rb.velocity = new Vector2(des.x * speed, rb.velocity.y);
    }

    private void CheckDash()
    {
        if (!isDasing) return;
        rb.velocity = transform.right * (dashDirection * dashSpeed);
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0)
        {
            isDasing = false;
        }
    }

    private void Dash(int x)
    {
        anim.SetTrigger("dash");
        isDasing = true;
        dashTimer = totalDashTime;
        
        rb.velocity = Vector2.zero;
        dashDirection = x; 
    }

}