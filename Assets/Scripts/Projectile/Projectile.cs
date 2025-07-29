using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
	[SerializeField] 
	protected float speed = 10f;
	[SerializeField]
	private float colliderDelay = 0.15f;
	protected Vector3 direction;

	protected WeaponInstance weaponData;
	protected int reboundLeft = 0;
	[SerializeField]
	private Collider projectileCollider;
	private float enableColliderDelay;
	private bool waitingToEnableCollider;
	protected bool isPiercing = false;
	protected bool canSlowOnImpact = false;

	public void SetSlowOnImpact()
	{
		canSlowOnImpact = true;
	}

	public void SetPiercing()
	{
		isPiercing = true;
	}

	public void SetRebound(int count)
	{
		reboundLeft += count;
	}

	public void DespawnProjectile()
	{
		ProjectileManager.Instance.UnregisterProjectile(this);
		EntityFactory.Instance.Despawn(weaponData.baseData.weaponId, gameObject);
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if ((weaponData.baseData.hittableLayers.value & (1 << other.gameObject.layer)) == 0)
		{
			return;
		}

		if (other.TryGetComponent<IDamageable>(out IDamageable target))
		{
			if (!isPiercing)
			{
				DespawnProjectile();
			}

			target.TakeDamage(weaponData.baseData.BaseDamage * RarityManager.Instance.Get(weaponData.rarity).damageMultiplier, canSlowOnImpact);
			SoundManager.Instance.PlayImpact();
		}

		if (other.gameObject.layer == LayerMask.NameToLayer("Arena"))
		{
			if(reboundLeft <= 0)
			{
				DespawnProjectile();
			}

			Vector3 normal = (transform.position - other.ClosestPoint(transform.position)).normalized;
			direction = Vector3.Reflect(direction, normal);

			Quaternion rotation = Quaternion.LookRotation(direction);
			transform.rotation = rotation;

			reboundLeft--;
		}
	}

	protected virtual void Update()
	{
		if (waitingToEnableCollider)
		{
			enableColliderDelay -= Time.deltaTime;
			if (enableColliderDelay <= 0f)
			{
				projectileCollider.enabled = true;
				waitingToEnableCollider = false;
			}
		}
	}

	public virtual void OnSpawn()
	{
		ProjectileManager.Instance.RegisterProjectile(this);
		enableColliderDelay = colliderDelay;
		waitingToEnableCollider = true;
		projectileCollider.enabled = false;
		isPiercing = false;
		canSlowOnImpact = false;
		reboundLeft = 0;
	}

	public virtual void OnDespawn() 
	{
		Debug.Log($"Despawn projectile {gameObject.name}");
	}
}