using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PS = transform.GetComponent<ParticleSystem>();
        StartCoroutine(Go());
    }
    ParticleSystem PS;
    // Update is called once per frame
    void Update()
    {
        
    }
    public int SecondsDelay;
    IEnumerator Go()
    {
        while (true)
        {
            yield return new WaitForSeconds(SecondsDelay);
            PS.Stop();
            PS.Play();
            
        }
    }
}
