using System.Collections;
using UnityEngine;

public class LightIntensityController : MonoBehaviour
{
    private Light directionalLight;

    void Start()
    {
        directionalLight = GetComponent<Light>();
        StartCoroutine(LightCycle());
    }

    IEnumerator LightCycle()
    {
        while (true)
        {
            yield return StartCoroutine(ChangeIntensity(1f, 2f, 0.5f));
            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(ChangeIntensity(1f, 2.5f, 0.5f));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(ChangeIntensity(1f, 2f, 0.5f));
            yield return new WaitForSeconds(8f);
        }
    }

    IEnumerator ChangeIntensity(float startIntensity, float endIntensity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration / 2f)
        {
            elapsed += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, elapsed / (duration / 2f));
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration / 2f)
        {
            elapsed += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(endIntensity, startIntensity, elapsed / (duration / 2f));
            yield return null;
        }
    }
}
