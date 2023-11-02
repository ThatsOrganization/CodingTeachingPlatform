using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private Transform target;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
