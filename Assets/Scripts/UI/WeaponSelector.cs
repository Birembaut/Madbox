using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectorUI : MonoBehaviour
{
	[SerializeField] 
	private Transform container;
	[SerializeField] 
	private ScrollRect scrollRect;

	[SerializeField]
	private TextMeshProUGUI currentWeaponText;
	[SerializeField]
	private Image currentWeaponImage;
	[SerializeField]
	private Image currentWeaponBackgroundImage;

	[SerializeField]
	private GameObject cheatPanel;
	[SerializeField]
	private Transform cheatContainer;


	private void Start()
	{
		InventoryManager.Instance.OnWeaponChanged += OnWeaponChanged;
	}

	private void OnDestroy()
	{
		InventoryManager.Instance.OnWeaponChanged -= OnWeaponChanged;
	}

	private void OnWeaponChanged(WeaponInstance weaponData)
	{
		RarityData rarityData = RarityManager.Instance.Get(weaponData.rarity);
		currentWeaponBackgroundImage.sprite = rarityData.backgroundSprite;
		currentWeaponText.text = weaponData.baseData.WeaponName;
		currentWeaponText.color = rarityData.textColor;
		currentWeaponImage.sprite = weaponData.baseData.Sprite;
	}

	public void FuzeWeapons()
	{
		if(InventoryManager.Instance.AutoFuseAll())
		{
			RefreshWeaponInventory();
		}
	}

	public void RefreshWeaponInventory()
	{
		InventoryManager inventoryManager = InventoryManager.Instance;

		OnWeaponChanged(inventoryManager.CurrentWeapon);
		for(int childIndex = container.childCount - 1; childIndex >= 0; childIndex--)
		{
			Transform child = container.GetChild(childIndex);
			EntityFactory.Instance.Despawn(UIItemID.EquipmentButton, child.gameObject);
		}

        for (int weaponIndex = 0; weaponIndex < inventoryManager.AvailableWeapons.Count; weaponIndex++)
		{
			WeaponInstance weapon = inventoryManager.AvailableWeapons[weaponIndex];
			WeaponButton weaponButton = EntityFactory.Instance.Spawn(UIItemID.EquipmentButton, transform.position).GetComponent<WeaponButton>();
			weaponButton.transform.SetParent(container.transform, false);
			weaponButton.transform.localScale = Vector3.one;
			weaponButton.BindButton(weapon);

			weaponButton.button.onClick.RemoveAllListeners();
			weaponButton.button.onClick.AddListener(() =>
			{
				InventoryManager.Instance.Equip(weapon);
				Debug.Log("Selected: " + weapon.baseData.WeaponName);
			});
		}

		scrollRect.verticalNormalizedPosition = 1f;
	}

	public void OpenCheatMenu()
	{
		cheatPanel.SetActive(true);
		for (int weaponIndex = 0; weaponIndex < CollectibleManager.Instance.AvailableDrops.Length; weaponIndex++)
		{
			DropData drop = CollectibleManager.Instance.AvailableDrops[weaponIndex];
			if(drop.WeaponDrop == null)
			{
				continue;
			}

			for(int rarityIndex = 0; rarityIndex < 5; rarityIndex++)
			{
				WeaponButton weaponButton = EntityFactory.Instance.Spawn(UIItemID.EquipmentButton, transform.position).GetComponent<WeaponButton>();
				weaponButton.transform.SetParent(cheatContainer.transform, false);
				weaponButton.transform.localScale = Vector3.one;
				WeaponInstance currentWeaponInstance = new WeaponInstance(drop.WeaponDrop, (Rarity)rarityIndex);

				weaponButton.BindButton(currentWeaponInstance);
				weaponButton.button.onClick.AddListener(() =>
				{
					InventoryManager.Instance.UnlockWeapon(new WeaponInstance(currentWeaponInstance));
					Debug.Log("Unlock: " + currentWeaponInstance.baseData.WeaponName);
				});
			}
		}
	}

	public void CloseCheatMenu()
	{
		while (cheatContainer.childCount > 0)
		{
			Transform child = cheatContainer.GetChild(0);
			EntityFactory.Instance.Despawn(UIItemID.EquipmentButton, child.gameObject);
		}
		cheatPanel.SetActive(false);

		RefreshWeaponInventory();
	}
}