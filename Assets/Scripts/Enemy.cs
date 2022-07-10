using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit(Vector2 dir)
    {
        Debug.Log(2);
        var oriScale = transform.localScale;
        oriScale.x = dir.x;
        rb.velocity = dir * 0.4f;
        StartCoroutine(Blink());
    }
    private IEnumerator Blink() {
        Color defaultColor = sr.color;
 
        sr.color = new Color(1, 1, 1,1);
 
        yield return new WaitForSeconds(0.05f);
        sr.color = defaultColor ;
    }
}
