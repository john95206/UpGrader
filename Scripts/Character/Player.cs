namespace Character
{
	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections;
	using UniRx;
	using UniRx.Triggers;
	using Zenject;

	public class Player : MonoBehaviour
    {
		[Inject]
		GameManager.InputMaster inputMaster;

		Rigidbody2D rb2D;

		[SerializeField]
		public UpGradeItem.ItemType gotItem;

		[SerializeField]
		bool IsActive;
		bool jumpEnabled = true;

		[SerializeField]
		float moveSpeed = 2.0f;
		[SerializeField]
		float jumpPower = 180;

		[SerializeField]
		float speedVx = 0;
		[SerializeField]
		float speedVy = 0;

		private void Awake()
		{
			rb2D = GetComponent<Rigidbody2D>();
		}

		private void Start()
		{
			IsActive = true;
		}

		private void Update()
		{
			if((int)gotItem >= (int)UpGradeItem.ItemType.UPGRADE_JUMP)
			{
				if (inputMaster.JoyPadCheck(GameManager.INPUT_TYPE.JUMP) && jumpEnabled)
				{
					rb2D.AddForce(new Vector2(0, jumpPower));
				}
			}
			
		}

		private void FixedUpdate()
		{
			float speed = 0.0f;

			if (inputMaster.JoyPadCheck(GameManager.INPUT_TYPE.LEFT))
			{
				speed = moveSpeed * -1;
			}
			else if (inputMaster.JoyPadCheck(GameManager.INPUT_TYPE.RIGHT))
			{
				speed = moveSpeed * 1;
			}

			speedVx = speed;
			speedVy = rb2D.velocity.y;

			rb2D.velocity = new Vector2(speedVx, speedVy);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if(collision.gameObject.tag == "Stage")
			{
				jumpEnabled = true;
			}
		}
		private void OnCollisionExit2D(Collision2D collision)
		{
			if (collision.gameObject.tag == "Stage")
			{
				jumpEnabled = false;
			}

		}
	}
}
