using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour {

	[SerializeField]
	protected Vector2 bulletSpeed = Vector2.zero;
	[SerializeField]
	protected float lifeTime = 3.0f;

	Rigidbody2D rb2D = null;

	protected void Awake()
	{
		rb2D = GetComponent<Rigidbody2D>();

		Destroy(gameObject, lifeTime);
	}

	protected virtual void FixedUpdate()
	{
		rb2D.velocity = bulletSpeed;
	}
}
