using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutMsg : MonoBehaviour
{
    public float time = 3f;
    public Transform rect;

    public void Show()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        rect.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        rect.gameObject.SetActive(false);
    }

}
