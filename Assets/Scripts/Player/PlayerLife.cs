using System.Collections;
using UnityEngine;

public class PlayerLife : MonoBehaviour, IDamageable
{
	public bool IsDead = false;

	[SerializeField] 
	private float maxHealth = 3f;
	private float additionalMaxHealth = 0f;
	private float currentHealth;

	[SerializeField]
	private float invincibilityDuration = 1f;
	private float currentInvincibilityDuration = 0f;

	[SerializeField]
	private Renderer modelRenderer;
	[SerializeField]
	private Color hitColor = Color.red;
	[SerializeField]
	private float flashDuration = 0.1f;
	private Animator animator;

	private MaterialPropertyBlock propertyBlock;
	private Coroutine flashCoroutine;
	private WorldHealthBar healthBar;

	private void Start()
	{
		healthBar = GetComponentInChildren<WorldHealthBar>();
		healthBar.gameObject.SetActive(false);
		animator = GetComponentInChildren<Animator>();
		propertyBlock = new MaterialPropertyBlock();
		modelRenderer = GetComponentInChildren<Renderer>();
		GameManager.Instance.OnGameStateChanged += HandleStateChange;
		UpgradeManager.Instance.UpgradeAcquired += OnUpgradeAcquired;
	}

	private void OnDestroy()
	{
		GameManager.Instance.OnGameStateChanged -= HandleStateChange;
		UpgradeManager.Instance.UpgradeAcquired -= OnUpgradeAcquired;
	}

	private void HandleStateChange(GameState state)
	{
		switch (state)
		{
			case GameState.GameOver:
				{
					animator.SetTrigger("Reset"); 
					animator.SetFloat("Speed", 0);
					healthBar.gameObject.SetActive(false);
					additionalMaxHealth = 0;
					break;
				}
			case GameState.Playing:
				{
					currentHealth = maxHealth;
					IsDead = false;
					healthBar.gameObject.SetActive(true);
					UpdateHealthUI();
					break;
				}

			default:
				{
					healthBar.gameObject.SetActive(false);
					break;
				}
		}
	}

	private void OnUpgradeAcquired(UpgradeData data)
	{
		if(data.upgradeID == UpgradeID.MaxHealthUp)
		{
			additionalMaxHealth += data.value;
			TakeDamage(-data.value);
		}
	}

	private void Update()
	{
		if (currentInvincibilityDuration > 0f)
		{
			currentInvincibilityDuration -= Time.deltaTime;
		}

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.F2))
		{
			TakeDamage(1);
        }
#endif
	}

	public void TakeDamage(float amount, bool isSlow = false)
	{
		if (IsDead)
		{
			return;
		}

		if (currentInvincibilityDuration > 0f)
		{
			return;
		}

		currentHealth -= amount;
		UpdateHealthUI();
		UIManager.Instance.AddDamageFlash();
		CameraManager.Instance.PlayFeedback();
		if (currentHealth <= 0)
		{
			Die();
		}
		else
		{
			FlashHit();
			currentInvincibilityDuration = invincibilityDuration;
		}
	}

	private void Die()
	{
		IsDead = true;
		animator.SetTrigger("Death");
		currentInvincibilityDuration = 1f;
		StartCoroutine(HandleDeathDelay());
	}

	private IEnumerator HandleDeathDelay()
	{
		yield return new WaitForSeconds(1.0f);
		GameManager.Instance.TriggerGameOver(false);
	}

	private void FlashHit()
	{
		ResetCoroutine();

		flashCoroutine = StartCoroutine(FlashCoroutine());
	}

	private IEnumerator FlashCoroutine()
	{
		modelRenderer.GetPropertyBlock(propertyBlock);
		propertyBlock.SetColor("_BaseColor", hitColor);
		modelRenderer.SetPropertyBlock(propertyBlock);

		yield return new WaitForSeconds(flashDuration);

		propertyBlock.SetColor("_BaseColor", Color.white);
		modelRenderer.SetPropertyBlock(propertyBlock);
	}

	private void ResetCoroutine()
	{
		if (flashCoroutine != null)
		{
			StopCoroutine(flashCoroutine);
		}

		propertyBlock.SetColor("_BaseColor", Color.white);
		modelRenderer.SetPropertyBlock(propertyBlock);
	}

	private void UpdateHealthUI()
	{
		if (healthBar != null)
		{
			healthBar.SetHealth((float)currentHealth / (maxHealth + additionalMaxHealth));
		}
	}
}