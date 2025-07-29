using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingText : MonoBehaviour, IPoolable
{
	[SerializeField]
	private UIItemData data;
	[SerializeField] 
	private TextMeshProUGUI text;
	[SerializeField] 
	private CanvasGroup canvasGroup;
	[SerializeField]
	private float duration = 1f;
	[SerializeField] 
	private float moveY = 1.5f;
	[SerializeField] 
	private float fadeDuration = 0.3f;

	private Sequence sequence;
	private void LateUpdate()
	{
		transform.forward = Camera.main.transform.forward;
	}

	public void Show(string value, Color color)
	{
		text.text = value;
		text.color = color;
		canvasGroup.alpha = 1f;

		sequence?.Kill();

		sequence = DOTween.Sequence()
			.Append(transform.DOMoveY(transform.position.y + moveY, duration).SetEase(Ease.OutCubic))
			.Join(canvasGroup.DOFade(0, fadeDuration).SetDelay(duration - fadeDuration))
			.OnComplete(() =>
			{
				EntityFactory.Instance.Despawn(data.EntityID ,gameObject);
			});
	}

	public void OnSpawn()
	{
		canvasGroup.alpha = 1f;
		transform.localScale = Vector3.one;
	}

	public void OnDespawn()
	{
		sequence?.Kill();
	}
}