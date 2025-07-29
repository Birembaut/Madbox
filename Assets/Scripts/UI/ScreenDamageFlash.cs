using UnityEngine;

public class ScreenDamageFlash : MonoBehaviour
{
	[SerializeField] 
	private CanvasGroup canvasGroup;
	[SerializeField] 
	private float flashAlpha = 0.4f;
	[SerializeField] 
	private float flashDuration = 0.2f;

	private float currentAlpha = 0f;
	private float flashTimer = 0f;
	private bool isFlashing = false;

	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void PlayFlash()
	{
		currentAlpha = flashAlpha;
		canvasGroup.alpha = currentAlpha;
		flashTimer = 0f;
		isFlashing = true;
	}

	private void Update()
	{
		if (!isFlashing)
		{
			return;
		}

		flashTimer += Time.deltaTime;
		float currentTimerRatio = flashTimer / flashDuration;
		currentAlpha = Mathf.Lerp(flashAlpha, 0f, currentTimerRatio);
		canvasGroup.alpha = currentAlpha;

		if (currentTimerRatio >= 1f)
		{
			isFlashing = false;
			canvasGroup.alpha = 0f;
		}
	}
}