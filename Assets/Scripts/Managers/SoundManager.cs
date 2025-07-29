using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	[SerializeField]
	private AudioClip projectileShot; 
	[SerializeField]
	private AudioClip hitImpact; 
	[SerializeField]
	private AudioClip enemyDeath; 
	[SerializeField]
	private AudioClip uiSelect;
	[SerializeField]
	private AudioClip playSelect;
	[SerializeField]
	private AudioSource sfxSource;

	public void PlaySound(AudioClip clip, float pitchVariation = 0.1f)
	{
		if (clip == null)
		{
			return;
		}

		float basePitch = sfxSource.pitch;
		sfxSource.pitch = basePitch + Random.Range(-pitchVariation, pitchVariation);
		sfxSource.PlayOneShot(clip);
		sfxSource.pitch = basePitch;
	}

	public void PlayImpact() => PlaySound(hitImpact);
	public void PlayShoot() => PlaySound(projectileShot, 0.2f);
	public void PlayEnemyDeath() => PlaySound(enemyDeath);
	public void PlayUISelect() => PlaySound(uiSelect);
	public void PlayPlaySelect() => PlaySound(playSelect, 0f);
}