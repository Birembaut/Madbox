using UnityEngine;

public class BoomerangProjectile : Projectile
{
	private Vector3 origin;
	private GameObject launcher;
	[SerializeField]
	private float maxDistance = 20f;
	[SerializeField]
	private float rotationSpeed = 20f;
	[SerializeField]
	private Transform weaponVisual;
	private bool isRetourning = false;

	public void Launch(Vector3 targetPosition, WeaponInstance weaponData, GameObject launcher)
	{
		origin = transform.position;
		direction = (targetPosition - transform.position).normalized;
		this.weaponData = weaponData;
		reboundLeft = weaponData.baseData.maxBounces;
		this.launcher = launcher;
	}

	protected override void Update()
	{
		base.Update();

		if(isRetourning && launcher != null && launcher.activeSelf)
		{
			direction = (launcher.transform.position - transform.position).normalized;
		}

		transform.position += direction * speed * Time.deltaTime;

		if (!isRetourning && Vector3.Distance(origin, transform.position) >= maxDistance)
		{
			isRetourning = true;
		}

		weaponVisual.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if (isRetourning && other.gameObject == launcher)
		{
			DespawnProjectile();
		}

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
			if (reboundLeft <= 0)
			{
				isRetourning = true;
			}

			Vector3 normal = (transform.position - other.ClosestPoint(transform.position)).normalized;
			direction = Vector3.Reflect(direction, normal);

			reboundLeft--;
		}
	}

	public override void OnSpawn()
	{
		base.OnSpawn();
		isRetourning = false;
	}
}