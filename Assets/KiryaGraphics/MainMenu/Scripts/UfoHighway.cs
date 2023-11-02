using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoHighway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float speedx = 0;
    public float speedy = 00;
    public float speedz = 0;
    public float x;
    public float y;
    public float z;
    public float xf = 1000;
    public float yf = 1000;
    public float zf=1000;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(speedx, speedy, speedz);
        if (transform.position.x > xf || transform.position.y>yf || transform.position.z>zf) transform.SetPositionAndRotation(new Vector3(x, y, z), transform.rotation);
        
    }
}
