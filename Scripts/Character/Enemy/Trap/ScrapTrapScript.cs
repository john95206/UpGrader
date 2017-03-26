namespace Character
{

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ScrapTrapScript : TrapBaseScript
	{

		BoxCollider2D col = null;

		// 上から押しつぶすタイプかどうか
		bool isVerticalScrap = false;

		protected override void Start()
		{
			base.Start();

			BoxCollider2D col = GetComponent<BoxCollider2D>();

			// 水平方向に判定が長い＝上下に押しつぶすタイプと判断
			isVerticalScrap = col.bounds.size.x > col.bounds.size.y;
		}

		private void OnCollisionStay2D(Collision2D collision)
		{
			if (collision.gameObject.tag == TermDefinition.Instance.PlayerTag)
			{
				Player player = collision.gameObject.GetComponent<Player>();
				if (player == null)
				{
#if UNITY_EDITOR
					Debug.Log("Player has no Player COMPONENT!!");
#endif
					return;
				}
				// Playerが死んでたら終了
				if (!player.IsActive)
				{
#if UNITY_EDITOR
					Debug.Log("Player is UNENABLED!!");
#endif
					return;
				}

				if (player.GetScrapped(collision.contacts[0].point, isVerticalScrap))
				{
					// プレイヤー殺す
					player.Dead();
				}
			}
		}
	}
}