namespace Character
{
	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections;
	using UpGradeItem;
	using UniRx;
	using UniRx.Triggers;
	using Zenject;

	public class Player : MonoBehaviour
	{
		[Inject]
		GameManager.InputMaster inputMaster;

		[NonSerialized]
		public Rigidbody2D rb2D;
		// 子オブジェクトに配置するかもしれないのでInspectorでアタッチを想定
		public Renderer render;
		GroundCollider groundCollider;
		// 接地したコライダーの保存用
		GameObject groundedGameObject;

		ItemType gotItem;

		// 生きているかフラグ。外からは取得だけできるようにする
		public bool IsActive { get; private set; }
		// 罠にかかっているか？
		public bool isTrapped = false;
		bool jumpEnabled = true;
		bool isVisible = false;

		[SerializeField]
		float moveSpeed = 2.0f;
		[SerializeField]
		Vector2 jumpPower = new Vector2(0, 6);
		[SerializeField]
		Vector2 maxSpeed = new Vector2(8, 5);
		[SerializeField]
		Vector2 minSpeed = new Vector2(-8, -5);
		// ショットの間隔
		[SerializeField]
		float shotInterval = 0.5f;
		// ショットの弾
		[SerializeField]
		PlayerBulletScript bullet = null;
		[SerializeField]
		float frashShotNum = 16;
		[SerializeField]
		float quickSpeed = 2;
		public const float normalGravity = 30;
		public float changableGravity = 1;

		FloatReactiveProperty gravity = new FloatReactiveProperty(1);
		
		public float speedVx = 0;
		public float speedVy = 0;

		float groundY = 0;

		public BoolReactiveProperty grounded = new BoolReactiveProperty(false);

		/// <summary>
		/// 他のオブジェクトの参照用
		/// </summary>
		/// <returns>Player</returns>
		public static Player GetPlayer()
		{
			return GameObject.FindGameObjectWithTag(TermDefinition.Instance.PlayerTag).GetComponent<Player>();
		}

		private void Awake()
		{
			rb2D = GetComponent<Rigidbody2D>();
			groundCollider = GetComponent<GroundCollider>();
			// renderはInspectorで取得
			if(groundCollider != null)
			{
				groundCollider.render = this.render;
			}else
			{
				Debug.Log("NULL: GroundCollider");
			}
		}

		private void Start()
		{
			IsActive = true;

			rb2D.gravityScale = gravity.Value;
			// Memo: anti rigidbody2D
			//grounded.Subscribe(_ =>
			//{
			//	if (grounded.Value)
			//	{
			//		Grounded();
			//	}
			//	else
			//	{
			//		DetouchGround();
			//	}
			//});

			// GunAction
			// TODO: いらないときに呼びすぎている。軽くしたい。
			var GunAction = Observable
				.Interval(TimeSpan.FromSeconds(shotInterval))
				.Select(enabled => ShotEnabled())
				.Where(x => x == true)
				.Subscribe(_ =>
				{
					Vector2 speedVec = new Vector2(speedVx, speedVy).normalized;
					float rotation = Mathf.Atan2(speedVec.x, speedVec.y) * 180 / Mathf.PI;
					if(rotation > 180)
					{
						rotation -= 360;
					}
					if(rotation < -180)
					{
						rotation += 360;
					}
					rotation -= 90;
					Shot(Quaternion.Euler(0, 0, rotation));
				}
				)
				.AddTo(gameObject);
		}

		/// <summary>
		/// ショットを撃てるか
		/// </summary>
		/// <returns>プレイヤーが動いていればTrueを返す</returns>
		bool ShotEnabled()
		{
			return (speedVx != 0 || speedVy != 0) && CheckUpgradeStatus(ItemType.UPGRADE_GUN);
		}

		private void Update()
		{
			// パワーアップアクション
			ActionAttack();
		}

		private void FixedUpdate()
		{
			float speed = 0.0f;

			#region InputHorizontal
			if (inputMaster.JoyPadCheck(GameManager.INPUT_TYPE.LEFT))
			{
				speed = moveSpeed * -1;
			}
			else if (inputMaster.JoyPadCheck(GameManager.INPUT_TYPE.RIGHT))
			{
				speed = moveSpeed * 1;
			}
			#endregion

			#region SpeedCalculate
			speedVx = speed;
			// Memo: anti rigidbody2D
			//speedVy -= GetGravity() * Time.fixedDeltaTime;

			// 接地判定の更新
			UpdateGrounded();

			rb2D.velocity = new Vector2(Mathf.Clamp(speedVx, minSpeed.x, maxSpeed.y), Mathf.Clamp(speedVy, minSpeed.y, maxSpeed.y));
			#endregion
		}

		/// <summary>
		/// パワーアップアクション
		/// </summary>
		void ActionAttack()
		{
			// ジャンプ
			if (CheckUpgradeStatus(ItemType.UPGRADE_JUMP))
			{
				if (inputMaster.JoyPadCheck(GameManager.INPUT_TYPE.JUMP))
				{
					if (JumpCheck())
					{
						ActionJump();
					}
					else if(CheckUpgradeStatus(ItemType.UPGRADE_DOUBLEJUMP))
					{
						ActionJump();
					}
				}
			}

			// 銃攻撃はStart()でObservableにやる
			
		}

		/// <summary>
		/// プレイヤーを強化。Managerから呼ぶ。
		/// </summary>
		/// <param name="newType"></param>
		public void SetUpgradeStatus(ItemType newType)
		{
			// プレイヤーのステータスを更新
			gotItem = newType;

			switch (gotItem)
			{
				case ItemType.UPGRADE_QUICK:
					// スピードの限界を上げ、キャラの基本的な速度を上げる
					maxSpeed.x *= quickSpeed;
					maxSpeed.y *= quickSpeed;
					minSpeed.x *= quickSpeed;
					minSpeed.y *= quickSpeed;
					moveSpeed *= quickSpeed;
					SetGravity(quickSpeed);
					break;
			}
		}

		/// <summary>
		/// プレイヤーが十分強化されているかチェック
		/// </summary>
		/// <param name="itemType"></param>
		/// <returns>強化が十分ならTrue</returns>
		bool CheckUpgradeStatus(ItemType itemType)
		{
			// 現在のプレイヤーの強化状態が引数のものよりも上回っていればTrue
			return (int)gotItem >= (int)itemType;
		}

		/// <summary>
		/// 弾を撃つ
		/// </summary>
		void Shot(Quaternion rotation)
		{
			PlayerBulletScript bulletInstance = Instantiate(bullet, transform.position, rotation) as PlayerBulletScript;
			bulletInstance.Initialize(moveSpeed);
		}

		/// <summary>
		/// 接地判定の更新
		/// </summary>
		void UpdateGrounded()
		{
			if (!GetGrounded())
			{
				speedVy = rb2D.velocity.y;
			}
			else
			{
				rb2D.gravityScale = 0;
				if (speedVy < 0)
				{
					speedVy = 0;
				}
			}
		}

		void Grounded(Collision2D collision)
		{
			// 接地したGameObjectを取得
			groundedGameObject = collision.gameObject;
			// 接地したのでジャンプしてもいいですよ
			jumpEnabled = true;
			// 重力を無効にする
			rb2D.gravityScale = 0;
			// Memo: anti rigidbody2D
			//if(GetGravity() > 0)
			//{
			//	SetGravity(-normalGravity);
			//	speedVy = 0;
			//}
		}

		void DetouchGround()
		{
			groundedGameObject = null;
			jumpEnabled = false;
			rb2D.gravityScale = gravity.Value;
			// Memo: anti rigidbody2D
			//if (GetGravity() <= 0)
			//{
			//	SetGravity(normalGravity);
			//}
		}

		/// <summary>
		/// カメラ内に自分が収まっているときに毎秒60回呼ばれる
		/// </summary>
		void OnWillRenderObject()
		{
#if UNITY_EDITOR
			groundCollider.CheckGrounded();
#endif
			// そのカメラはメインカメラ？
			if (Camera.current.name == TermDefinition.Instance.MainCameraName)
			{
				// 見えているぞ
				isVisible = true;
			}
			else
			{
				// 観なかったことにしてやろう
				isVisible = false;
			}
			Camera.main.GetComponent<CameraScripts>().CameraSwtich(transform.position);
		}

		/// <summary>
		/// 衝突時の処理
		/// </summary>
		/// <param name="collision"></param>
		private void OnCollisionEnter2D(Collision2D collision)
		{
			#region Grounded
			//接触したのが"Stage"タグだったら
			if (collision.gameObject.tag == TermDefinition.Instance.StageTag)
			{
				// 接触したポイントすべてにおいて接地判定
				foreach (ContactPoint2D point in collision.contacts)
				{
					// 接地点の方がボーダーラインよりも低かったら
					if (groundCollider.CheckGrounded())
					{
						Grounded(collision);
						break;
					}
				}
			}
			#endregion
		}
		/// <summary>
		/// 物体から離れたときの処理
		/// </summary>
		/// <param name="collision"></param>
		private void OnCollisionExit2D(Collision2D collision)
		{
			#region DetouchGrounded
			// 接地していたら
			if (groundedGameObject != null)
			{
				// 離れた物体が地面だったら離地処理
				if (collision.gameObject == groundedGameObject)
				{
					DetouchGround();
				}
			}
			#endregion
		}

		bool JumpCheck()
		{
			return jumpEnabled || groundCollider.CheckGrounded();
		}

		public void ActionJump()
		{
			// 全方位攻撃
			if (CheckUpgradeStatus(ItemType.UPGRADE_FRASH))
			{
				ActionFrash();
			}
			rb2D.velocity = jumpPower;
			// Memo: anti rigidbody2D
			//SetGrounded(false);
		}

		/// <summary>
		/// ジャンプ時に全方位にShotする攻撃。
		/// </summary>
		void ActionFrash()
		{
			for(int i = 0; i <= frashShotNum; i++)
			{
				Shot(Quaternion.Euler(0, 0, i * 360 / frashShotNum));
			}
		}

		public void Damaged()
		{
			Dead();
		}

		public void Dead()
		{
			Destroy(gameObject);
		}

		/// <summary>
		/// 重力の値を変更する
		/// </summary>
		/// <param name="gravityValue"></param>
		public void SetGravity(float gravityValue)
		{
			changableGravity = gravityValue;
			gravity.Value = changableGravity;
		}

		public float GetGravity()
		{
			return gravity.Value + changableGravity;
		}

		/// <summary>
		/// 接地フラグの変更
		/// </summary>
		/// <param name="isGrounded"></param>
		public void SetGrounded(bool isGrounded)
		{
			grounded.Value = isGrounded;
		}

		/// <summary>
		/// 接地フラグを取得
		/// </summary>
		/// <returns></returns>
		public bool GetGrounded()
		{
			return grounded.Value;
		}
		
		public bool GetScrapped(Vector2 originPos, bool isVertical)
		{
			return groundCollider.IsScrapped(originPos, isVertical);
		}

		public float GetPlayerWidth()
		{
			if (groundCollider.bodyCollider == null)
			{
#if UNITY_EDITOR
				Debug.Log("NO COLLIDER");
#endif
				return 0;
			}
			return groundCollider.bodyCollider.bounds.size.x;
		}

		public float GetPlayerHeight()
		{
			if(groundCollider.bodyCollider == null)
			{
#if UNITY_EDITOR
				Debug.Log("NO COLLIDER");
#endif
				return 0;
			}
			return groundCollider.bodyCollider.bounds.size.y;
		}
	}
}
