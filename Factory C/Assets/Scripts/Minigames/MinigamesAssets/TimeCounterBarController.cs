using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TimeCounterBarController : MonoBehaviour
{
    public Transform toScale;
    public float maxTime = 2f;
    private Coroutine barCoroutine;
    public bool reverse = false;

    private void Start()
    {
        ResetBar();
    }

    public void StartTimer()
    {
        if (barCoroutine == null)
        {
            barCoroutine = StartCoroutine(IncreaseBar());
        }
    }

    public void StopTimer()
    {
        if (barCoroutine != null)
        {
            StopCoroutine(barCoroutine);
            barCoroutine = null;
        }
    }

    private IEnumerator IncreaseBar()
    {
        Vector3 initialScale = toScale.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < maxTime)
        {
            elapsedTime += Time.deltaTime;
            initialScale.x = (elapsedTime / maxTime);
            if (reverse)
            {
                initialScale.x = 1 - initialScale.x;
            }
            toScale.localScale = initialScale;
            yield return null;
        }

        initialScale.x = 1;
        if (reverse) 
        {
            initialScale.x = 0;
        }
        toScale.transform.localScale = initialScale;
        barCoroutine = null;
    }


    public void ResetBar()
    {
        if (barCoroutine != null)
        {
            StopCoroutine(barCoroutine);
            barCoroutine = null;
        }

        Vector3 initialScale = toScale.transform.localScale;
        initialScale.x = 0;
        if (reverse) { 
            initialScale.x = 1;
        }
        toScale.transform.localScale = initialScale;
    }
}
