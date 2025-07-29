using UnityEngine;

public class InputManager : Singleton<InputManager>
{
	public delegate void OnPlayerMoved(Vector2 movement);
	public OnPlayerMoved PlayerMoved;
	private Vector2 movement;

	public void OnInputMoved(Vector2 input)
	{
		if (input != movement)
		{
			movement = input;
			PlayerMoved?.Invoke(movement);
		}
	}
}