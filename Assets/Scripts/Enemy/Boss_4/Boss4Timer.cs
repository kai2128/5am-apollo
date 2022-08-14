using System.Collections;
using System.Collections.Generic;
using Class;
using Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Boss4Timer : MonoBehaviour
{
    public float timer;
    public float endTime;
    public TextMeshProUGUI canvasTimer;
    public bool isTimerStarted;
    void Start()
    {
        endTime = 60;
        timer = endTime;
    }

    void Update()
    {
        if (isTimerStarted)
        {
            timer -= Time.deltaTime;
            canvasTimer.SetText("Time left:"+timer.ToString("0.00"));
        }

        if (timer <0)
        {
            isTimerStarted = false;
            PlayerManager.Instance.GetComponent<PlayerOnHit>().GetHit(new AttackArguments(99999f, 0).UpdateTransform(transform));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(isTimerStarted) return;
        if(col.CompareTag("Player"))
        {
            PlayerManager.Instance.SetStatusMessage("Reach boss room before times out!");
            isTimerStarted = true;
            timer = endTime;
        }
    }
}
