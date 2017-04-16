using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class PlayerBulletScript : BaseBullet {


	/// <summary>
	/// 初期化。生成時にPlayerのスピードを取得
	/// </summary>
	/// <param name="playerSpeed"></param>
	public void Initialize (float playerVx = 0.0f, float playerVy = 0.0f)
	{
		bulletSpeed = new Vector2(bulletSpeed.x * Mathf.Sign(playerVx), bulletSpeed.y * Mathf.Sign(playerVy));
		bulletSpeed += new Vector2(playerVx * 2, playerVy * 2);

		// 向きを変える
		transform.localScale = new Vector3(Mathf.Sign(playerVx), Mathf.Sign(playerVy), transform.localScale.z);
	}
}
