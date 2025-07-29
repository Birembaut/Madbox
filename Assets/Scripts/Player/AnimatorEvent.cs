using UnityEngine;

public class AnimatorEvent : MonoBehaviour
{
	public PlayerAttack Player;
	public Enemy Enemy;

	public void Attack()
	{
		if (Player != null)
		{
			Player.Attack();
		}

		if (Enemy != null)
		{
			Enemy.Attack();
		}
	}

	public void FinishAttack()
	{
		if (Player != null)
		{
			Player.FinishAttack();
		}

		if (Enemy != null)
		{
			Enemy.FinishAttack();
		}
	}
}
