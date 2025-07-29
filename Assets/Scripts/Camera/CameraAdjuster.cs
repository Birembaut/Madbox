using UnityEngine;
using Cinemachine;

public class CameraAdjuster : MonoBehaviour
{
	[SerializeField] 
	private CinemachineVirtualCamera virtualCamera;
	[SerializeField] 
	private float referenceAspect = 16f / 9f;
	[SerializeField] 
	private float referenceOrthoSize = 15f;

	private void Start()
	{
		virtualCamera = GetComponent<CinemachineVirtualCamera>();
		AdjustCamera();
	}

	private void AdjustCamera()
	{
		float currentAspect = (float)Screen.height / Screen.width;
		float scale = referenceAspect / currentAspect;

		virtualCamera.m_Lens.OrthographicSize = referenceOrthoSize * (1 + 1 - scale);
	}
}