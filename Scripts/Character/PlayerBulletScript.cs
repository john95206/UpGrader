using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class PlayerBulletScript : BaseBullet {

	/// <summary>
	/// 角度で初期化
	/// </summary>
	public void Initialize(float playerSpeed)
	{
		// 敵を標的にする
		isEnemyTarget = true;

		// 自身の向きベクトル取得
		float angleDir = transform.eulerAngles.z * (Mathf.PI / 180.0f);
		dir = new Vector3(Mathf.Cos(angleDir), Mathf.Sin(angleDir), 0.0f);
		speed += playerSpeed;
	}

	protected override void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag != TermDefinition.Instance.PlayerTag)
		{
			base.OnTriggerEnter2D(col);
		}
	}

	protected override void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag != TermDefinition.Instance.PlayerTag)
		{
			base.OnCollisionEnter2D(col);
		}
	}
}