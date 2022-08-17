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
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.Find("Player");
            wings = player.transform.GetChild(4).gameObject;
            wings.SetActive(false);
        }
        void Update()
        {

            if (Input.GetButton("Fly"))
            {
                wings.SetActive(true);
                rb.velocity = Vector2.up * 10f;
            }
            if (Input.GetButtonUp("Fly"))
            {
                wings.SetActive(false);
            }

        }

    }
}