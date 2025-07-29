using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
	public delegate void OnEnemyDied(Enemy enemy);
	public OnEnemyDied EnemyDied;
	public delegate void OnWaveEnded();
	public OnWaveEnded WaveEnded;

	public BoxCollider SpawnArea;
	[SerializeField] 
	private WaveSetData waveSet;
	private int currentWaveIndex = -1;


	private List<Enemy> aliveEnemies = new List<Enemy>();
	Coroutine spawnCoroutine;

	private void OnEnable()
	{
		GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
		EnemyDied += OnEnemyDiedEvent;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
		EnemyDied -= OnEnemyDiedEvent;
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.F1))
		{
            for (int enemyIndex = aliveEnemies.Count - 1; enemyIndex >= 0; enemyIndex--)
            {
				Enemy enemy = aliveEnemies[enemyIndex];
				enemy.TakeDamage(enemy.CurrentHealth);
			}
        }
#endif
	}

	public Enemy FindNearestTarget(Vector3 position)
	{
		Enemy nearestTarget = null;
		float distance = float.MaxValue;

		int enemiesCount = aliveEnemies.Count;
		for (int enemyIndex = 0; enemyIndex < enemiesCount; enemyIndex++)
		{
			Enemy enemy = aliveEnemies[enemyIndex];

			if (enemy.State != Enemy.EnemyState.Alive)
			{
				continue;
			}

			float currentDistance = Vector3.Distance(position, enemy.transform.position);
			if (currentDistance < distance)
			{
				distance = currentDistance;
				nearestTarget = enemy;
			}
		}

		return nearestTarget;
	}

	public void SpawnNextWave()
	{
		currentWaveIndex++;
		GameManager.Instance.CurrentSession.WavesReach++;
		Debug.Log($"Wave {currentWaveIndex} reached");

		WaveData wave = waveSet.Waves[currentWaveIndex];
		spawnCoroutine = StartCoroutine(SpawnWave(wave));
	}

	public void RegisterEnemy(Enemy enemy)
	{
		aliveEnemies.Add(enemy);
	}

	public void UnregisterEnemy(Enemy enemy)
	{
		aliveEnemies.Remove(enemy);
	}

	protected void HandleGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.Playing:
				{
                    for (int waveSetIndex = 0; waveSetIndex < waveSet.Waves.Count; waveSetIndex++)
					{
						for (int EnemyToSpawnIndex = 0; EnemyToSpawnIndex < waveSet.Waves[waveSetIndex].Enemies.Count; EnemyToSpawnIndex++)
						{
							EnemySpawnData enemySpawnData = waveSet.Waves[waveSetIndex].Enemies[EnemyToSpawnIndex];
							EntityFactory.Instance.RegisterData(enemySpawnData.EnemyData);
							if(enemySpawnData.EnemyData.WeaponIfNecessary.baseData != null)
							{
								EntityFactory.Instance.RegisterData(enemySpawnData.EnemyData.WeaponIfNecessary.baseData);
							}
						}
					}

					currentWaveIndex = -1;
					WaveEnded.Invoke();
					break;
				}
			case GameState.GameOver:
				{
					for (int enemyIndex = aliveEnemies.Count - 1; enemyIndex >= 0; enemyIndex--)
					{
						Enemy enemy = aliveEnemies[enemyIndex];
						EntityFactory.Instance.Despawn(enemy.EnemyData, enemy.gameObject);
						aliveEnemies.RemoveAt(enemyIndex);
					}

					if(spawnCoroutine != null)
					{
						StopCoroutine(spawnCoroutine);
					}

					break;
				}
			default:
				break;
		}
	}

	private void OnEnemyDiedEvent(Enemy enemy)
	{
		aliveEnemies.Remove(enemy);

		if (aliveEnemies.Count == 0)
		{
			if (currentWaveIndex +1  == waveSet.Waves.Count)
			{
				GameManager.Instance.TriggerGameOver(true);
				return;
			}

			WaveEnded.Invoke();
		}
	}

	private IEnumerator SpawnWave(WaveData wave)
	{
		yield return new WaitForSeconds(wave.DelayBeforeNextWave);

		List<EnemySpawnData> enemyList = wave.Enemies;
		int totalToSpawn = 0;

		for (int EnemyToSpawnIndex = 0; EnemyToSpawnIndex < enemyList.Count; EnemyToSpawnIndex++)
		{
			EnemySpawnData enemyData = enemyList[EnemyToSpawnIndex];
			totalToSpawn += enemyData.Count;
		}

		int spawnIndex = 0;
		for (int EnemyToSpawnIndex = 0; EnemyToSpawnIndex < wave.Enemies.Count; EnemyToSpawnIndex++)
		{
			EnemySpawnData enemySpawnData = wave.Enemies[EnemyToSpawnIndex];
			for (int enemyIndex = 0; enemyIndex < enemySpawnData.Count; enemyIndex++)
			{
				Vector3 pos = GetRandomSpawnPosition();
				Enemy enemy = EntityFactory.Instance.Spawn(enemySpawnData.EnemyData.enemyId, pos).GetComponent<Enemy>();
				enemy.Init(enemySpawnData.EnemyData);
				spawnIndex++;
			}
		}
	}

	private Vector3 GetRandomSpawnPosition()
	{
		Bounds bounds = SpawnArea.bounds;

		float x = Random.Range(bounds.min.x, bounds.max.x);
		float z = Random.Range(bounds.min.z, bounds.max.z);

		return new Vector3(x, 0f, z);
	}
}