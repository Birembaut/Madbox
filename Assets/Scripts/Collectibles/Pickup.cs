using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour, IPoolable
{
	public DropData dropData;
	public Rarity dropRarity;
	[SerializeField]
	private float RotationSpeed;
	[SerializeField]
	private Transform root;
	[SerializeField] 
	private float minAlpha = 0.3f;
	[SerializeField] 
	private float maxAlpha = 0.6f;
	[SerializeField] 
	private float pulseDuration = 1f;
	[SerializeField]
	private CanvasGroup canvasGroup; 
	[SerializeField]
	private Image backgroundImage;
	private Tween pulseTween;

	private void Update()
	{
		root.Rotate(Vector3.up * Time.deltaTime * RotationSpeed);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			SoundManager.Instance.PlayUISelect();
			Despawn();
		}
	}

	public void SetRarity(Rarity rarity)
	{
		dropRarity = rarity;
		Color rarityColor = RarityManager.Instance.Get(rarity).backgroundColor;
		backgroundImage.color = rarityColor;
	}

	public void OnSpawn()
	{
		GameObject fxGO = EntityFactory.Instance.Spawn(FxID.SpawnBurst, transform.position);
		fxGO.GetComponent<ParticleSystem>().Play();

		transform.localScale = Vector3.zero;
		transform.DOScale(Vector3.one * Random.Range(0.9f, 1.1f), 0.5f)
			.SetEase(Ease.OutBack);

		StartPulse();
	}

	public void Despawn()
	{
		CollectibleManager.Instance.UnregisterPickup(this);
		EntityFactory.Instance.Despawn(dropData.EntityID, gameObject);
	}

	public void OnDespawn()
	{
		pulseTween?.Kill();
	}

	private void StartPulse()
	{
		canvasGroup.alpha = minAlpha;

		pulseTween = canvasGroup.DOFade(maxAlpha, pulseDuration)
			.SetEase(Ease.InOutSine)
			.SetLoops(-1, LoopType.Yoyo);
	}
}