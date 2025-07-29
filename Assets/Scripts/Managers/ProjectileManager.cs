using System.Collections.Generic;

public class ProjectileManager : Singleton<ProjectileManager>
{
	private List<Projectile> projectiles = new List<Projectile>();

	private void OnEnable()
	{
		GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
		WaveManager.Instance.WaveEnded += OnWaveEnded;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
		WaveManager.Instance.WaveEnded -= OnWaveEnded;
	}

	private void HandleGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.GameOver:
				{
					DespawnProjectiles();
                    break;
				}
			default:
				break;
		}
	}

	private void OnWaveEnded()
	{
		DespawnProjectiles();
	}

	public void RegisterProjectile(Projectile projectile)
	{
		projectiles.Add(projectile);
	}

	public void UnregisterProjectile(Projectile projectile)
	{
		projectiles.Remove(projectile);
	}

	private void DespawnProjectiles()
	{
		for (int projectileIndex = projectiles.Count - 1; projectileIndex >= 0; projectileIndex--)
		{
			projectiles[projectileIndex].DespawnProjectile();
		}
	}
}