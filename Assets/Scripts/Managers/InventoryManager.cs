using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
	public List<WeaponInstance> AvailableWeapons = new List<WeaponInstance>();
	public WeaponInstance CurrentWeapon = null;

	public event Action<WeaponInstance> OnWeaponChanged;

	private void OnEnable()
	{
		GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
		CollectibleManager.Instance.PickupSpawned += OnPickupSpawned;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
		CollectibleManager.Instance.PickupSpawned -= OnPickupSpawned;
	}

	private void HandleGameStateChanged(GameState state)
	{
		if (state != GameState.Playing)
		{
			return;
		}

		EntityFactory.Instance.RegisterData(CurrentWeapon.baseData);
	}

	private void OnPickupSpawned(Pickup pickup)
	{
		WeaponInstance weaponInstance = new WeaponInstance(pickup.dropData.WeaponDrop, pickup.dropRarity);
		UnlockWeapon(weaponInstance);
	}

	public void Equip(WeaponInstance newWeapon)
	{
		if (newWeapon == null || newWeapon == CurrentWeapon)
		{
			return;
		}

		CurrentWeapon = newWeapon;
		OnWeaponChanged?.Invoke(CurrentWeapon);
	}

	public static bool CanFuse(WeaponInstance left, WeaponInstance right)
	{
		if (left == null || right == null)
		{
			return false;
		}
		if (left.baseData != right.baseData)
		{
			return false;
		}
		if (left.rarity != right.rarity)
		{
			return false;
		}
		if (left.rarity >= Rarity.Legendary)
		{
			return false;
		}

		return true;
	}

	public WeaponInstance Fuse(WeaponInstance left, WeaponInstance right)
	{
		Rarity newRarity = left.rarity + 1;

		return new WeaponInstance(left.baseData, newRarity);
	}

	public bool AutoFuseAll()
	{
		int fusionPerformedCount = 0;
		bool fusionPerformed = true;
		bool hasChangedWeapon = false;
		WeaponInstance equipedWeapon = CurrentWeapon;

		while (fusionPerformed)
		{
			fusionPerformed = false;

			for (int firstWeaponIndex = 0; firstWeaponIndex < AvailableWeapons.Count; firstWeaponIndex++)
			{
				for (int secondWeaponIndex = firstWeaponIndex + 1; secondWeaponIndex < AvailableWeapons.Count; secondWeaponIndex++)
				{
					WeaponInstance left = AvailableWeapons[firstWeaponIndex];
					WeaponInstance right = AvailableWeapons[secondWeaponIndex];


					if (CanFuse(left, right))
					{
						Debug.Log($"fuse left {left.baseData.WeaponName}/{left.rarity.ToString()} with right {right.baseData.WeaponName}/{right.rarity.ToString()}");
						WeaponInstance fusedWeapon = Fuse(left, right);

						fusionPerformedCount++;
						if (left == equipedWeapon || right == equipedWeapon)
						{
							Debug.Log($"Equiped fused weapon {fusedWeapon.baseData.WeaponName}/{fusedWeapon.rarity.ToString()}");
							equipedWeapon = fusedWeapon;
							hasChangedWeapon = true;
						}

						AvailableWeapons.RemoveAt(secondWeaponIndex);
						AvailableWeapons.RemoveAt(firstWeaponIndex);
						AvailableWeapons.Add(fusedWeapon);

						fusionPerformed = true;
						break;
					}
				}

				if (fusionPerformed)
					break; 
			}
		}

		if(hasChangedWeapon)
		{
			Equip(equipedWeapon);
		}

		return fusionPerformedCount > 0;
	}

	public void UnlockWeapon(WeaponInstance unlockedWeapon)
	{
		AvailableWeapons.Add(unlockedWeapon);
		if (CurrentWeapon == null || CurrentWeapon.baseData == null)
		{
			Equip(unlockedWeapon);
		}
	}
}