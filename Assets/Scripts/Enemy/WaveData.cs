using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Waves/WaveData")]
public class WaveData : ScriptableObject
{
	public List<EnemySpawnData> Enemies;
	public float DelayBeforeNextWave = 2f;
}