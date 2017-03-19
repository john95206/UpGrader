
namespace GameManager
{
	using InControl;
	using UnityEditor;
	using UnityEngine;
	using UniRx;
	using UniRx.Triggers;
	using Zenject;

	public enum INPUT_TYPE
	{
		LEFT,
		RIGHT,
		UP,
		DOWN,
		ATTACK,
		JUMP,
		SUBMIT,
		RETRY,
	}

	public class InputMaster : MonoBehaviour
    {
		
		InputDevice device;

		public INPUT_TYPE inputType;

		private void Start()
		{
			device = InputManager.ActiveDevice;

			var InputStream = this.UpdateAsObservable()
				.Where(_ => CanPlayerMove())
				.Select(x => x)
				.Publish()
				.RefCount();
		}

		bool CanPlayerMove()
		{
			//if (!playerCtrl.activeSts || playerCtrl.activeEvent || FadeManager.Instance.SceneLoad ||
			//	playerCtrl.hirumi || !playerCtrl.narrativeCheck)
			//{
			//	return false;
			//}

			return true;
		}

		/// <summary>
		/// 入力のチェック。コントローラーがないならキーボードで検知
		/// </summary>
		/// <param name="commandName"></param>
		/// <returns></returns>
		public bool JoyPadCheck(INPUT_TYPE commandName)
		{
			device = InputManager.ActiveDevice;
			float x = Input.GetAxisRaw("Horizontal");
			float y = Input.GetAxisRaw("Vertical");

			switch (commandName)
			{
				case INPUT_TYPE.LEFT:
					if (device.DPadLeft)
					{
						return true;
					}

					if (x == -1)
					{
						return true;
					}
					break;
				case INPUT_TYPE.RIGHT:
					if (device.DPadRight)
					{
						return true;
					}

					if(x == 1)
					{
						return true;
					}
					break;

				case INPUT_TYPE.ATTACK:
					if (device.Action2.WasPressed)
					{
						return true;
					}
					if (Input.GetButtonDown("Fire1"))
					{
						return true;
					}

					break;

				case INPUT_TYPE.JUMP:
					if (device.Action4.WasPressed)
					{
						return true;
					}

					if (Input.GetButtonDown("Jump"))
					{
						return true;
					}
					break;

				case INPUT_TYPE.SUBMIT:
					if (device.Action4.WasPressed)
					{
						return true;
					}

					if (Input.GetButtonDown("Submit"))
					{
						return true;
					}
					break;
				case INPUT_TYPE.RETRY:
					if (device.MenuWasPressed)
					{
						return true;
					}

					if (Input.GetButtonDown("Retry"))
					{
						return true;
					}
					break;
			}

			return false;
		}
	}
}
