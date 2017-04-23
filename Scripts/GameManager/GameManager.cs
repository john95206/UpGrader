
namespace GameManager
{
	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections;
	using Zenject;

    public class GameManager : MonoBehaviour
    {
        public  bool IsCleared;

        public  bool IsMovieScene;

        public bool IsActivePlayer
		{ get{ return IsActivePlayer; }
			private set
			{
				if (!IsActivePlayer)
				{
					DisActiveLevel();
				}
			}
		}

		[Inject]
		InputMaster inputMaster;
		[Inject]
		CameraManager cameraManager;
		LevelManager levelManager = null;
		UpGradeManager upgradeManager = null;

		/// <summary>
		/// プレイヤーの死を受けてステージ全体を喪に服す
		/// </summary>
		void DisActiveLevel()
		{
			// カメラを止める
			cameraManager.DisActiveMainCamera();
			// 操作をプレイヤー操作でなくする
			inputMaster.IsActivePlayer = false;
		}

		/// <summary>
		/// プレイヤーが死んだことをGameManagerが感知
		/// </summary>
		/// <param name="player"></param>
		public void DisActivePlayer(Character.Player player)
		{
			if (!player.IsActive)
			{
				Debug.Log("Im Still Alive!!!!");
			}else
			{
				IsActivePlayer = true;
			}
		}
    }
}
