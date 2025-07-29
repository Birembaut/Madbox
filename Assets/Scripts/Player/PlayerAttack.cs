using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField]
	private Transform weaponSlot; 
	private WeaponInstance weaponData;

	[SerializeField]
	private Enemy target;

	private PlayerLife playerLife;
	private Animator animator;

	[SerializeField]
	private bool isMoving;
	[SerializeField]
	private bool canAttack;

	private GameObject currentWeaponVisual;

	//Upgrades
	private bool canPierce = false;
	private bool canSlownOnHit = false;
	private UpgradeData doubleProjectile = null;
	private UpgradeData diagonalProjectile = null;
	private int reboundAddedOnProjectile = 0;

	private void Awake()
	{
		playerLife = GetComponent<PlayerLife>();
		animator = GetComponentInChildren<Animator>();
		GetComponentInChildren<AnimatorEvent>().Player = this;
	}

	private void Start()
	{
		InputManager.Instance.PlayerMoved += OnPlayerMoved;
		InventoryManager.Instance.OnWeaponChanged += OnWeaponChanged;
		WaveManager.Instance.EnemyDied += OnEnemyDied;
		UpgradeManager.Instance.UpgradeAcquired += OnUpgradeAcquired;
		GameManager.Instance.OnGameStateChanged += HandleStateChange;

		Reset();
	}

	private void OnDestroy()
	{
		InputManager.Instance.PlayerMoved -= OnPlayerMoved;
		InventoryManager.Instance.OnWeaponChanged -= OnWeaponChanged;
		WaveManager.Instance.EnemyDied -= OnEnemyDied;
		UpgradeManager.Instance.UpgradeAcquired -= OnUpgradeAcquired;
		GameManager.Instance.OnGameStateChanged -= HandleStateChange;
	}

	private void HandleStateChange(GameState state)
	{
		switch (state)
		{
			case GameState.GameOver:
				{
					Reset();
					break;
				}

			default:
					break;
		}
	}

	private void OnUpgradeAcquired(UpgradeData data)
	{
		switch(data.upgradeID)
		{
			case UpgradeID.Piercing:
				canPierce = true;
				break;
			case UpgradeID.SlowOnHit:
				canSlownOnHit = true;
				break;
			case UpgradeID.Rebound:
				reboundAddedOnProjectile = (int)data.value;
				break;
			case UpgradeID.DoubleProjectile:
				doubleProjectile = data;
				break;
			case UpgradeID.DiagonalProjectile:
				diagonalProjectile = data;
				break;
		}
	}

	private void OnPlayerMoved(Vector2 movement)
	{
		isMoving = movement != Vector2.zero;
		if(isMoving)
		{
			canAttack = true;
			target = null;
		}
	}

	private void OnWeaponChanged(WeaponInstance newData)
	{
		weaponData = newData;
		animator.SetFloat("AttackSpeed", weaponData.baseData.AnimationAttackSpeedMultiplier);

		if (GameManager.Instance.CurrentState == GameState.Inventory)
		{
			animator.SetTrigger("Attack");
		}

		if (currentWeaponVisual != null)
		{
			Destroy(currentWeaponVisual);
		}

		currentWeaponVisual = Instantiate(weaponData.baseData.WeaponVisualPrefab, weaponSlot);
		currentWeaponVisual.transform.localPosition = Vector3.zero;
	}

	private void OnEnemyDied(Enemy enemy)
	{
		if(target == enemy)
		{
			target = null;
		}
	}

	private void Update()
	{
		if (GameManager.Instance.CurrentState != GameState.Playing)
		{
			return;
		}

		if(playerLife.IsDead)
		{
			return;
		}

		if (isMoving)
		{
			return;
		}

		if(target == null)
		{
			target = WaveManager.Instance.FindNearestTarget(transform.position);
		}

		if (target != null)
		{
			transform.LookAt(target.transform.position);

			if (canAttack)
			{
				canAttack = false;
				animator.SetTrigger("Attack");
			}
		}
	}

	public void Attack()
	{
		if (GameManager.Instance.CurrentState != GameState.Playing)
		{
			return;
		}

		if (target == null)
		{
			return;
		}

		if (doubleProjectile != null)
		{
			Vector3 offset = transform.right * doubleProjectile.value;
			SpawnProjectile(offset);
			SpawnProjectile(-offset);
		}
		else
		{
			SpawnProjectile(Vector3.zero);
		}

		if (diagonalProjectile != null)
		{
			SpawnProjectile(Vector3.zero, diagonalProjectile.value);
			SpawnProjectile(Vector3.zero, -diagonalProjectile.value);
		}

		SoundManager.Instance.PlayShoot();
	}

	private void SpawnProjectile(Vector3 positionOffset, float directionOffSet = 0f)
	{

		GameObject projectileGO = EntityFactory.Instance.Spawn(weaponData.baseData.weaponId, transform.position + positionOffset);
		Projectile projectile = projectileGO.GetComponent<Projectile>();

		Vector3 attackTargetPosition = transform.forward;
		if(directionOffSet != 0)
		{
			Vector3 rotatedDirection = Quaternion.Euler(0, directionOffSet, 0) * attackTargetPosition;
			attackTargetPosition = transform.position + rotatedDirection;
		}
		else if (target != null)
		{
			// Add security if target is dead during animation
			attackTargetPosition = target.transform.position + positionOffset;
		}

		Debug.Log($"Spawn projectile {projectileGO.name} with positionOffset {positionOffset} and direction offset {directionOffSet}, attackTarget Position {attackTargetPosition}");

		switch (projectile)
		{
			case SimpleProjectile simpleProjectile:
				{
					simpleProjectile.Launch(attackTargetPosition, weaponData);
					break;
				}

			case HomingProjectile homingProjectile:
				{
					homingProjectile.Launch(attackTargetPosition, weaponData, target.gameObject);
					break;
				}

			case BoomerangProjectile boomerangProjectile:
				{
					boomerangProjectile.Launch(attackTargetPosition, weaponData, this.gameObject);
					break;
				}
		}

		if (canPierce)
		{
			projectile.SetPiercing();
		}

		if(canSlownOnHit)
		{
			projectile.SetSlowOnImpact();
		}

		projectile.SetRebound(reboundAddedOnProjectile);
	}

	public void FinishAttack()
	{
		canAttack = true;
	}

	public void Reset()
	{
		isMoving = false;
		canAttack = true;

		// Upgrades
		canPierce = false;
		canSlownOnHit = false;
		doubleProjectile = null;
		diagonalProjectile = null;
		reboundAddedOnProjectile = 0;
	}
}