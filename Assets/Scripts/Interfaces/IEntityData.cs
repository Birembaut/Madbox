using UnityEngine;

public interface IEntityData<ID>
{
	ID EntityID { get; }
	GameObject Prefab { get; }
	int PreloadCount { get; }
}