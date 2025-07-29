using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class WeaponButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI text;
	[SerializeField]
	private Image image;
	public Button button;
	[SerializeField]
	private Image buttonImage;

	public void BindButton(WeaponInstance weaponInstance)
	{
		RarityData rarityData = RarityManager.Instance.Get(weaponInstance.rarity);
		text.text = weaponInstance.baseData.WeaponName;
		text.color = rarityData.textColor;
		buttonImage.sprite = rarityData.backgroundSprite;
		image.sprite = weaponInstance.baseData.Sprite;
	}
}