using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	public RectTransform backgroundJoystick;
	public RectTransform innerStick;

	public float HandleLimit = 1f;

	public Vector2 input = Vector2.zero;

	private Vector2 joyPosition = Vector2.zero;
	private Vector2 initialPosition;

	private void Start()
	{
		initialPosition = backgroundJoystick.position;
		GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
	}

	private void OnDestroy()
	{
		GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
	}

	protected void HandleGameStateChanged(GameState state)
	{
		if(state != GameState.GameOver)
		{
			return;
		}

		input = Vector2.zero;
		innerStick.anchoredPosition = Vector2.zero;
		backgroundJoystick.position = initialPosition;
		InputManager.Instance.OnInputMoved(Vector2.zero);
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 joyDirection = eventData.position - joyPosition;
		input = (joyDirection.magnitude > backgroundJoystick.sizeDelta.x / 2f) ? joyDirection.normalized : joyDirection / (backgroundJoystick.sizeDelta.x / 2f);
		
		innerStick.anchoredPosition = (input * backgroundJoystick.sizeDelta.x / 2f) * HandleLimit;

		InputManager.Instance.OnInputMoved(input);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		input = Vector2.zero;
		innerStick.anchoredPosition = Vector2.zero;
		backgroundJoystick.position = initialPosition;

		InputManager.Instance.OnInputMoved(input);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnDrag(eventData);
		joyPosition = eventData.position;
		backgroundJoystick.position = eventData.position;
		innerStick.anchoredPosition = Vector2.zero;
	}
}