using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace Character
{
	public class DamageController : MonoBehaviour
	{
		public float hp = 0;
		// 死んだかどうか
		public bool isDead = false;

		/// <summary>
		/// 外部からのアクセス用。ダメージを受ける処理。
		/// </summary>
		public void OnDamaged()
		{
			if (hp == 0)
			{
				Dead();
			}
		}

		/// <summary>
		/// 死亡
		/// </summary>
		public void Dead()
		{
			// 死亡演出前に死亡フラグをTrueにする。
			isDead = true;

			// 死亡演出が完了したら破壊
			Observable.FromCoroutine(DeadEvent)
				.Subscribe(

				onError =>
				{
					// どうしても完了時に一回呼ばれてしまう
					//Debug.Log("DeadEvent: ERROR!!!");
				},

				// 完了
				() => 
				{
					 Vainish();
				})
				.AddTo(gameObject);
		}

		/// <summary>
		/// 死亡演出。継承して中身を入れる
		/// </summary>
		/// <returns></returns>
		protected virtual IEnumerator DeadEvent()
		{
			yield break;
		}

		/// <summary>
		/// キャラクターを消滅させる
		/// </summary>
		void Vainish()
		{
			Destroy(gameObject);
		}

		private void OnDisable()
		{
		}
	}
}