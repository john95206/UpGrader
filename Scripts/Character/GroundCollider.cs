using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class GroundCollider : PixelCollider {
	
	RaycastHit2D groundRay;
	[SerializeField]
	float edgeMargin = 0.3f;
	[SerializeField]
	float edgeLength = 0.6f;

	/// <summary>
	/// 接地判定。キャラの足元から5ピクセル下の間に
	/// 地面があればTrueを返す
	/// </summary>
	/// <returns>接地判定</returns>
	public bool CheckGrounded()
	{
		groundRay = DrawRayGizmo.RayCast(new Vector2(spriteCenter.x, render.bounds.min.y - 0.05f), Vector2.up, spriteUpY - spriteBottomY
			, LayerMask.GetMask(TermDefinition.Instance.GroundLayer), Color.yellow, true);

		return groundRay;
	}

	/// <summary>
	/// 崖っぷちフラグ
	/// </summary>
	/// <returns>がけっぷちにいる→True</returns>
	public bool CheckGroundEdge(float dir)
	{
		// そもそも設置してなかったらfalse
		if (!CheckGrounded())
		{
			Debug.Log("NOT GROUNDED!!");
			return false;
		}

		// キャラの右端に地面があるか検出
		RaycastHit2D rightEdgeRay = DrawRayGizmo.RayCast(new Vector2(spriteRightX - edgeMargin, spriteBottomY - 0.05f), Vector2.right, edgeLength, LayerMask.GetMask(TermDefinition.Instance.GroundLayer), Color.gray, true);
		// キャラの左端に地面があるか検出
		RaycastHit2D leftEdgeRay = DrawRayGizmo.RayCast(new Vector2(spriteLeftX + edgeMargin, spriteBottomY - 0.05f), Vector2.left, edgeLength, LayerMask.GetMask(TermDefinition.Instance.GroundLayer), Color.gray, true);

		// 左を向いているときは左が崖っぷちだったらTrue、右を向いているときは右が崖っぷちだったらTrue
		return dir < 0 ? !leftEdgeRay : !rightEdgeRay;
	}
}
