using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FinalBossManager : MonoBehaviour
{
    public RawImage kilnRawImage;
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

    public GameObject player;
    public GameObject attackArea;
    private Renderer attackAreaRenderer;
    public Material attackAreaMaterial;

    public Color startColor = Color.white;
    public Color chargedColor = Color.red;
    private bool isCharging = false;
    private float chargeTime = 3f;
    private float cooldownTime = 5f;
    private float chargeTimer = 0f;
    private float cooldownTimer = 0f;

    public RawImage healthBar; // RawImage do paska zdrowia
    public Sprite[] healthBarSprites; // Tablica obrazków dla paska zdrowia

    private bool damageTaken = false; // Flaga do œledzenia otrzymanych obra¿eñ

    public TextMeshProUGUI potionText; // Dodajemy zmienn¹ do przechowywania tekstu mikstur
    public int potionCount = 1; // Pocz¹tkowa liczba mikstur

    public GameObject respawnButton; // Przycisk respawn
    private bool isDead = false; // Flaga sprawdzaj¹ca, czy gracz jest martwy

    private MovementController movementController; // Odwo³anie do komponentu MovementController

    private void Start()
    {
        // Uzyskujemy odwo³anie do MovementController
        movementController = player.GetComponent<MovementController>();

        StartCoroutine(ShowRawImageWithFadeOut(kilnRawImage));
        BossDetectionScript.e_Collision += PlayerCollision;

        attackAreaRenderer = attackArea.GetComponent<Renderer>();
        if (attackAreaRenderer != null)
        {
            attackAreaMaterial = attackAreaRenderer.material;
            attackAreaMaterial.color = startColor;
        }

        if (healthBar != null && healthBarSprites.Length > 0)
        {
            healthBar.texture = healthBarSprites[0].texture; // Ustaw obrazek na pe³ne HP na pocz¹tku
        }

        UpdatePotionText(); // Na pocz¹tku aktualizujemy licznik mikstur
        respawnButton.SetActive(false); // Ukrywamy przycisk respawn na pocz¹tku
    }

    private void UpdatePotionText()
    {
        // Aktualizujemy tekst wyœwietlaj¹cy tylko liczbê mikstur
        potionText.text = potionCount.ToString();
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
        if (rawImage != null)
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
        if (gwynRawImage != null)
        {
            gwynRawImage.gameObject.SetActive(true);
            currentHealthIndex = 0;
            UpdateBossHealthImage();
        }

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
            if (gwynRawImage != null)
            {
                gwynRawImage.texture = bossHealthTextures[currentHealthIndex];
            }
        }
    }

    private void BossDefeated()
    {
        bossDefeated = true;

        if (bossObject != null)
        {
            bossObject.SetActive(false);
        }

        StartCoroutine(HandleBossDefeatSequence());
    }

    private IEnumerator HandleBossDefeatSequence()
    {
        yield return StartCoroutine(FadeOutMusic(bossMusicSource));
        yield return StartCoroutine(FadeOutRawImage(gwynRawImage, 1f));

        yield return new WaitForSeconds(1f);

        PlayDefeatSound();
        yield return StartCoroutine(FadeInRawImage(winPromptRawImage, 0.5f));

        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(FadeOutRawImage(winPromptRawImage, 1f));

        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }

    private IEnumerator FadeImage(RawImage image, float startAlpha, float endAlpha, float duration)
    {
        if (image != null)
        {
            float elapsedTime = 0f;
            Color originalColor = image.color;

            while (elapsedTime < duration)
            {
                image.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, endAlpha);
        }
    }

    private IEnumerator FadeOutRawImage(RawImage rawImage, float duration)
    {
        if (rawImage != null)
        {
            yield return FadeImage(rawImage, 1f, 0f, duration);
            rawImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeInRawImage(RawImage rawImage, float duration)
    {
        if (rawImage != null)
        {
            rawImage.gameObject.SetActive(true);
            yield return FadeImage(rawImage, 0f, 1f, duration);
        }
    }

    private void PlayerCollision(object s, Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss") && !bossDefeated)
        {
            HitBoss();
        }
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

    private void Update()
    {
        if (isDead)
        {
            player.GetComponent<MeshRenderer>().enabled = false;
            player.GetComponent<MovementController>().enabled = false;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            return;
        }

        // Kontrolowanie stanu ataku i cooldownu
        HandleChargingAndCooldown();

        // Sprawdzamy, czy gracz nacisn¹³ "Q", aby u¿yæ mikstury
        if (Input.GetKeyDown(KeyCode.Q) && potionCount > 0 && !isDead)
        {
            UsePotion();
        }
    }

    private void HandleChargingAndCooldown()
    {
        if (attackArea != null && player != null)
        {
            float distance = Vector3.Distance(player.transform.position, attackArea.transform.position);

            if (distance < 12f)
            {
                if (!isCharging && cooldownTimer <= 0f)
                {
                    StartChargingAttack();
                }

                if (isCharging)
                {
                    chargeTimer += Time.deltaTime;
                    attackAreaMaterial.color = Color.Lerp(startColor, chargedColor, chargeTimer / chargeTime);

                    if (chargeTimer >= chargeTime)
                    {
                        ExecuteAttack();
                    }
                }
            }
            else
            {
                if (isCharging)
                {
                    chargeTimer = 0f;
                    attackAreaMaterial.color = startColor;
                }
            }
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void StartChargingAttack()
    {
        isCharging = true;
        chargeTimer = 0f;
    }

    private void ExecuteAttack()
    {
        if (bossDefeated)
        {
            return;
        }


        if (healthBar != null && healthBarSprites.Length > 1)
        {
            if (!damageTaken)
            {
                healthBar.texture = healthBarSprites[1].texture; // Po³owa HP
                damageTaken = true; // Flaga zmieniona na true po pierwszym ataku
            }
            else
            {
                healthBar.texture = healthBarSprites[2].texture; // Brak HP
                damageTaken = false; // Resetowanie flagi po drugim ataku
                isDead = true;  // Gracz jest martwy
                ShowRespawnButton();  // Pokazujemy przycisk respawn
            }
        }

        isCharging = false;
        cooldownTimer = cooldownTime;
        attackAreaMaterial.color = startColor; // Resetowanie koloru ataku
    }

    private void UsePotion()
    {
        if (potionCount > 0 && !isDead)
        {
            potionCount--;
            damageTaken = false;
            healthBar.texture = healthBarSprites[0].texture;
            UpdatePotionText();

            if (potionCount <= 0)
            {
                potionText.text = "0";
            }
        }
    }

    private void ShowRespawnButton()
    {
        respawnButton.SetActive(true); // Pokazujemy przycisk respawn
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Respawn()
    {
        SceneManager.LoadScene("FinalBoss", LoadSceneMode.Single);

        isDead = false;
        respawnButton.SetActive(false); // Ukrywamy przycisk respawn
        currentHealthIndex = 0;

        // Ponowne w³¹czenie ruchu po respawnie
        if (movementController != null)
        {
            movementController.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        BossDetectionScript.e_Collision -= PlayerCollision;
    }
}
