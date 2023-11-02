using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReScaleSript : MonoBehaviour
{
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
        sp = transform.GetComponent<SpriteRenderer>();
        color = sp.color;
    }
    private Transform target;
    SpriteRenderer sp;
    // float tor = 0.18F;
    //float tor2 = 0.2F;
    //float dist = 0;
    public float tor = 0.18F;
    public float tor2 = 0.2F;
    public float dist = 0;
    public float sup = 0.10F;
    Color color;
    void Update()
    {
        dist = Vector3.Distance(target.position, transform.position);
        transform.localScale = new Vector3(tor * dist, tor * dist, 0);
        //color.a = (1-1/(dist*tor2));
        color.a = Mathf.Lerp(0, sup, 1 - 1 / (dist * tor2));
        sp.color = color;
    }
}
