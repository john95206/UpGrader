using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class BaseBullet : MonoBehaviour {

	// 標的が敵キャラか？　
	// True：敵のみにあたり判定
	// False: 自分のみにあたり判定
	// null: 敵味方にあたり判定
	protected bool? isEnemyTarget = false;
	[SerializeField]
	protected Vector2 bulletSpeedVector = Vector2.zero;
	[SerializeField]
	protected float lifeTime = 3.0f;
	[SerializeField]
	bool isRotation = false;
	[SerializeField]
	protected float speed = 3.0f;
	protected Vector3 dir = Vector3.zero;
	
	Rigidbody2D rb2D = null;
	Renderer render = null;

	protected void Awake()
	{
		rb2D = GetComponent<Rigidbody2D>();
		if (rb2D == null)
		{
			isRotation = true;
		}

		render = GetComponent<Renderer>();
		if(render == null)
		{
			Debug.Log("Gun Renderer is null!!!");
		}

		Destroy(gameObject, lifeTime);
	}

	protected virtual void FixedUpdate()
	{
		if (rb2D != null)
		{
			rb2D.velocity = bulletSpeedVector;
		}
		else
		{
			// 自身の向きに移動
			transform.position += dir * speed * Time.deltaTime;
		}
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}

	#region Collider
	protected virtual void OnTriggerEnter2D(Collider2D col)
	{
		// 衝突相手がキャラクターだったらダメージ処理
		Character.DamageController damageCtrl = col.GetComponent<Character.DamageController>();
		if(damageCtrl != null)
		{
			damageCtrl.OnDamaged();
		}
	}

	protected virtual void OnCollisionEnter2D(Collision2D col)
	{
		// 衝突相手がキャラクターだったらダメージ処理
		Character.DamageController damageCtrl = col.gameObject.GetComponent<Character.DamageController>();
		if (damageCtrl != null)
		{
			damageCtrl.OnDamaged();
		}
	}
	#endregion
}
