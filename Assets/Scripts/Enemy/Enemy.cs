using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable, IDamageable
{
	public enum EnemyState
	{
		Spawning,
		Alive,
		Dead,
	}

	[SerializeField]
	private Renderer modelRenderer;
	[SerializeField]
	private Collider enemyCollider;
	[SerializeField] 
	private Color hitColor = Color.red;
	[SerializeField] 
	private float flashDuration = 0.1f; 
	
	private MaterialPropertyBlock propertyBlock;
	private Coroutine flashCoroutine;

	[SerializeField]
	public float CurrentHealth;
	[SerializeField]
	public EnemyData EnemyData;
	[SerializeField]
	public EnemyState State;

	private EnemyBehaviour behaviour;
	private Animator animator;
	private float slowDuration;

	private void Awake()
	{
		propertyBlock = new MaterialPropertyBlock();
		modelRenderer = GetComponentInChildren<Renderer>();
		animator = GetComponentInChildren<Animator>();
		GetComponentInChildren<AnimatorEvent>().Enemy = this;
		behaviour = GetComponent<EnemyBehaviour>();
		enemyCollider = GetComponent<Collider>();
	}

	private void Update()
	{
		if(State != EnemyState.Alive)
		{
			return;
		}

		if(slowDuration > 0)
		{
			slowDuration -= Time.deltaTime;
			if(slowDuration <= 0)
			{
				behaviour.SetSlowValue(1f);
			}
		}

		behaviour.Tick();
	}

	public void Init(EnemyData enemyData)
	{
		this.EnemyData = enemyData;
		CurrentHealth = enemyData.MaxHealth;
	}

	public void TakeDamage(float damage, bool isSlow = false)
	{
		CurrentHealth -= damage;
		FlashHit();

		Vector3 positionForDamage = transform.position + Vector3.up * 1.5f + new Vector3(
			Random.Range(-0.3f, 0.3f),
			0,
			Random.Range(-0.3f, 0.3f));
		GameObject textGO = EntityFactory.Instance.Spawn(UIItemID.FloatingText, positionForDamage);
		FloatingText floatingText = textGO.GetComponent<FloatingText>();
		floatingText.Show(((int)damage).ToString(), Color.white);

		GameObject fxGO = EntityFactory.Instance.Spawn(FxID.Impact, transform.position);
		fxGO.GetComponent<ParticleSystem>().Play();

		if (CurrentHealth <= 0f)
		{
			Die();
		}
		else if(isSlow)
		{
			SetSlow();
		}

	}

	public void Attack()
	{
		behaviour.Attack();
	}

	public void FinishAttack()
	{
		behaviour.FinishAttack();
	}

	private void SetSlow()
	{
		slowDuration = 3.0f;
		behaviour.SetSlowValue(0.4f);
	}

	private void Die()
	{
		State = EnemyState.Dead;
		enemyCollider.enabled = false;
		animator.SetTrigger("Death");
		SoundManager.Instance.PlayEnemyDeath();
		WaveManager.Instance.UnregisterEnemy(this);
		WaveManager.Instance.EnemyDied.Invoke(this);

		transform.DOScale(Vector3.zero, 0.5f)
			.SetEase(Ease.OutBack)
			.OnComplete(() =>
			{
				EntityFactory.Instance.Despawn(EnemyData.EntityID, gameObject);
			});
	}

	public void OnSpawn()
	{
		WaveManager.Instance.RegisterEnemy(this); 
		animator.SetTrigger("Reset");
		State = EnemyState.Spawning;
		enemyCollider.enabled = false;
		GameObject fxGO = EntityFactory.Instance.Spawn(FxID.SpawnBurst, transform.position);
		fxGO.GetComponent<ParticleSystem>().Play();

		transform.localScale = Vector3.zero;
		transform.DOScale(Vector3.one * Random.Range(0.9f, 1.1f), 0.5f)
			.SetEase(Ease.OutBack)
			.OnComplete(() =>
			{
				EnableLogic();
			});
	}

	public void OnDespawn()
	{
		behaviour.SetSlowValue(1f);
		behaviour.OnDespawn();
		ResetCoroutine();
		WaveManager.Instance.UnregisterEnemy(this);
	}

	private void EnableLogic()
	{
		State = EnemyState.Alive;
		enemyCollider.enabled = true;
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
}