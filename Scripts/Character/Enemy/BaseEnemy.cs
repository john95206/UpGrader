
namespace Character
{
	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections;
	using UniRx;
	using UniRx.Triggers;

    public class BaseEnemy : MonoBehaviour
    {
		protected Collider2D[] colliderList;
		protected GroundCollider groundCollider;
		protected Rigidbody2D rb2D;
		protected DamageController damageCtrl;

		// カメラに写っているかフラグ
		protected bool isVisible = false;
		// 生存フラグ
		protected bool isAlive;
		// 動けるかフラグ。拡張予定
		public bool CanMove
		{
			get
			{
				return isAlive && isVisible;
			}
		}
		// 向いている方向
		public FloatReactiveProperty dir = new FloatReactiveProperty(-1.0f);
		// x座標のスピード
		public float speedVx = 0.0f;
		// y座標のスピード
		public float speedVy = 0.0f;
		BoolReactiveProperty groundEdgeCheck = new BoolReactiveProperty();

		[SerializeField]
		protected bool isGroundEdgeChecker = true;

		public virtual void Awake()
		{
			colliderList = GetComponents<Collider2D>();
			groundCollider = GetComponent<GroundCollider>();
			rb2D = GetComponent<Rigidbody2D>();
			damageCtrl = GetComponent<DamageController>();
		}

		protected virtual void Start()
		{
			// dirが変化したらlocalScaleを更新
			dir.Subscribe(_ =>
			transform.localScale = new Vector3(transform.localScale.x * dir.Value, transform.localScale.y, transform.localScale.z)
			)
			// 死んだらストリーム破棄
			.AddTo(this.gameObject);

			isAlive = true;

			Initialize();
		}

		protected virtual void Initialize()
		{

		}

		private void Update()
		{
			if (groundCollider != null)
			{
				// 崖っぷち時を気にする？
				if (isGroundEdgeChecker)
				{
					// 崖っぷちの時に方向転換
					if (groundCollider.CheckGroundEdge(dir.Value))
					{
						LookBack();
					}
				}
			}
		}

		void FixedUpdate()
		{
			FixedUpdateCharacter();

			if(rb2D != null)
			{
				rb2D.velocity = new Vector2(speedVx * dir.Value, rb2D.velocity.y + speedVy);
			}
		}

		public virtual void FixedUpdateCharacter()
		{
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.gameObject.tag == TermDefinition.Instance.PlayerTag)
			{
				Player player = collision.GetComponent<Player>();

				if (collision.transform.position.y > transform.position.y)
				{
					player.ActionJump();
					Damaged();
				}
				else
				{
					player.Damaged();
				}
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			// プレイヤーとの衝突イベント
			if (collision.gameObject.tag == TermDefinition.Instance.PlayerTag)
			{
				Player player = collision.gameObject.GetComponent<Player>();

				if (collision.transform.position.y > transform.position.y)
				{
					player.ActionJump();
					Damaged();
				}
				else
				{
					player.Damaged();
				}
			}
			
			// 壁とかに衝突したら引き返す
			foreach(ContactPoint2D contact in collision.contacts)
			{
				if(contact.point.y > groundCollider.spriteBottomY)
				{
					LookBack();
					break;
				}
			}
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
		}

		private void OnBecameInvisible()
		{
			isVisible = false;
		}

		/// <summary>
		/// 引き返す
		/// </summary>
		void LookBack()
		{
			dir.Value *= -1;
		}

		/// <summary>
		/// SpeedVxを上書きする
		/// </summary>
		/// <param name="vx"></param>
		protected void SetSpeedVx(float vx)
		{
			speedVx = vx;
		}

		/// <summary>
		/// SpeedVyを上書きする
		/// </summary>
		/// <param name="vy"></param>
		protected void SetSpeedVy(float vy)
		{
			speedVy = vy;
		}

		/// <summary>
		/// SpeedVxに加算
		/// </summary>
		/// <param name="vx"></param>
		protected void AddSpeedVx(float vx)
		{
			speedVx += vx;
		}

		/// <summary>
		/// SpeedVyに加算
		/// </summary>
		/// <param name="vy"></param>
		protected void SpeedVy(float vy)
		{
			speedVy += vy;
		}

		/// <summary>
		/// ダメージを受けた処理。DamageControllerに委任
		/// </summary>
		void Damaged()
		{
			if(damageCtrl != null)
			{
				damageCtrl.OnDamaged();
			}
		}

		/// <summary>
		/// ボスが死んだ時に呼び出す。使うかは未定
		/// </summary>
		protected virtual void BossDefeated()
		{
		}
    }
}
