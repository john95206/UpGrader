
namespace UpGradeItem
{
	using UnityEditor;
	using UnityEngine;
	using Character;
	using GameManager;
	using Zenject;

	public class UpGradeItem : MonoBehaviour
	{
		[Inject]
		UpGradeManager upgradeManager;

		protected virtual void OnTriggerEnter2D(Collider2D col)
		{
			Player player = col.GetComponent<Player>();

			if (player != null)
			{
				UpgradePlayer(player);

				Dead();
			}
		}

		void UpgradePlayer(Player player)
		{
			ItemType managerItemType = upgradeManager.UpGradePlayer(this);

			switch (managerItemType)
			{
				case ItemType.NONE:
					Debug.Log("UPGRADE: All Abilitiy locked");
					break;
				case ItemType.UPGRADE_JUMP:
					Debug.Log("UPGRADE: Jump Unlocked");
					break;
				case ItemType.UPGRADE_GUN:
					Debug.Log("UPGRADE: Gun Unlocked");
					break;
				case ItemType.UPGRADE_FRASH:
					Debug.Log("UPGRADE: Frash Unlocked");
					break;
				case ItemType.UPGRADE_QUICK:
					Debug.Log("UPGRADE: Quick Unlocked");
					break;
				case ItemType.UPGRADE_SWORD:
					Debug.Log("UPGRADE: Sword Unlocked");
					break;
				case ItemType.UPGRADE_HP:
					Debug.Log("UPGRADE: HP Unlocked");
					break;
				case ItemType.UPGRADE_DOUBLEJUMP:
					Debug.Log("UPGRADE: DoubleJump Unlocked");
					break;
				case ItemType.UPGRADE_FLY:
					Debug.Log("UPGRADE: Fly Unlocked");
					break;
			}
			player.gotItem = managerItemType;
		}

		/// <summary>
		/// アイテムを破壊
		/// </summary>
		void Dead()
		{
			// Destroy(gameObject);
		}
	}
}
