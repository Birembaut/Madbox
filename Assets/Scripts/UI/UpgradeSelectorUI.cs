using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelectorUI : MonoBehaviour
{
	[SerializeField] 
	private Transform buttonContainer;

	public void OnWaveEnded()
	{
		gameObject.SetActive(true);
		List<UpgradeData> randomUpgrades = UpgradeManager.Instance.Get2RandomUpgrades();
		for (int upgradeIndex = 0; upgradeIndex < randomUpgrades.Count; upgradeIndex++)
		{
			UpgradeData upgrade = randomUpgrades[upgradeIndex];
			UpgradeButton button = EntityFactory.Instance.Spawn(UIItemID.UpgradeButton, transform.position).GetComponent<UpgradeButton>();
			button.transform.SetParent(buttonContainer.transform, false);
			button.transform.localScale = Vector3.one;
			button.BindButton(upgrade, OnUpgradeSelected);
		}
	}

	private void ClearButtons()
	{
		for (int childIndex = buttonContainer.childCount - 1; childIndex >= 0; childIndex--)
		{
			Transform child = buttonContainer.GetChild(childIndex);
			EntityFactory.Instance.Despawn(UIItemID.UpgradeButton, child.gameObject);
		}
	}

	private void OnUpgradeSelected(UpgradeID id)
	{
		Debug.Log($"Upgrade selected : {id.ToString()}");
		ClearButtons();
		UpgradeManager.Instance.AddUpgrade(id);
		WaveManager.Instance.SpawnNextWave();
		gameObject.SetActive(false);
	}
}