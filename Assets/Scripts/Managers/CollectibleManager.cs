using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : Singleton<CollectibleManager>
{
	public delegate void OnPickupSpawned(Pickup pickup);
	public OnPickupSpawned PickupSpawned;

	public DropData[] AvailableDrops; 
	[SerializeField] 
	private float dropChance = 0.15f;

	private List<Pickup> spawnedPickups = new List<Pickup>();

	public void Start()
	{
		WaveManager.Instance.EnemyDied += OnEnemyDied;

		int length = AvailableDrops.Length;
		for (int dropIndex = 0; dropIndex < length; dropIndex++)
        {
            EntityFactory.Instance.RegisterData(AvailableDrops[dropIndex]);
        }
	}

	private void OnEnable()
	{
		GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
	}

	private void HandleGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.GameOver:
				{
					for (int pickupIndex = spawnedPickups.Count - 1; pickupIndex >= 0; pickupIndex--)
					{
						spawnedPickups[pickupIndex].Despawn();
					}
					break;
				}
			default:
				break;
		}
	}


	public void OnDestroy()
	{
		WaveManager.Instance.EnemyDied -= OnEnemyDied;
	}

	private void OnEnemyDied(Enemy enemy)
	{
		if (AvailableDrops.Length == 0 || Random.value > dropChance)
		{
			return;
		}

		int index = Random.Range(0, AvailableDrops.Length);
		DropData dropData = AvailableDrops[index];
		GameObject dropGO = EntityFactory.Instance.Spawn(dropData.EntityID, enemy.transform.position);
		Pickup pickup = dropGO.GetComponent<Pickup>();
		spawnedPickups.Add(pickup);
		Rarity rarity = RarityManager.Instance.GetRandomRarity();
		pickup.SetRarity(rarity);
		
		WeaponInstance weaponInstance = new WeaponInstance(pickup.dropData.WeaponDrop, pickup.dropRarity);
		PickupSpawned?.Invoke(pickup);
	}

	public void UnregisterPickup(Pickup pickup)
	{
		spawnedPickups.Remove(pickup);
	}
}