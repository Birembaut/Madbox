using DG.Tweening;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
	public enum UIDirection
	{
		Top,
		Bot,
		Left,
		Right,
	}

	[SerializeField]
	private RectTransform menuUI;

	[SerializeField]
	private WeaponSelectorUI inventoryUI;

	[SerializeField]
	private RectTransform hudUI;

	[SerializeField]
	private GameOverUI gameOverUI;

	[SerializeField]
	private UpgradeSelectorUI upgradeUI;

	[SerializeField]
	private ScreenDamageFlash damageFlash;
	[SerializeField]
	private float slideInDuration = 0.3f;
	[SerializeField]
	private float slideOutDuration = 0.5f;
	[SerializeField]
	private Ease easeIn;
	[SerializeField]
	private Ease easeOut;

	private GameState previousState = GameState.None;

	private void Start()
	{
		GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
		WaveManager.Instance.WaveEnded += OnWaveEnded;
		upgradeUI.gameObject.SetActive(false);
		HandleGameStateChanged(GameState.Menu);

		Debug.Log($"screen height : {Screen.height}, screen wigth : {Screen.width}");
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
		WaveManager.Instance.WaveEnded -= OnWaveEnded;
	}

	private void HandleGameStateChanged(GameState state)
	{
		if(previousState == state)
		{
			return;
		}

		switch(previousState)
		{
			case GameState.Inventory:
				{
					HideUI(inventoryUI.GetComponent<RectTransform>(), UIDirection.Bot, slideInDuration);
					break;
				}
			case GameState.GameOver:
				{
					HideUI(gameOverUI.GetComponent<RectTransform>(), UIDirection.Top, slideInDuration);
					break;
				}
			case GameState.Playing:
				{
					hudUI.gameObject.SetActive(false);
					break;
				}
			case GameState.Menu:
				{
					HideUI(menuUI, UIDirection.Top, slideInDuration);
					break;
				}
			default:
				break;
		}

		//menuUI.SetActive(state == GameState.Menu);
		//inventoryUI.gameObject.SetActive(state == GameState.Inventory);
		//hudUI.SetActive(state == GameState.Playing);
		//gameOverUI.gameObject.SetActive(state == GameState.GameOver);

		switch(state)
		{
			case GameState.Inventory:
				{
					inventoryUI.RefreshWeaponInventory();
					ShowUI(inventoryUI.GetComponent<RectTransform>(), UIDirection.Bot, slideOutDuration);
					break;
				}
			case GameState.GameOver:
				{
					gameOverUI.FeedGameOver();
					ShowUI(gameOverUI.GetComponent<RectTransform>(), UIDirection.Bot, slideOutDuration);
					break;
				}
			case GameState.Playing:
				{
					hudUI.gameObject.SetActive(true);
					break;
				}
			case GameState.Menu:
				{
					ShowUI(menuUI, UIDirection.Top, slideOutDuration);
					break;
				}
			default:
				break;
		}

		previousState = state;
	}

	private void OnWaveEnded()
	{
		upgradeUI.OnWaveEnded();
		ShowUI(upgradeUI.GetComponent<RectTransform>(), UIDirection.Top, slideOutDuration);
	}

	public void OnPlayPressed()
	{
		GameManager.Instance.StartGame();
	}

	public void OnReturnToMenuPressed()
	{
		GameManager.Instance.ReturnToMenu();
	}

	public void OnInventoryPressed()
	{
		GameManager.Instance.OpenInventory();
	}

	public void OnLeavePressed()
	{
		GameManager.Instance.TriggerGameOver(false);
	}

	public void AddDamageFlash()
	{
		damageFlash.PlayFlash();
	}

	public void ShowUI(RectTransform panel, UIDirection direction, float duration)
	{
		switch(direction)
		{
			case UIDirection.Left:
				{
					panel.gameObject.SetActive(true);
					panel.anchoredPosition = new Vector2(-Screen.width, 0);
					panel.DOAnchorPos(Vector2.zero, duration).SetEase(easeIn);
					break;
				}
			case UIDirection.Right:
				{
					panel.gameObject.SetActive(true);
					panel.anchoredPosition = new Vector2(Screen.width, 0);
					panel.DOAnchorPos(Vector2.zero, duration).SetEase(easeIn);
					break;
				}
			case UIDirection.Top:
				{
					panel.gameObject.SetActive(true);
					panel.anchoredPosition = new Vector2(0, Screen.height);
					panel.DOAnchorPos(Vector2.zero, duration).SetEase(easeIn);
					break;
				}
			case UIDirection.Bot:
				{
					panel.gameObject.SetActive(true);
					panel.anchoredPosition = new Vector2(0, -Screen.height);
					panel.DOAnchorPos(Vector2.zero, duration).SetEase(easeIn);
					break;
				}
		}
	}

	public void HideUI(RectTransform panel, UIDirection direction, float duration)
	{
		switch (direction)
		{
			case UIDirection.Left:
				{
					panel.DOAnchorPos(new Vector2(-Screen.width, 0), duration)
					  .SetEase(easeOut)
					  .OnComplete(() => panel.gameObject.SetActive(false));
					break;
				}
			case UIDirection.Right:
				{
					panel.DOAnchorPos(new Vector2(Screen.width, 0), duration)
					  .SetEase(easeOut)
					  .OnComplete(() => panel.gameObject.SetActive(false));
					break;
				}
			case UIDirection.Top:
				{
					panel.DOAnchorPos(new Vector2(0, Screen.height), duration)
					  .SetEase(easeOut)
					  .OnComplete(() => panel.gameObject.SetActive(false));
					break;
				}
			case UIDirection.Bot:
				{
					panel.DOAnchorPos(new Vector2(0, -Screen.height), duration)
					  .SetEase(easeOut)
					  .OnComplete(() => panel.gameObject.SetActive(false));
					break;
				}
		}
	}
}