
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
		Collider2D[] colliderList;
		GroundCollider groundCollider;

		// 生存フラグ
		bool isAlive;
		// 動けるかフラグ。拡張予定
		public bool CanMove { get { return isAlive; } }



		private void Awake()
		{
			colliderList = GetComponents<Collider2D>();
			groundCollider = GetComponent<GroundCollider>();
		}

		private void Start()
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
					Dead();
				}
				else
				{
					player.Dead();
				}
			}
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.tag == TermDefinition.Instance.PlayerTag)
			{
				Player player = collision.gameObject.GetComponent<Player>();

				if (collision.transform.position.y > transform.position.y)
				{
					player.ActionJump();
					Dead();
				}
				else
				{
					player.Dead();
				}
			}
		}

		void Dead()
		{
			Destroy(gameObject);
		}

		/// <summary>
		/// ボスが死んだ時に呼び出す。
		/// </summary>
		protected virtual void BossDefeated()
		{
		}
    }
}
