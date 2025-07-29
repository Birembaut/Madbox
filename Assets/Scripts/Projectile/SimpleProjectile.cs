using UnityEngine;

public class SimpleProjectile : Projectile
{
	public void Launch(Vector3 targetPosition, WeaponInstance weaponData)
	{
		direction = (targetPosition - transform.position).normalized;
		this.weaponData = weaponData;
		reboundLeft = weaponData.baseData.maxBounces;
	}

	protected override void Update()
	{
		base.Update();
		transform.position += direction * speed * Time.deltaTime;
		transform.forward = direction;
	}
}