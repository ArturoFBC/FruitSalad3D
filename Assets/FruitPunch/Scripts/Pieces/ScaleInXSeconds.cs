using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleInXSeconds : MonoBehaviour
{
    public void Set(Vector3 finalScale, float time)
    {
        StopAllCoroutines();
        StartCoroutine(DisplaceTowards(finalScale, time));
    }

    private IEnumerator DisplaceTowards(Vector3 finalScale, float time)
    {
        float currentTime = 0;
        Vector3 initialScale = transform.localScale;

        while (currentTime < time)
        {
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, finalScale, currentTime / time);
        }
    }
}
