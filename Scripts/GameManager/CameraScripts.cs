using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraScripts : MonoBehaviour
{
	Camera thisCamera;

	private void Start()
	{
		thisCamera = GetComponent<Camera>();
	}

	/// <summary>
	/// カメラの座標を切り替える
	/// </summary>
	/// <param name="playerPos">プレイヤーの座標</param>
	public void CameraSwtich(Vector3 playerPos)
	{
		var playerPosPoint = new Vector3(playerPos.x, playerPos.y, -transform.position.z);

		// プレイヤーの座標をカメラ座標で取得
		var pos = thisCamera.ScreenToWorldPoint(playerPosPoint);

		// カメラの境界線の座標をカメラ座標で取得
		var bottomLeft = thisCamera.ViewportToWorldPoint(new Vector3(0, 0, -transform.position.z));
		var topRight = thisCamera.ViewportToWorldPoint(new Vector3(1, 1, -transform.position.z));

		// どの方向に見切れたかでカメラの新しい座標を決める。
		// 新しい座標は単位合わせのために0.01倍する
		//　左に見切れた時
		if(playerPos.x < bottomLeft.x)
		{
			SetPositionX(1200 * -0.01f);
		}
		// 右に見切れた時
		else if(playerPos.x > topRight.x)
		{
			SetPositionX(1200 * 0.01f);
		}
		// 下に見切れた時
		else if(playerPos.y < bottomLeft.y)
		{
			SetPositionY(900 * -0.01f);
		}
		// 上に見切れた時
		else if(playerPos.y > topRight.y)
		{
			SetPositionY(900 * 0.01f);
		}
	}

	/// <summary>
	/// 画面揺れ
	/// </summary>
	/// <param name="duration"></param>
	/// <param name="strength"></param>
	public void ShakeCamera(float duration = 0.5f, float strength = 0.5f, int vibrato = 30, float randomness = 90, bool snap = false, bool fadeout = false)
	{
		transform.DOShakePosition(duration, strength, vibrato, randomness, snap, fadeout);
	}
	void SetPositionX(float posX)
	{
		transform.position = new Vector3(transform.position.x + posX, transform.position.y, transform.position.z);
	}

	void SetPositionY(float posY)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + posY, transform.position.z);
	}
}
