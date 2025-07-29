using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Waves/WaveSetData")]
public class WaveSetData : ScriptableObject
{
	public List<WaveData> Waves;
}