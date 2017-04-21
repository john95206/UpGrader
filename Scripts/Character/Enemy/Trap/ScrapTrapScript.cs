namespace Character
{

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Zenject;

	public class ScrapTrapScript : TrapBaseScript
	{
		[Inject]
		GameManager.CameraManager cameraManager;
		TweenOneWay tween;
		BoxCollider2D col = null;

		[SerializeField]
		bool ShakeCameraEnabled = true;
		// 上から押しつぶすタイプかどうか
		bool isVerticalScrap = false;

		protected override void Start()
		{
			base.Start();

			col = GetComponent<BoxCollider2D>();
			tween = GetComponent<TweenOneWay>();

			// 水平方向に判定が長い＝上下に押しつぶすタイプと判断
			isVerticalScrap = col.bounds.size.x > col.bounds.size.y;
		}

		public void OnTrapEnabled()
		{
			if (tween != null)
			{
				isActive = true;
				tween.Move(()=> OnCompleted());
			}
		}

		void OnCompleted()
		{
			isActive = false;
			cameraManager.ShakeCaemra();
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			// アクティブでないなら処理終了
			if (!isActive)
			{
				return;
			}

			if (collision.gameObject.tag == TermDefinition.Instance.PlayerTag)
			{
				Player player = collision.gameObject.GetComponent<Player>();
				if (player == null)
				{
					Debug.Log("Player has no Player COMPONENT!!");
					return;
				}
				// Playerが死んでたら終了
				if (!player.IsActive)
				{
					Debug.Log("Player is UNENABLED!!");
					return;
				}

				for(int i = 0; i < collision.contacts.Length; i++)
				{
					if (player.GetScrapped(collision.contacts[i].point, isVerticalScrap))
					{
						// プレイヤー殺す
						player.Dead();
						break;
					}
				}
			}
		}
	}
}