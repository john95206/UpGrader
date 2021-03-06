﻿
namespace GameManager
{
	using UnityEditor;
	using UnityEngine;
	using Zenject;

    public class CameraManager : MonoBehaviour
    {
		public enum CameraType
		{
			Classic,
			Homing,
			Stop,
		};

        public CameraType cameraType = CameraType.Classic;

		const float _cameraSizeX = 1280;
		const float _cameraSizeY = 960;
		public float CameraSizeX { get { return _cameraSizeX; } private set {; } }
		public float CameraSizeY { get { return _cameraSizeY; } private set {; } }
		[System.NonSerialized]
		public float cameraSizeHalf_X = 0;
		[System.NonSerialized]
		public float cameraSizeHalf_Y = 0;
		[SerializeField]
		Camera mainCamera;
		Camera[] cameras;
		CameraScripts cameraScr;
		[Inject]
		GameManager gameManager;

		private void Start()
		{
			cameraSizeHalf_X = CameraSizeX / 2;
			cameraSizeHalf_Y = CameraSizeY / 2;
			RefreshCameraStatus();
			cameras = FindObjectsOfType<Camera>();
		}

		public void MainCameraChange()
		{
			// ChangeCamera
			RefreshCameraStatus();
		}

		void RefreshCameraStatus()
		{
			mainCamera = Camera.main;
			cameraScr = Camera.main.GetComponent<CameraScripts>();
		}

		public void GetCameraRegion()
		{

		}

		public void SetCameraType(CameraType type)
		{
			cameraType = type;
		}

		/// <summary>
		/// カメラの動きを止める
		/// </summary>
		public void DisActiveMainCamera()
		{
			SetCameraType(CameraType.Stop);
		}

		/// <summary>
		/// 画面揺れ
		/// </summary>
		/// <param name="duration"></param>
		/// <param name="strength"></param>
		/// <param name="vibrato"></param>
		/// <param name="randomness"></param>
		/// <param name="snap"></param>
		/// <param name="fadeout"></param>
		public void ShakeCaemra(float duration = 0.5f, float strength = 0.5f, int vibrato = 30, float randomness = 90, bool snap = false, bool fadeout = false)
		{
			cameraScr.ShakeCamera(duration, strength, vibrato, randomness, snap, fadeout);
		}

		private void Update()
		{
		}
	}
}
