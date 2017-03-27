
namespace GameManager
{
	using UnityEditor;
	using UnityEngine;
	using Zenject;

    public class CameraManager : MonoBehaviour
    {
		public enum CameraType { };

        public CameraType GetCameraType;

		[SerializeField]
		Camera mainCamera;
		CameraScripts cameraScr;

		private void Start()
		{
			RefreshCameraStatus();
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

		public void SetCameraType()
		{
			
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
