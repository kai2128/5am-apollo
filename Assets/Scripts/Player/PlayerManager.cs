using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager: MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    [Header("states")]
    public bool isAttacking = false;

    public bool canMove = true;
    public bool isDashing = false;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);

        Instance = this;
    }
}