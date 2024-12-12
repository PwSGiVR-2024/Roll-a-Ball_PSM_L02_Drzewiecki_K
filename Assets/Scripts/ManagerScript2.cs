using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    public RawImage klinRawImage;
    public RawImage gwynRawImage;
    public RawImage winPromptRawImage;
    public Texture[] bossHealthTextures;
    public float fadeDuration = 1f;
    public float displayDuration = 3f;

    public GameObject bossObject;
    public Collider bossCollider;
    public int maxBossHealth = 4;

    private int currentHealthIndex;
    private bool bossDefeated = false;

    public AudioSource bossMusicSource;
    public AudioClip defeatClip;

    private void Start()
    {
        StartCoroutine(ShowRawImageWithFadeOut(klinRawImage));
        BossDetectionScript.e_Collision += PlayerCollision;
    }

    private void PlayBossMusic()
    {
        if (bossMusicSource != null)
        {
            bossMusicSource.Play();
        }
    }

    private IEnumerator ShowRawImageWithFadeOut(RawImage rawImage)
    {
        rawImage.gameObject.SetActive(true);
        rawImage.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(displayDuration);

        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            rawImage.color = new Color(1, 1, 1, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rawImage.color = new Color(1, 1, 1, 0);
        rawImage.gameObject.SetActive(false);
    }


    private IEnumerator FadeOutMusic(AudioSource audioSource)
    {
        if (audioSource == null) yield break;

        float startVolume = audioSource.volume;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private void PlayDefeatSound()
    {
        if (bossMusicSource != null && defeatClip != null)
        {
            bossMusicSource.PlayOneShot(defeatClip);
        }
    }

    public void TriggerShowGwyn()
    {
        gwynRawImage.gameObject.SetActive(true);
        currentHealthIndex = 0;
        UpdateBossHealthImage();

        if (bossObject != null)
        {
            bossObject.SetActive(true);
        }

        PlayBossMusic();
    }

    private void UpdateBossHealthImage()
    {
        if (currentHealthIndex < bossHealthTextures.Length)
        {
            gwynRawImage.texture = bossHealthTextures[currentHealthIndex];
        }
    }

    private void BossDefeated()
    {
        bossDefeated = true;
        Debug.Log("Boss pokonany!");

        if (bossObject != null)
        {
            bossObject.SetActive(false);
        }

        StartCoroutine(HandleBossDefeatSequence());
    }

    private IEnumerator HandleBossDefeatSequence()
    {
        print("boss defeated");
        yield return StartCoroutine(FadeOutMusic(bossMusicSource));
        yield return StartCoroutine(FadeOutRawImage(gwynRawImage));

        yield return new WaitForSeconds(3f);

        PlayDefeatSound();
        yield return StartCoroutine(FadeInRawImage(winPromptRawImage));

        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeOutRawImage(winPromptRawImage));
    }


    private IEnumerator FadeOutRawImage(RawImage rawImage)
    {
        float elapsedTime = 0;
        Color originalColor = rawImage.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            rawImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rawImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        rawImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeInRawImage(RawImage rawImage)
    {
        rawImage.gameObject.SetActive(true);
        float elapsedTime = 0;
        Color originalColor = rawImage.color;
        rawImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            rawImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rawImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    private void PlayerCollision(object s, Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss") && !bossDefeated)
        {
            HitBoss();
        }
        Debug.Log($"Kolizja z: {collision.gameObject.name}");
    }

    public void HitBoss()
    {
        if (bossDefeated) return;

        currentHealthIndex++;

        if (currentHealthIndex >= maxBossHealth)
        {
            UpdateBossHealthImage();
            BossDefeated();
        }
        else
        {
            UpdateBossHealthImage();
        }
    }
}
