using UnityEngine;
using System.Collections;

public class LightIntensityController : MonoBehaviour
{
    private Light _directionalLight;
    public AudioClip thunderClip;
    public AudioSource audioSource;

    public AudioClip windSound;
    public AudioSource windAudioSource;

    private PortalController portalController;
    private Coroutine lightCycleCoroutine;
    private bool isCyclePaused = false;

    void Start()
    {
        _directionalLight = GetComponent<Light>();

        portalController = FindFirstObjectByType<PortalController>();


        lightCycleCoroutine = StartCoroutine(LightCycle());

        if (windAudioSource != null && !windAudioSource.isPlaying)
        {
            windAudioSource.loop = true;
            windAudioSource.PlayOneShot(windSound);
        }
    }

    void Update()
    {
        if (portalController != null && portalController.isPlayerInPortal)
        {
            if (!isCyclePaused)
            {
                StopCoroutine(lightCycleCoroutine);
                lightCycleCoroutine = null;
                audioSource.Stop();
                isCyclePaused = true;
                _directionalLight.intensity = 1f;
            }
        }
    }

    IEnumerator LightCycle()
    {
        while (!isCyclePaused)
        {
            yield return StartCoroutine(FlashLight(2.5f, 0.5f));
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(FlashLight(2.5f, 0.5f));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(FlashLight(2.5f, 0.5f));
            yield return new WaitForSeconds(4f);
        }
    }

    IEnumerator FlashLight(float peakIntensity, float fadeDuration)
    {
        if (thunderClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(thunderClip);
        }

        _directionalLight.intensity = peakIntensity;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            _directionalLight.intensity = Mathf.Lerp(peakIntensity, 1f, elapsed / fadeDuration);
            yield return null;
        }

        _directionalLight.intensity = 1f;
    }

    public void ResetLightCycle()
    {
        isCyclePaused = false;
        lightCycleCoroutine = StartCoroutine(LightCycle());
    }
}
