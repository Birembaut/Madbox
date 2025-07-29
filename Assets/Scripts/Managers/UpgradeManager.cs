using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
	public delegate void OnUpgradeAcquired(UpgradeData upgrade);
	public OnUpgradeAcquired UpgradeAcquired;

	[SerializeField]
	private UpgradeData[] upgradeDatas;

	private List<UpgradeID> leftUpgrades = new List<UpgradeID>();
	private HashSet<UpgradeID> activeUpgrades = new HashSet<UpgradeID>();
	private Dictionary<UpgradeID, UpgradeData> upgradeData = new Dictionary<UpgradeID, UpgradeData>();

	protected override void Awake()
	{
		base.Awake();
		LoadUpgradeData();
	}

	private void OnEnable()
	{
		GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
	}

	protected void HandleGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.GameOver:
				{
					ResetAll();
					break;
				}
			default:
				break;
		}
	}

	private void LoadUpgradeData()
	{
		for (int upgradeIndex = 0; upgradeIndex < upgradeDatas.Length; upgradeIndex++)
		{
			UpgradeData data = upgradeDatas[upgradeIndex];
			leftUpgrades.Add(data.upgradeID);
			if (!upgradeData.ContainsKey(data.upgradeID))
			{
				upgradeData[data.upgradeID] = data;
			}
		}
	}

	public void AddUpgrade(UpgradeID id)
	{
		UpgradeAcquired.Invoke(GetData(id));
		activeUpgrades.Add(id);
		leftUpgrades.Remove(id);
	}

	public bool HasUpgrade(UpgradeID id)
	{
		return activeUpgrades.Contains(id);
	}

	public UpgradeData GetData(UpgradeID id)
	{
		upgradeData.TryGetValue(id, out var data);
		return data;
	}

	public void ResetAll()
	{
		activeUpgrades.Clear();
		leftUpgrades.Clear();

		for (int upgradeIndex = 0; upgradeIndex < upgradeDatas.Length; upgradeIndex++)
		{
			UpgradeData data = upgradeDatas[upgradeIndex];
			leftUpgrades.Add(data.upgradeID);
		}
	}

	public List<UpgradeData> Get2RandomUpgrades()
	{
		List<UpgradeData> result = new List<UpgradeData>();
		if(leftUpgrades.Count == 0)
		{
			Debug.LogError("not enough upgrade left");
			result.Add(upgradeData[UpgradeID.MaxHealthUp]);
			result.Add(upgradeData[UpgradeID.MaxHealthUp]);
			return result;
		}

		if(leftUpgrades.Count == 1)
		{
			result.Add(upgradeData[leftUpgrades[0]]);
			result.Add(upgradeData[leftUpgrades[0]]);
			return result;
		}

		int first = Random.Range(0, leftUpgrades.Count);
		int second = Random.Range(0, leftUpgrades.Count - 1);
		if (second >= first)
		{
			second += 1;
		}

		result.Add(upgradeData[leftUpgrades[first]]);
		result.Add(upgradeData[leftUpgrades[second]]);

		return result;
	}
}