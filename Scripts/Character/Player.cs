namespace Character
{
	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections;
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
		GroundCollider collider;
		// 接地したコライダーの保存用
		GameObject groundedGameObject;

		[SerializeField]
		public UpGradeItem.ItemType gotItem;

		[SerializeField]
		bool IsActive;
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
		public const float normalGravity = 30;
		public float changableGravity = 0;

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
			collider = GetComponent<GroundCollider>();
			// renderはInspectorで取得
			collider.render = this.render;
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
		}

		private void Update()
		{
			if ((int)gotItem >= (int)UpGradeItem.ItemType.UPGRADE_JUMP)
			{
				if (inputMaster.JoyPadCheck(GameManager.INPUT_TYPE.JUMP))
				{
					if (JumpCheck())
					{
						ActionJump();
					}
				}
			}
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
			speedVy = rb2D.velocity.y;
			// Memo: anti rigidbody2D
			//speedVy -= GetGravity() * Time.fixedDeltaTime;

			// 接地判定の更新
			UpdateGrounded();

			rb2D.velocity = new Vector2(Mathf.Clamp(speedVx, minSpeed.x, maxSpeed.y), Mathf.Clamp(speedVy, minSpeed.y, maxSpeed.y));
			#endregion
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
					if (collider.CheckGrounded())
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
			return jumpEnabled || collider.CheckGrounded();
		}

		public void ActionJump()
		{
			rb2D.velocity = jumpPower;
			// Memo: anti rigidbody2D
			//SetGrounded(false);
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
	}
}
