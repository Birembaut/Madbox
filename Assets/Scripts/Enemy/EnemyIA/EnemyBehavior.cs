using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
	protected Enemy enemy;
	[SerializeField]
	protected float moveSpeed;
	protected float slowValue = 1f;

	protected virtual void Awake()
	{
		enemy = GetComponent<Enemy>();
	}

	public void SetSlowValue(float value)
	{
		slowValue = value;
	}

	public abstract void Tick();
	public abstract void Attack();
	public abstract void FinishAttack();
	public abstract void OnDespawn();
}