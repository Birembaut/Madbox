using UnityEngine;

[CreateAssetMenu(menuName = "Pickup/DropData")]
public class DropData : ScriptableObject, IEntityData<DropID>
{
	public DropID dropID;
	public GameObject prefab;
	public int preloadCount = 5;

	public WeaponData WeaponDrop;

	public int PreloadCount => preloadCount;
	public GameObject Prefab => prefab;
	public DropID EntityID => dropID;
}