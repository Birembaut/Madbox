using System.Collections.Generic;
using UnityEngine;

public class RarityManager : Singleton<RarityManager>
{
	[SerializeField]
	private RarityData[] rarityDatas;
	private int totalWeight;
	private Dictionary<Rarity, RarityData> dataByRarity = new Dictionary<Rarity, RarityData>();

	protected override void Awake()
	{
		base.Awake();

		int rarityLength = rarityDatas.Length;
		for (int rarityIndex = 0; rarityIndex < rarityLength; rarityIndex++)
		{
			RarityData rarityData = rarityDatas[rarityIndex];
			totalWeight += rarityData.DropWeight;

			if (!dataByRarity.ContainsKey(rarityData.Rarity))
			{
				dataByRarity.Add(rarityData.Rarity, rarityData);
			}
		}
	}

	public RarityData Get(Rarity rarity)
	{
		if (dataByRarity.ContainsKey(rarity))
		{
			return dataByRarity[rarity];
		}

		Debug.LogError($"RarityData not found for: {rarity}");
		return null;
	}

	public Rarity GetRandomRarity()
	{
		int roll = Random.Range(0, totalWeight);
		int current = 0;

		int rarityLength = rarityDatas.Length;
		for (int rarityIndex = 0; rarityIndex < rarityLength; rarityIndex++)
		{
			RarityData rarityData = rarityDatas[rarityIndex];
			current += rarityData.DropWeight;
			if (roll < current)
			{
				return rarityData.Rarity;
			}
		}

		return Rarity.Common;
	}
}