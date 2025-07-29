using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI titleText;
	[SerializeField] 
	private TextMeshProUGUI wavesText;
	[SerializeField] 
	private TextMeshProUGUI killsText;
	[SerializeField] 
	private TextMeshProUGUI timeText;
	[SerializeField]
	private Transform weaponLootedContainer;

	public void FeedGameOver()
	{
		GameSessionData session = GameManager.Instance.CurrentSession;
		titleText.text = session.IsWin ? "CONGRATULATION" : "OOPS";
		wavesText.text = $"Waves Reach: {session.WavesReach}";
		killsText.text = $"Kills: {session.EnemiesKilled}";
		timeText.text = $"Time: {session.PlayTime:F1}s";

		for (int childIndex = weaponLootedContainer.childCount - 1; childIndex >= 0; childIndex--)
		{
			Transform child = weaponLootedContainer.GetChild(childIndex);
			EntityFactory.Instance.Despawn(UIItemID.EquipmentButton, child.gameObject);
		}

		for (int weaponIndex = 0; weaponIndex < session.CollectedPickups.Count; weaponIndex++)
		{
			WeaponInstance weapon = session.CollectedPickups[weaponIndex];
			WeaponLooted weaponButton = EntityFactory.Instance.Spawn(UIItemID.WeaponLooted, transform.position).GetComponent<WeaponLooted>();
			weaponButton.transform.SetParent(weaponLootedContainer.transform, false);
			weaponButton.transform.localScale = Vector3.one;
			weaponButton.Bind(weapon);
		}
	}
}