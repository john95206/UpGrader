using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class GroundCollider : PixelCollider {
	
	RaycastHit2D groundRay;
	
	/// <summary>
	/// 接地判定。キャラの足元から5ピクセル下の間に
	/// 地面があればTrueを返す
	/// </summary>
	/// <returns>接地判定</returns>
	public bool CheckGrounded()
	{
		groundRay = DrawRayGizmo.RayCast(new Vector2(spriteCenter.x, spriteBottomY - 0.05f), Vector2.up, spriteUpY - spriteBottomY
			, LayerMask.GetMask(TermDefinition.Instance.GroundLayer), Color.yellow, true);

		return groundRay;
	}

	/// <summary>
	/// 崖っぷちフラグ
	/// </summary>
	/// <returns>がけっぷちにいる→True</returns>
	public bool CheckGroundEdge()
	{
		// そもそも設置してなかったらfalse
		if (!CheckGrounded())
		{
			Debug.Log("GroundERROR : NOT GROUNDED!!");
			return false;
		}

		// キャラの右端に地面があるか検出
		RaycastHit2D rightEdgeRay = DrawRayGizmo.RayCast(new Vector2(spriteRightX, spriteBottomY - 0.05f), Vector2.right, 1, LayerMask.GetMask(TermDefinition.Instance.GroundLayer), Color.gray, true);
		// キャラの左端に地面があるか検出
		RaycastHit2D leftEdgeRay = DrawRayGizmo.RayCast(new Vector2(spriteLeftX, spriteBottomY - 0.05f), Vector2.left, 1, LayerMask.GetMask(TermDefinition.Instance.GroundLayer), Color.gray, true);

		// 右か左で崖っぷちだったらTrue
		return !rightEdgeRay || !leftEdgeRay;
	}
}
