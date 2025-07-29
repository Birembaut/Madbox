using System.Collections.Generic;
using UnityEngine;

public class EntityPool
{
	private GameObject prefab;
	private Transform parent;
	private Stack<GameObject> pool;

	public EntityPool(GameObject prefab, int preloadCount, Transform parent = null)
	{
		this.prefab = prefab;
		this.parent = parent;
		pool = new Stack<GameObject>(preloadCount);

		for (int i = 0; i < preloadCount; i++)
		{
			GameObject entity = Object.Instantiate(prefab, parent);
			entity.name = prefab.name + $"({EntityFactory.EntityIndex})";
			EntityFactory.EntityIndex++;
			entity.SetActive(false);
			pool.Push(entity);
		}
	}

	public GameObject Get(Vector3 position)
	{
		GameObject entity = null;
		if (pool.Count > 0)
		{
			entity = pool.Pop();
		}
		else
		{
			entity = Object.Instantiate(prefab, parent);
			entity.name = prefab.name + $"({EntityFactory.EntityIndex})";
			EntityFactory.EntityIndex++;
		}
		
		entity.transform.position = position;
		entity.SetActive(true);

		if (entity.TryGetComponent<IPoolable>(out IPoolable poolable))
		{
			poolable.OnSpawn();
		}

		return entity;
	}

	public void Return(GameObject entity)
	{
		if (entity.TryGetComponent<IPoolable>(out IPoolable poolable))
		{
			poolable.OnDespawn();
		}

		entity.transform.SetParent(parent);
		entity.SetActive(false);
		pool.Push(entity);
	}
}