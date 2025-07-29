using UnityEngine;

[CreateAssetMenu(menuName = "Data/RarityData")]
public class RarityData : ScriptableObject
{
	public Rarity Rarity;
	public int DropWeight;

	[Header("Visual")]
	public Sprite backgroundSprite;
	public Color backgroundColor;
	public Color textColor;

	[Header("Gameplay")]
	public float damageMultiplier = 1f;
}