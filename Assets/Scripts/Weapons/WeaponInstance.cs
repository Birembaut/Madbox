[System.Serializable]
public class WeaponInstance
{
	public WeaponData baseData;
	public Rarity rarity;

	public WeaponInstance(WeaponInstance other)
	{
		baseData = other.baseData;
		rarity = other.rarity;
	}

	public WeaponInstance(WeaponData data, Rarity rarity)
	{
		baseData = data;
		this.rarity = rarity;
	}

	public float GetDamage()
	{
		return baseData.BaseDamage * RarityManager.Instance.Get(rarity).damageMultiplier;
	}
}