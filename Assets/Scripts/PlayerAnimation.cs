using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    private Player player;
    private PlayerCollision col;
    [HideInInspector]
    public SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponentInParent<PlayerCollision>();
        player = GetComponentInParent<Player>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("onGround", col.onGround);
    }

    public void SetHorizontalMovement(float x, float y, float yVel)
    {
        anim.SetFloat("horizontalAxis", Mathf.Abs(x));
        anim.SetFloat("verticalAxis", Mathf.Abs(y));
        anim.SetFloat("verticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void FlipDirection(float side)
    {
        var facing = side switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => transform.localScale.x 
        };
        transform.localScale = new Vector3(facing, 1, 1) ;
    }
    
}
