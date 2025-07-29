using System.Collections.Generic;

public class GameSessionData
{
	public int WavesReach;
	public int EnemiesKilled;
	public float PlayTime;
	public bool IsWin;
	public List<WeaponInstance> CollectedPickups = new List<WeaponInstance>();
}