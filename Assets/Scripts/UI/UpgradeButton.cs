using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
	[SerializeField] 
	private TextMeshProUGUI title;
	[SerializeField] 
	private Image icon;
	[SerializeField] 
	private Button button;

	private UpgradeID upgradeID;
	private System.Action<UpgradeID> onClick;

	public void BindButton(UpgradeData data, System.Action<UpgradeID> callback)
	{
		upgradeID = data.upgradeID;
		title.text = data.upgradeName;
		icon.sprite = data.icon;

		onClick = callback;
		button.onClick.RemoveAllListeners();
		button.onClick.AddListener(() => onClick?.Invoke(upgradeID));
	}
}