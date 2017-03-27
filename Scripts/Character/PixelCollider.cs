using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class PixelCollider : MonoBehaviour {

	// オブジェクトのスプライト
	[SerializeField]
	public Renderer render;
	// 現在の衝突レイヤー
	protected LayerMask nowLayer;
	protected LayerMask scrapLayer;
	public Collider2D bodyCollider = null;
	BoxCollider2D boxCol = null;
	CircleCollider2D circleCol = null;

	// スプライトの真ん中の座標
	public Vector3 spriteCenter = new Vector2(0, 0);
	// スプライトの左端のx座標
	public float spriteLeftX = 0.0f;
	// スプライトの右端のx座標
	public float spriteRightX = 0.0f;
	// スプライトの上のy座標
	public float spriteUpY = 0.0f;
	// スプライトの下のy座標
	public float spriteBottomY = 0.0f;
	// スプライトの真ん中の座標
	public Vector3 colliderCenter = new Vector2(0, 0);
	// コライダーの左端のx座標
	public float colliderLeftX = 0.0f;
	// コライダーの右端のx座標
	public float colliderRightX = 0.0f;
	// コライダーの上のy座標
	public float colliderUpY = 0.0f;
	// コライダーの下のy座標
	public float colliderBottomY = 0.0f;

	// コライダーが有効かどうか
	bool isColliderEnabled = false;
	// コライダーのTriggerが有効かどうか
	protected bool isColliderTrigger = false;
	
	//地面判定
	bool grounded = false;
	ReactiveProperty<bool> isGrounded = new ReactiveProperty<bool>();
	// オブジェクトのX座標の速度
	float speedX = 0.0f;
	// オブジェクトのY座標の速度
	float speedY = 0.0f;

	protected virtual void Start()
	{
		SetCollider2D();

		// 壁と地面を検出するレイヤーを設定
		AddLayerMask(scrapLayer, TermDefinition.Instance.DefaultLayer);
		AddLayerMask(scrapLayer, TermDefinition.Instance.GroundLayer);
	}

	protected virtual void Update()
	{
	}

	public bool IsScrapped(Vector2 originPos, bool isVertical)
	{
		// 上から押しつぶす系
		if (isVertical)
		{
			// Rayを飛ばす方向。トラップに当たった場所の高さがキャラクターよりも高かった場合、プレイヤーの下にRayを飛ばす。
			var checkDir = originPos.y > bodyCollider.bounds.center.y ? -1.0f : 1.0f;
			var rayDir = new Vector2(0, checkDir);
			// Ray生成
			var scrapCheckRay = DrawRayGizmo.RayCast(bodyCollider.bounds.center, rayDir, bodyCollider.bounds.size.y / 2 + 0.01f, LayerMask.GetMask(TermDefinition.Instance.GroundLayer) | LayerMask.GetMask(TermDefinition.Instance.WallLayer), Color.red, true);
			// Rayを飛ばした先に地面があればTrue
			return scrapCheckRay;
		}
		else
		{
			// Rayを飛ばす方向。トラップにあたった場所がキャラクターよりも右だった場合、プレイヤーの左にRayを飛ばす。
			var checkDir = originPos.x > bodyCollider.bounds.center.x ? -1.0f : 1.0f;
			var rayDir = new Vector2(checkDir, 0);
			// Ray生成
			var scrapCheckRay = DrawRayGizmo.RayCast(bodyCollider.bounds.center, rayDir, bodyCollider.bounds.size.x / 2 + 0.01f, LayerMask.GetMask(TermDefinition.Instance.GroundLayer) | LayerMask.GetMask(TermDefinition.Instance.WallLayer), Color.red, true);
			// Rayを飛ばした先に地面があればTrue
			return scrapCheckRay;
		}
	}

	/// <summary>
	/// スプライト関係を更新
	/// </summary>
	protected virtual void UpdateSprite()
	{
		SetSpriteEdge();
	}

	/// <summary>
	/// コライダー関係を更新
	/// </summary>
	protected virtual void UpdateCollider()
	{
		SetColliderEdge();
	}

	/// <summary>
	/// スプライトの境界を設定
	/// </summary>
	void SetSpriteEdge()
	{
		float spriteHalfX = render.bounds.size.x / 2;
		float spriteHalfY = render.bounds.size.y / 2;
		spriteCenter = render.bounds.center;
		spriteLeftX = spriteCenter.x - spriteHalfX;
		spriteRightX = spriteCenter.x + spriteHalfX;
		spriteBottomY = spriteCenter.y - spriteHalfY;
		spriteUpY = spriteCenter.y + spriteHalfY;
	}

	/// <summary>
	/// コライダーの境界を設定
	/// </summary>
	void SetColliderEdge()
	{
		float colliderWidthHalf = 0.0f;
		float colliderHeightHalf = 0.0f;

		if (boxCol != null)
		{
			colliderWidthHalf = boxCol.bounds.size.x / 2;
			colliderHeightHalf = boxCol.bounds.size.y / 2;
			colliderCenter = boxCol.bounds.center;
		}else if(circleCol != null)
		{
			colliderWidthHalf = circleCol.bounds.size.x / 2;
			colliderHeightHalf = circleCol.bounds.size.y / 2;
			colliderCenter = circleCol.bounds.center;
		}else
		{
			Debug.Log("===Have You Attached COLLIDER?? ===");
			return;
		}

		colliderLeftX = colliderCenter.x - colliderWidthHalf;
		colliderRightX = colliderCenter.x + colliderWidthHalf;
		colliderBottomY = colliderCenter.y - colliderHeightHalf;
		colliderUpY = colliderCenter.y + colliderHeightHalf;
	}

	/// <summary>
	/// 衝突レイヤーを設定
	/// </summary>
	/// <param name="LayerMaskName">レイヤー名</param>
	public void SetLayerMask(string LayerMaskName)
	{
		nowLayer = LayerMask.GetMask(LayerMaskName);
	}

	/// <summary>
	/// 衝突レイヤーを追加
	/// </summary>
	/// <param name="LayerMaskName"></param>
	public void AddLayerMask(LayerMask layer, string LayerMaskName)
	{
		layer |= LayerMask.GetMask(LayerMaskName);
	}

	/// <summary>
	/// 衝突レイヤーを取得
	/// </summary>
	/// <returns>LayerMask</returns>
	public LayerMask GetLayerMask()
	{
		return nowLayer;
	}

	public void SetBoxCollider2D(BoxCollider2D boxCollider2D = null)
	{
		if (boxCollider2D)
		{
			boxCol = boxCollider2D;
		}else
		{
			boxCol = GetComponent<BoxCollider2D>();
		}

#if Unity_Editor
		if (boxCol == null)
		{
			Debug.Log(this.gameObject.name + "Has No BoxCollider2D");
		}
#endif
	}

	public void SetCircleCollider2D(CircleCollider2D circleCollider2D = null)
	{
		if (circleCollider2D)
		{
			circleCol = circleCollider2D;
		}else
		{
			circleCol = GetComponent<CircleCollider2D>();
		}

#if Unity_Editor
		if (boxCol == null)
		{
			Debug.Log(this.gameObject.name + "Has No CircleCollider2D");
		}
#endif
	}

	void SetCollider2D()
	{
		SetBoxCollider2D();
		SetCircleCollider2D();
		if (boxCol != null)
		{
			bodyCollider = boxCol;
		}
		else if(circleCol != null)
		{
			bodyCollider = circleCol;
		}
#if UNITY_EDITOR
		else
		{
			Debug.Log("NO COLLIDER!!");
		}
#endif
	}

	protected void UpdateColliderEdge()
	{
		SetColliderEdge();
		SetSpriteEdge();
	}

	/// <summary>
	/// コライダーを有効/無効にする
	/// </summary>
	/// <param name="enabled"></param>
	public void SetColliderEnabled(bool enabled)
	{
		if (circleCol)
		{
			circleCol.enabled = enabled;
		}
		if (boxCol)
		{
			boxCol.enabled = enabled;
		}

		isColliderEnabled = enabled;
	}

	/// <summary>
	/// コライダーの有効無効を取得する
	/// </summary>
	/// <returns></returns>
	public bool GetColliderEnabled()
	{
		return isColliderEnabled;
	}
}
