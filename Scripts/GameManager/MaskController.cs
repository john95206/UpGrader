using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MaskController : MonoBehaviour {

	[Inject]
	GameManager.CameraManager cameraManager;

	// Use this for initialization
	void Start () {
	if(Screen.width <= cameraManager.CameraSizeX)
		{
			gameObject.SetActive(false);
		}	
	}
}
