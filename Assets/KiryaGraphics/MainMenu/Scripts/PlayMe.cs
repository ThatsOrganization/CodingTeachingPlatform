using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Anim = transform.GetComponent<Animator>();
    }
    Animator Anim;
    // Update is called once per frame
    public void Play(AnimationClip AnimClip)
    {
        Anim.Play(AnimClip.name);
    }
}
