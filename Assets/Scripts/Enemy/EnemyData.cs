using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject, IEntityData<EnemyID>
{
	public EnemyID enemyId;
	public GameObject prefab;
	public int preloadCount = 5;

	public float Damage;
	public float MaxHealth;
	public WeaponInstance WeaponIfNecessary;

	public int PreloadCount => preloadCount;
	public GameObject Prefab => prefab;
	public EnemyID EntityID => enemyId;
}