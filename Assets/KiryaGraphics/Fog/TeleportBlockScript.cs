using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBlockScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        part1 = transform.Find("Particles 1").GetComponent<ParticleSystem>();
        part2 = transform.Find("Particles 2").GetComponent<ParticleSystem>();
        L1 = transform.Find("Particles 2").Find("Point Light").GetComponent<Light>();
        L2 = transform.Find("Particles 1").Find("Point Light").GetComponent<Light>();
    }
    private float duration = 1.0f;
    ParticleSystem part1;
    ParticleSystem part2;
    Light L1;
    Light L2;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (part2.particleCount == 0) { part2.Stop(); part1.Stop(); L1.intensity = 0; L2.intensity = 0; }
            else
            {
                L1.intensity = 1;
                L2.intensity = 1;
            }
            part2.Play();
            part1.Play();
            
            
        }
    }
}
