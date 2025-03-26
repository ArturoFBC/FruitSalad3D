using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DisplaceTowardsInXSeconds : MonoBehaviour
{
    public void Set(Vector3 position, float time)
    {
        StartCoroutine(DisplaceTowards(position, time));
    }

    private IEnumerator DisplaceTowards(Vector3 position, float time)
    {
        float currentTime = 0;
        Vector3 initialPosition = transform.position;

        while (currentTime < time)
        {
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(initialPosition, position, currentTime / time);
        }
    }
}
