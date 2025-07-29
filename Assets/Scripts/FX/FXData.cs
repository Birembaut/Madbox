using UnityEngine;

[CreateAssetMenu(menuName = "FX/FXData")]
public class FXData : ScriptableObject, IEntityData<FxID>
{
	public FxID fxID;
	public GameObject prefab;
	public int preloadCount = 5;

	public int PreloadCount => preloadCount;
	public GameObject Prefab => prefab;
	public FxID EntityID => fxID;
}