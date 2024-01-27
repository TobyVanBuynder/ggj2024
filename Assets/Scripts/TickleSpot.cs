using System.Collections;
using UnityEngine;

public class TickleSpot : MonoBehaviour
{
    public void StartTickleMinigame()
    {
        StartCoroutine(TickleSuccess());
    }

    private IEnumerator TickleSuccess()
    {
        yield return new WaitForSeconds(0.5f);

        if (Random.Range(0f, 1f) > .5f)
        {
            
        }
    }
}