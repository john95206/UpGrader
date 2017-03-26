using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenObject : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		DOTween.Init();
	}
	
	public void MoveHorizontal(GameObject go, float moveX, float time = 0)
	{
	}
}
