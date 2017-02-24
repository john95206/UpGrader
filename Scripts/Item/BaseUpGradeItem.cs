
namespace UpGradeItem
{
	using UnityEditor;
	using UnityEngine;

    public class BaseUpGradeItem : MonoBehaviour
    {
		[SerializeField]
		ItemType itemType;

		protected virtual void OnTriggerEnter2D(Collider2D col)
		{
			if(col.tag == "Player")
			{
				col.GetComponent<Character.Player>().gotItem = itemType;

				Dead();
			}
		}

		void Dead()
		{
			Destroy(gameObject);
		}
    }
}
