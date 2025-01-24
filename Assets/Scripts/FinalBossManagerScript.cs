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

    public Transform swordTransform;
    public float swordRotationDuration = 0.5f;

    private int _currentHealthIndex;
    private bool _bossDefeated = false;

    public AudioSource bossMusicSource;
    public AudioClip defeatClip;

    public GameObject player;
    public GameObject attackArea;
    private Renderer _attackAreaRenderer;
    public Material attackAreaMaterial;

    public Color startColor = Color.white;
    public Color chargedColor = Color.red;
    private bool _isCharging = false;
    private float _chargeTime = 2.5f;
    private float _cooldownTime = 3f;
    private float _chargeTimer = 0f;
    private float _cooldownTimer = 0f;

    public RawImage healthBar;
    public Sprite[] healthBarSprites;

    private bool _damageTaken = false;

    public TextMeshProUGUI potionText;
    public int potionCount = 1;

    public GameObject respawnButton;
    private bool _isDead = false;

    private MovementController movementController;

    public RawImage finalScreenRawImage;
    public Button retryButton;
    public Button menuButton;
    public Button quitButton;


    private void Start()
    {
        movementController = player.GetComponent<MovementController>();

        StartCoroutine(ShowRawImageWithFadeOut(kilnRawImage));
        BossDetectionScript.e_Collision += PlayerCollision;

        _attackAreaRenderer = attackArea.GetComponent<Renderer>();
        if (_attackAreaRenderer != null)
        {
            attackAreaMaterial = _attackAreaRenderer.material;
            attackAreaMaterial.color = startColor;
        }

        if (healthBar != null && healthBarSprites.Length > 0)
        {
            healthBar.texture = healthBarSprites[0].texture;
        }

        UpdatePotionText();
        respawnButton.SetActive(false);
    }

    private void UpdatePotionText()
    {
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
            _currentHealthIndex = 0;
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
        if (_currentHealthIndex < bossHealthTextures.Length)
        {
            if (gwynRawImage != null)
            {
                gwynRawImage.texture = bossHealthTextures[_currentHealthIndex];
            }
        }
    }

    private void BossDefeated()
    {
        _bossDefeated = true;

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

        ShowEndGameOptions();
    }

    private void ShowEndGameOptions()
    {
        finalScreenRawImage.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        retryButton.onClick.AddListener(RestartBossFight);
        menuButton.onClick.AddListener(ReturnToMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void RestartBossFight()
    {
        SceneManager.LoadScene("FinalBoss", LoadSceneMode.Single);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }

    private void QuitGame()
    {
        Application.Quit();
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
        if (collision.gameObject.CompareTag("Boss") && !_bossDefeated)
        {
            HitBoss();
        }
    }

    public void HitBoss()
    {
        if (_bossDefeated) return;

        _currentHealthIndex++;

        if (_currentHealthIndex >= maxBossHealth)
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
        if (_isDead)
        {
            player.GetComponent<MeshRenderer>().enabled = false;
            player.GetComponent<MovementController>().enabled = false;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            return;
        }

        HandleChargingAndCooldown();

        if (Input.GetKeyDown(KeyCode.Q) && potionCount > 0 && !_isDead)
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
                if (!_isCharging && _cooldownTimer <= 0f)
                {
                    StartChargingAttack();
                }

                if (_isCharging)
                {
                    _chargeTimer += Time.deltaTime;
                    attackAreaMaterial.color = Color.Lerp(startColor, chargedColor, _chargeTimer / _chargeTime);

                    if (_chargeTimer >= _chargeTime)
                    {
                        ExecuteAttack();
                    }
                }
            }
            else
            {
                if (_isCharging)
                {
                    _chargeTimer = 0f;
                    attackAreaMaterial.color = startColor;
                }
            }
        }

        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }

    private void StartChargingAttack()
    {
        _isCharging = true;
        _chargeTimer = 0f;
    }

    private void ExecuteAttack()
    {
        if (_bossDefeated) return;

        StartCoroutine(RotateSwordAroundBoss());

        if (healthBar != null && healthBarSprites.Length > 1)
        {
            if (!_damageTaken)
            {
                healthBar.texture = healthBarSprites[1].texture;
                _damageTaken = true;
            }
            else
            {
                healthBar.texture = healthBarSprites[2].texture;
                _damageTaken = false;
                _isDead = true;
                ShowRespawnButton();
            }
        }

        _isCharging = false;
        _cooldownTimer = _cooldownTime;
        attackAreaMaterial.color = startColor;
    }


    private IEnumerator RotateSwordAroundBoss()
    {
        if (swordTransform == null || bossObject == null) yield break;

        Vector3 bossCenter = bossObject.transform.position;
        float elapsedTime = 0f;
        float adjustedDuration = swordRotationDuration / 2f;

        while (elapsedTime < adjustedDuration)
        {
            float angle = (-360f / adjustedDuration) * Time.deltaTime;

            swordTransform.RotateAround(bossCenter, Vector3.up, angle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        swordTransform.RotateAround(bossCenter, Vector3.up, -360f - (-360f * (elapsedTime / adjustedDuration)));
    }

    private void UsePotion()
    {
        if (potionCount > 0 && !_isDead)
        {
            potionCount--;
            _damageTaken = false;
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
        respawnButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Respawn()
    {
        SceneManager.LoadScene("FinalBoss", LoadSceneMode.Single);

        _isDead = false;
        respawnButton.SetActive(false);
        _currentHealthIndex = 0;

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
