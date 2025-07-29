using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject, IEntityData<WeaponID>
{
	public WeaponID weaponId;
	public GameObject prefab;
	public int preloadCount = 5;

	public string WeaponName;
	public float AnimationAttackSpeedMultiplier;
	public float AnimationMoveSpeedMultiplier;
	public float BaseMoveSpeed;
	public float BaseDamage;
	public int maxBounces;
	public LayerMask hittableLayers;

	public GameObject WeaponVisualPrefab;
	public Sprite Sprite;

	public int PreloadCount => preloadCount;
	public GameObject Prefab => prefab;
	public WeaponID EntityID => weaponId;
}