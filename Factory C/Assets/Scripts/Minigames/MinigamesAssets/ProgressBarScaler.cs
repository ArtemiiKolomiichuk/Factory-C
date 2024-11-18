using System.Collections;
using UnityEngine;

public class ProgressBarScaler : MonoBehaviour
{
    [SerializeField]
    private GameObject toScale = null;

    [SerializeField]
    private float duration = 0.5f;

    private float lastProgress = 0;
    private float lastMaxProgress = 0;

    public float getLastProgress() 
    { 
        return lastProgress;
    }

    public float getLastMaxProgress()
    {
        return lastMaxProgress;
    }

    private void Awake()
    {
        Vector3 scale = toScale.transform.localScale;
        scale.x = 0;
        toScale.transform.localScale = scale;
    }

    public float ChangeProgressBar(float currentPoints, float maxPoints)
    {
        lastProgress = currentPoints;
        lastMaxProgress = maxPoints;
        float targetScaleX = lastProgress / lastMaxProgress;
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(targetScaleX));
        return targetScaleX;
    }

    private IEnumerator ScaleOverTime(float targetScaleX)
    {
        Vector3 initialScale = toScale.transform.localScale;
        Vector3 targetScale = new Vector3(targetScaleX, initialScale.y, initialScale.z);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            toScale.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toScale.transform.localScale = targetScale;
    }
}
