using System;
using Class;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Player
{
    public class PlayerFly : MonoBehaviour
    {
        [HideInInspector] public Rigidbody2D rb;
        [Header("Fly")]
        private GameObject wings;
        private GameObject player;
        public float stamina;
        public float maxStamina = 100;
        public bool startRecoverStamina;
        public float recoverTimer = 2f;
        private float timer = 0f;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.Find("Player");
            wings = player.transform.GetChild(4).gameObject;
            wings.SetActive(false);
            stamina = maxStamina;
        }
        void Update()
        {
            if(!PlayerManager.Instance.unlockedFly)
                return;
            
            timer += Time.deltaTime;
            if (stamina <= maxStamina && startRecoverStamina)
            {
                stamina += maxStamina * 0.15f * Time.deltaTime;
            }
            if (timer > recoverTimer)
            {
                startRecoverStamina = true;
            }
            
            if (stamina <= Mathf.Epsilon)
            {
                wings.SetActive(false);
                return;
            }
            
            if (Input.GetButton("Fly"))
            {
                wings.SetActive(true);
                rb.velocity = Vector2.up * 5f;
                stamina -= maxStamina * 0.3f * Time.deltaTime;
                timer = 0f;
                startRecoverStamina = false;
            }

            if (Input.GetButtonUp("Fly"))
            {
                wings.SetActive(false);
            }
        }

    }
}