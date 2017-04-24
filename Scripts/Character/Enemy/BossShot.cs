using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace Character
{

	public class BossShot : Boss
	{
		[SerializeField]
		float shotInterval = 0.3f;

		[SerializeField]
		BaseBullet bullet = null;

		[SerializeField]
		int shotBufferCount = 5;

		protected override void Initialize()
		{
			base.Initialize();

			// GunAction
			// TODO: いらないときに呼びすぎている。軽くしたい。
			var GunAction = Observable
				.Interval(TimeSpan.FromSeconds(shotInterval))
				.Select(enabled => ShotEnabled())
				.Where(x => x == true)
				//.Buffer(shotBufferCount, 7)
				.Subscribe(_ =>
				{
					Vector2 speedVec = rb2D.velocity.normalized;
					float rotation = Mathf.Atan2(speedVec.x, speedVec.y) * 180 / Mathf.PI;
					if (rotation > 180)
					{
						rotation -= 360;
					}
					if (rotation < -180)
					{
						rotation += 360;
					}
					rotation -= 90;
					Shot(Quaternion.Euler(0, 0, rotation));
				}
				)
				.AddTo(gameObject);

			float speed = speedVx;
			var _canMove = this.UpdateAsObservable();
			_canMove
				.Select(x => CanMove)
				.DistinctUntilChanged()
				.Subscribe(x =>
				{
					if (x)
					{
						SetSpeedVx(speed);
					}
					else
					{
						SetSpeedVx(0.0f);
					}
				})
				.AddTo(gameObject);
		}

		// Update is called once per frame
		void Update()
		{
		}

		/// <summary>
		/// ショットを撃てるか
		/// </summary>
		/// <returns>プレイヤーが動いていればTrueを返す</returns>
		bool ShotEnabled()
		{
			return /*(speedVx != 0 || speedVy != 0) &&*/ isAlive;
		}

		/// <summary>
		/// 弾を撃つ
		/// </summary>
		void Shot(Quaternion rotation)
		{
			BaseBullet bulletInstance = Instantiate(bullet, transform.position, rotation) as BaseBullet;
			bulletInstance.Initialize(speedVx);
		}
	}

}