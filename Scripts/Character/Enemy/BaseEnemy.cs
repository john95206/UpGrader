
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
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.gameObject.tag == "Player")
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
