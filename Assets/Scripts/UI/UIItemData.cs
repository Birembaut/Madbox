using UnityEngine;

[CreateAssetMenu(menuName = "UI/UIItemData")]
public class UIItemData : ScriptableObject, IEntityData<UIItemID>
{
	public UIItemID uiItemID;
	public GameObject prefab;
	public int preloadCount = 5;

	public int PreloadCount => preloadCount;
	public GameObject Prefab => prefab;
	public UIItemID EntityID => uiItemID;
}