
namespace Character
{
	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections;
	using UniRx;
	using UniRx.Triggers;

	public class EnemyHorizonMove : BaseEnemy
    {

		protected override void Start()
		{
			base.Start();

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
	}
}
