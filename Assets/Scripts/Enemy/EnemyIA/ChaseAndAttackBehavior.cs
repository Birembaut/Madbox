using UnityEngine;

public class ChaseAndAttackBehaviour : EnemyBehaviour
{
	[SerializeField] 
	private float attackRange = 1.5f;
	[SerializeField] 
	private float attackCooldown = 1.5f;

	private float cooldownTimer;

	private bool isAttacking;
	private Transform player;
	private Animator animator;

	private void Start()
	{
		player = GameManager.Instance.Player.transform;
		animator = GetComponentInChildren<Animator>();
	}

	public override void Tick()
	{
		if (player == null)
		{
			return;
		}

		float distance = Vector3.Distance(transform.position, player.position);

		bool isMoving = false;
		if (!isAttacking && distance > attackRange)
		{
			Vector3 direction = (player.position - transform.position).normalized;
			transform.position += direction * moveSpeed * Time.deltaTime * slowValue;
			transform.forward = direction;
			isMoving = true;
		}
		else if (cooldownTimer <= 0f && !isAttacking)
		{
			animator.SetTrigger("Attack");
			isAttacking = true;
		}
		animator.SetBool("IsMoving", isMoving);

		cooldownTimer -= Time.deltaTime;
	}

	public override void Attack()
	{
		float distance = Vector3.Distance(transform.position, player.position);

		if (distance < attackRange)
		{
			player.GetComponent<IDamageable>().TakeDamage(enemy.EnemyData.Damage);
		}
	}

	public override void FinishAttack()
	{
		isAttacking = false;
		cooldownTimer = attackCooldown;
	}

	public override void OnDespawn()
	{
		isAttacking = false;
	}
}