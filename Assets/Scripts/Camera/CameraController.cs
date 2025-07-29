using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
	[SerializeField] 
	private CinemachineVirtualCamera gameplayCam;
	[SerializeField] 
	private CinemachineVirtualCamera menuCam;
	[SerializeField] 
	private CinemachineVirtualCamera inventoryCam; 
	[SerializeField] 
	private CinemachineImpulseSource impulseSource;

	public void PlayFeedback()
	{
		impulseSource.GenerateImpulse();
	}

	private void Start()
	{
		GameManager.Instance.OnGameStateChanged += HandleStateChange;
		HandleStateChange(GameManager.Instance.CurrentState);
	}

	private void OnDestroy()
	{
		GameManager.Instance.OnGameStateChanged -= HandleStateChange;
	}

	private void HandleStateChange(GameState state)
	{
		switch (state)
		{
			case GameState.Menu:
				menuCam.Priority = 10;
				gameplayCam.Priority = 5;
				inventoryCam.Priority = 5;
				break;
			case GameState.Inventory:
				menuCam.Priority = 5;
				gameplayCam.Priority = 5;
				inventoryCam.Priority = 10;
				break;
			case GameState.Playing:
				menuCam.Priority = 5;
				gameplayCam.Priority = 10;
				inventoryCam.Priority = 5;
				break;
		}
	}
}