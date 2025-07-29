using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
	[SerializeField] 
	private Image instantFill;
	[SerializeField] 
	private Image delayedFill;
	[SerializeField] 
	private float delayDuration = 0.4f;

	private float targetFill = 1f;

	private float delayedStartFill;
	private float delayTimer;
	private bool isDelaying;

	public void SetHealth(float normalized)
	{
		normalized = Mathf.Clamp01(normalized);
		targetFill = normalized;
		instantFill.fillAmount = targetFill;

		if (delayedFill.fillAmount > targetFill)
		{
			delayedStartFill = delayedFill.fillAmount;
			delayTimer = 0f;
			isDelaying = true;
		}
		else
		{
			delayedFill.fillAmount = targetFill;
			isDelaying = false;
		}
	}

	private void Update()
	{
		if (isDelaying)
		{
			delayTimer += Time.deltaTime;
			float timeLeftRatio = Mathf.Clamp01(delayTimer / delayDuration);
			delayedFill.fillAmount = Mathf.Lerp(delayedStartFill, targetFill, timeLeftRatio);

			if (timeLeftRatio >= 1f)
			{
				isDelaying = false;
			}
		}

		transform.forward = Camera.main.transform.forward;
	}
}