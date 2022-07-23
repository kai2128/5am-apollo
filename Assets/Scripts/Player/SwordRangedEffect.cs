using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using Player;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

public class SwordRangedEffect : MonoBehaviour
{
    private Sword _sword;
    public Transform startPos;
    public LayerMask groundLayer;
    private float _maxTravelDistance;
    private float _travelSpeed;
    private Vector2 _direction;
    private float _distanceTravelled;
    private Vector3 _oldPos;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        _sword = PlayerManager.Instance.playerSword;
        _maxTravelDistance = _sword.maximumTravelDistance;
        _travelSpeed = _sword.rangedSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _distanceTravelled += Vector3.Distance(transform.position, _oldPos);
        _oldPos = transform.position;
        if (_distanceTravelled > _maxTravelDistance || ReachedWall())
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.GetFacingDirection() * _travelSpeed;
    }

    bool ReachedWall()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + transform.GetFacingDirection() * .1f, new Vector2(1f,.6f),0, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + transform.GetFacingDirection() * .33f, new Vector3(.1f,.6f,0));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            _sword.OnHitEnemy(col);
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        transform.position = startPos.position;
        transform.localScale = PlayerManager.Instance.transform.localScale; 
        _direction = PlayerManager.Instance.transform.GetFacingDirection();
        _distanceTravelled = 0;
        _oldPos = transform.position;
    }

}
