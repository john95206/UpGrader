using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenOneWay : MonoBehaviour {

	[SerializeField]
	bool playOnAwake = false;
	[SerializeField]
	float speed;
	[SerializeField]
	float duration;
	[SerializeField]
	Vector3 targetPos;
	[SerializeField]
	GameObject targetObject;
	[SerializeField]
	Vector3 targetScale;
	[SerializeField]
	Ease easeType = Ease.Linear;
	[SerializeField]
	bool isSnap = false;
	public TweenCallback callback;

	// Use this for initialization
	protected virtual void Start ()
	{
		DOTween.Init();

		if (playOnAwake)
		{
			Move();
		}
	}

	/// <summary>
	/// Tweenするスクリプトのパラメータに依存したMove
	/// </summary>
	public void Move(TweenCallback callBack = null)
	{
		if (callBack == null)
		{
			callBack = () => OnCompleted();
		}

		if(targetObject != null)
		{
			targetPos = targetObject.transform.position;
		}

		transform.DOLocalMove(targetPos, duration, isSnap)
			.SetEase(easeType)
			.OnComplete(callBack);
	}

	/// <summary>
	/// 呼び出し側の引数に依存したMove
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="time"></param>
	/// <param name="snap"></param>
	public void Move(Vector3 pos, float time, bool snap, GameObject targetObject = null, TweenCallback callBack = null)
	{
		if(targetObject != null)
		{
			pos = targetObject.transform.position;
		}

		transform.DOLocalMove(pos, time, snap)
			.SetEase(easeType)
			.OnComplete(callBack);
	}

	public virtual void OnCompleted()
	{

	}
}
