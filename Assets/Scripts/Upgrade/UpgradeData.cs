using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
	public UpgradeID upgradeID;
	public string upgradeName;
	public Sprite icon;
	public float value;
}