
namespace GameManager
{
	using UnityEditor;
	using UnityEngine;
	using Zenject;
	using UnityEngine.SceneManagement;

    public class LevelManager : MonoBehaviour
    {
		public string NowScene;

        //public  LevelStart levelStart;

        //public  LevelGoal levelGoal;

		[Inject]
		InputMaster inputMaster;

		private void Start()
		{
			SetSceneName(SceneManager.GetActiveScene().name);
		}

		private void SetSceneName(string sceneName)
		{
			for(int i = 0; i < SceneManager.sceneCount; i++)
			{
				if(sceneName == SceneManager.GetSceneAt(i).name)
				{
					NowScene = sceneName;
					return;
				}
			}
			NowScene = "_Initialize";
		}

		private void Update()
		{
			if (inputMaster.JoyPadCheck(INPUT_TYPE.RETRY))
			{
				SceneManager.LoadScene(NowScene);
			}
		}
	}
}
