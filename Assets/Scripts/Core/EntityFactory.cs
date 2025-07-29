using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityFactory : Singleton<EntityFactory>
{
	private readonly Dictionary<Type, Dictionary<object, EntityPool>> pools = new();
	private readonly Dictionary<Type, Dictionary<object, object>> data = new();
	public static int EntityIndex = 0;

	public void RegisterData<ID>(IEntityData<ID> data)
	{
		var type = typeof(ID);

		if (!pools.ContainsKey(type))
		{
			pools[type] = new Dictionary<object, EntityPool>();
		}

		if (!this.data.ContainsKey(type))
		{
			this.data[type] = new Dictionary<object, object>();
		}

		if(pools[type].ContainsKey(data.EntityID))
		{
			return;
		}

		pools[type][data.EntityID] = new EntityPool(data.Prefab, data.PreloadCount, transform);
		this.data[type][data.EntityID] = data;
	}

	public GameObject Spawn<ID>(ID id, Vector3 position)
	{
		var type = typeof(ID);
		if (pools.TryGetValue(type, out var dict) && dict.TryGetValue(id, out var pool))
		{
			return pool.Get(position);
		}

		Debug.LogError($"[EntityFactory] No pool for {id}");
		return null;
	}

	public void Despawn<ID>(ID id, GameObject entity)
	{
		var type = typeof(ID);
		if (pools.TryGetValue(type, out var dict) && dict.TryGetValue(id, out var pool))
		{
			pool.Return(entity);
		}
		else
		{
			GameObject.Destroy(entity);
		}
	}

	public IEntityData<ID> GetData<ID>(ID id)
	{
		var type = typeof(ID);
		if (data.TryGetValue(type, out var dict) && dict.TryGetValue(id, out var found))
		{
			return (IEntityData<ID>)found;
		}

		Debug.LogError($"[EntityFactory] No data found for {id}");
		return default;
	}
}