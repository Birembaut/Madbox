using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class WeaponLooted : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI text;
	[SerializeField]
	private Image weaponImage;
	[SerializeField]
	private Image background;

	public void Bind(WeaponInstance weaponInstance)
	{
		RarityData rarityData = RarityManager.Instance.Get(weaponInstance.rarity);
		text.text = weaponInstance.baseData.WeaponName;
		text.color = rarityData.textColor;
		background.sprite = rarityData.backgroundSprite;
		weaponImage.sprite = weaponInstance.baseData.Sprite;
	}
}