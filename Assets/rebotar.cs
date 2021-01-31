using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rebotar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeInSine).setLoopPingPong();
        LeanTween.scale(gameObject, Vector3.one, 1f).setEase(LeanTweenType.easeOutSine).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
