
namespace GameManager
{
	using UnityEngine;
	using UnityEditor;
	using UniRx;
	using UpGradeItem;
	using Character;

	public class UpGradeManager : MonoBehaviour
	{
		[SerializeField]
		ItemType nowPlayerUpgrade = ItemType.NONE;
		[SerializeField]
		ItemType nowUpgradeItem = ItemType.NONE;

		/// <summary>
		/// TODO: セーブデータからロードする
		/// </summary>
		private void Start()
		{
			nowPlayerUpgrade = ItemType.NONE;
			nowUpgradeItem = ItemType.UPGRADE_JUMP;
		}

		/// <summary>
		/// アイテム取得時にアイテムから呼び出される
		/// </summary>
		/// <param name="item"></param>
		public ItemType UpGradePlayer(UpGradeItem item)
		{
#if UNITY_EDITOR
			// 未取得アイテムのとき
			if (nowPlayerUpgrade < nowUpgradeItem)
			{

			}
			// ダウングレードアイテム
			else if (nowPlayerUpgrade > nowUpgradeItem)
			{
				Debug.Log("DownGrade...");
			}
			// 前と同じアイテム
			else
			{
				Debug.Log("Allready Up to Date!!!");
			}
#endif
			nowPlayerUpgrade = nowUpgradeItem;
			// TODO: SAVE
			nowUpgradeItem += 1;
			// TODO: SAVE

			return nowUpgradeItem;
		}
	}
}