using UnityEngine;

public class FX : MonoBehaviour, IPoolable
{
	[SerializeField]
	private FXData FXData;
	private ParticleSystem ps;

	void Awake()
	{
		ps = GetComponent<ParticleSystem>();
		var main = ps.main;
		main.stopAction = ParticleSystemStopAction.Callback;
	}

	void OnParticleSystemStopped()
	{
		EntityFactory.Instance.Despawn(FXData.EntityID, gameObject);
	}

	public void Play()
	{
		ps.Play();
	}

	public void OnSpawn()
	{
		ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		ps.Play();
	}

	public void OnDespawn()
	{
		ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}
}