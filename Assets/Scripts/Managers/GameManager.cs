using System;
using System.Collections;
using UnityEngine;

public enum GameState { Menu, Inventory, Playing, GameOver, None }

public class GameManager : Singleton<GameManager>
{
	public GameState CurrentState { get; private set; }
	public GameSessionData CurrentSession = new GameSessionData();

	public event Action<GameState> OnGameStateChanged;
	public GameObject Player;

	public WeaponInstance weaponStarterPack;

	public FXData[] FxToPreload;
	public UIItemData[] UiItemToLoad;

	protected override void Awake()
	{
		base.Awake();
		ChangeState(GameState.Menu);
	}

	private IEnumerator Start()
	{
		yield return null;

		int fxLength = FxToPreload?.Length ?? 0;
		for (int fxIndex = 0; fxIndex < fxLength; fxIndex++)
		{
			EntityFactory.Instance.RegisterData(FxToPreload[fxIndex]);
		}

		int uiItemLength = UiItemToLoad?.Length ?? 0;
		for (int itemIndex = 0; itemIndex < uiItemLength; itemIndex++)
		{
			EntityFactory.Instance.RegisterData(UiItemToLoad[itemIndex]);
		}

		// Wait for everything initialization before initializing Player
		InventoryManager.Instance.UnlockWeapon(weaponStarterPack);
		WaveManager.Instance.EnemyDied += OnEnemyDied;
		CollectibleManager.Instance.PickupSpawned += OnPickupSpawned;
	}

	private void OnDestroy()
	{
		WaveManager.Instance.EnemyDied -= OnEnemyDied;
		CollectibleManager.Instance.PickupSpawned += OnPickupSpawned;
	}

	private void OnPickupSpawned(Pickup pickup)
	{
		WeaponInstance weaponInstance = new WeaponInstance(pickup.dropData.WeaponDrop, pickup.dropRarity);
		CurrentSession.CollectedPickups.Add(weaponInstance);
	}

	private void OnEnemyDied(Enemy enemy)
	{
		CurrentSession.EnemiesKilled++;
	}

	public void ChangeState(GameState newState)
	{
		if (newState == CurrentState)
			return;

		CurrentState = newState;
		Debug.Log($"[GameManager] State changed to: {newState}");
		OnGameStateChanged?.Invoke(newState);
	}

	public void StartGame()
	{
		ResetGameSession();
		SoundManager.Instance.PlayPlaySelect();
		ChangeState(GameState.Playing);
	}

	public void OpenInventory() => ChangeState(GameState.Inventory);
	public void TriggerGameOver(bool isWin)
	{
		Player.transform.position = Vector3.zero;
		Player.transform.rotation = Quaternion.identity;

		CurrentSession.IsWin = isWin;
		CurrentSession.PlayTime = Time.time - CurrentSession.PlayTime;
		ChangeState(GameState.GameOver);
	}

	public void ReturnToMenu() => ChangeState(GameState.Menu);

	private void ResetGameSession()
	{
		CurrentSession.EnemiesKilled = 0;
		CurrentSession.WavesReach = 0;
		CurrentSession.PlayTime = Time.time;
		CurrentSession.IsWin = false;
		CurrentSession.CollectedPickups.Clear();
	}
}