using UnityEngine;
using System.Collections;

public class ChangeTransparency : MonoBehaviour
{

    private float duration = 1.0f;
    Color textureColor;
    Material mat;

    void Start()
    {
        textureColor = GetComponent<Renderer>().material.color;
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        textureColor.a = Mathf.PingPong(Time.time, duration) / duration;
        mat.color = textureColor;

    }
}