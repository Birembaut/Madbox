using UnityEngine;

public class RoamAndProjectileBehavior : EnemyBehaviour
{
	[SerializeField] 
	private float roamRadius = 3f;
	[SerializeField] 
	private float attackCooldown = 1.5f;

	private float shootCooldown;

	private bool isAttacking;
	private Transform player;
	private Animator animator;
	private Vector3 targetPosition;

	private void Start()
	{
		player = GameManager.Instance.Player.transform;
		animator = GetComponentInChildren<Animator>();
	}

	public override void Tick()
	{
		if (player == null || isAttacking)
		{
			return;
		}

		shootCooldown -= Time.deltaTime;
		if (shootCooldown < 0f)
		{
			animator.SetTrigger("Attack");
			isAttacking = true;
			Vector3 newDirection = (player.position - transform.position);
			transform.forward = newDirection.normalized;
		}
		else
		{
			Vector3 direction = (targetPosition - transform.position);
			if (direction.magnitude < 0.1f)
			{
				PickNewDestination();
			}
			else
			{
				transform.position += direction.normalized * moveSpeed * Time.deltaTime * slowValue;
				transform.forward = direction.normalized;
			}
		}
	}

	private void PickNewDestination()
	{
		Vector2 offset = Random.insideUnitCircle * roamRadius;
		Vector3 rawTarget = transform.position + new Vector3(offset.x, 0f, offset.y);

		Bounds bounds = WaveManager.Instance.SpawnArea.bounds;

		rawTarget.x = Mathf.Clamp(rawTarget.x, bounds.min.x, bounds.max.x);
		rawTarget.z = Mathf.Clamp(rawTarget.z, bounds.min.z, bounds.max.z);

		targetPosition = new Vector3(rawTarget.x, transform.position.y, rawTarget.z);
	}

	public override void Attack()
	{
		if (player == null)
		{
			return;
		}

		Vector3 direction = (player.position - transform.position).normalized;
		GameObject projGO = EntityFactory.Instance.Spawn(enemy.EnemyData.WeaponIfNecessary.baseData.EntityID, transform.position);
		Projectile projectile = projGO.GetComponent<Projectile>();
		switch (projectile)
		{
			case SimpleProjectile simpleProjectile:
				{
					simpleProjectile.Launch(player.position, enemy.EnemyData.WeaponIfNecessary);
					break;
				}

			case HomingProjectile homingProjectile:
				{
					homingProjectile.Launch(player.position, enemy.EnemyData.WeaponIfNecessary, player.gameObject);
					break;
				}

			case BoomerangProjectile boomerangProjectile:
				{
					boomerangProjectile.Launch(player.position, enemy.EnemyData.WeaponIfNecessary, this.gameObject);
					break;
				}
		}
	}

	public override void FinishAttack()
	{
		shootCooldown = attackCooldown;
		isAttacking = false;
	}

	public override void OnDespawn()
	{
		isAttacking = false;
	}
}