using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character;

public class PlayerCollider : PixelCollider {

	Player player;

	RaycastHit2D groundRay;

	protected override void Awake()
	{
		player = Player.GetPlayer();
		render = player.render;

		base.Awake();

	}

	protected override void Update()
	{
		//groundRay = DrawRayGizmo.RayCast(new Vector2(spriteLeftX, spriteBottomY - Mathf.Abs(player.speedVy * 0.01f)), Vector2.right, 1
		//	, LayerMask.GetMask(TermDefinition.Instance.GroundLayer), Color.yellow, true);

		groundRay = DrawRayGizmo.RayCast(new Vector2(spriteCenter.x, spriteBottomY - Mathf.Abs(player.speedVy * 0.01f)), Vector2.up, spriteUpY - spriteBottomY
			, LayerMask.GetMask(TermDefinition.Instance.GroundLayer), Color.yellow, true);

		base.Update();

		if (groundRay)
		{
			player.SetGrounded(true);
		}else
		{
			player.SetGrounded(false);
		}
	}
	
}
