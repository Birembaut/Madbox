using UnityEngine;

public class CameraZTarget : MonoBehaviour
{
	[SerializeField] 
	private Transform player;

	[SerializeField]
	private float minZ = 10f;
	[SerializeField]
	private float maxZ = 15f;
	[SerializeField]
	private float gapWithPlayer = 10f;

	private void LateUpdate()
	{
		Vector3 pos = transform.position;
		pos.z = Mathf.Clamp(player.position.z + gapWithPlayer, minZ, maxZ);
		transform.position = pos;
	}
}