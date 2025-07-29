using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private float baseSpeed = 5f;
	[SerializeField]
	private float rotationSpeed = 10f;
	private Vector3 direction;

	private CharacterController controller;
	private Animator animator;
	private PlayerLife playerLife;

	private void Start()
	{
		controller = GetComponent<CharacterController>();
		playerLife = GetComponent<PlayerLife>();
		animator = GetComponentInChildren<Animator>();

		InputManager.Instance.PlayerMoved += OnPlayerMoved;
		InventoryManager.Instance.OnWeaponChanged += OnWeaponChanged;
	}

	private void OnDestroy()
	{
		InputManager.Instance.PlayerMoved -= OnPlayerMoved;
		InventoryManager.Instance.OnWeaponChanged -= OnWeaponChanged;
	}

	private void OnPlayerMoved(Vector2 movement)
	{
		direction = new Vector3(movement.x, 0, movement.y);
		if (direction != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		}
	}

	private void OnWeaponChanged(WeaponInstance newData)
	{
		baseSpeed = newData.baseData.BaseMoveSpeed;
		animator.SetFloat("AnimationWalkSpeed", newData.baseData.AnimationMoveSpeedMultiplier);
	}

	private void Update()
	{
		if (GameManager.Instance.CurrentState != GameState.Playing)
		{
			return;
		}

		if (playerLife.IsDead)
		{
			return;
		}

		controller.Move(direction.normalized * baseSpeed * Time.deltaTime);
		animator.SetFloat("Speed", direction.magnitude);
	}
}