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
    public int jumpCount = 2;

    [Header("Dash")] 
    public bool canDash = true;
    public bool isDasing = false;
    public float dashSpeed = 10;
    public float dashCooldown = 0.2f;
    public float dashTime = 0.4f;

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
        if(isDasing)
            return;
        
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

        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void Walk(Vector2 des)
    {
        rb.velocity = new Vector2(des.x * speed, rb.velocity.y);
    }

    private IEnumerator Dash()
    {
        anim.SetTrigger("dash");
        canDash = false;
        isDasing = true;
        yield return new WaitForSeconds(0.2f);

        rb.velocity = Vector2.zero;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0.02f;
        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDasing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}