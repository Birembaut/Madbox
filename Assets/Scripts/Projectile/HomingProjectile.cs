using UnityEngine;

public class HomingProjectile : Projectile
{
	private GameObject projectileTarget;
	[SerializeField]
	private float rotateSpeed = 50f;

	public void Launch(Vector3 targetPosition, WeaponInstance weaponData, GameObject target)
	{
		this.projectileTarget = target;
		direction = (targetPosition - transform.position).normalized;
		transform.forward = direction;
		this.weaponData = weaponData;
		reboundLeft = weaponData.baseData.maxBounces;
	}

	protected override void Update()
	{
		base.Update();

		Vector3 currentDirection = direction;
		if (projectileTarget != null && projectileTarget.activeSelf == true)
		{
			currentDirection = (projectileTarget.transform.position - transform.position).normalized;
			Quaternion rotation = Quaternion.LookRotation(currentDirection);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
		}
		
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);

		if ((weaponData.baseData.hittableLayers.value & (1 << other.gameObject.layer)) == 0)
		{
			return;
		}

		if (other.TryGetComponent<IDamageable>(out IDamageable target))
		{
			projectileTarget = null;
		}

		if (other.gameObject.layer == LayerMask.NameToLayer("Arena"))
		{
			projectileTarget = null;
		}
	}
}