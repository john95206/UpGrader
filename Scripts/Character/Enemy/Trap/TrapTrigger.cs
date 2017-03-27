using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrapTrigger : MonoBehaviour {

	[SerializeField]
	UnityEvent callback;
	[SerializeField]
	string targetTag;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(targetTag == string.Empty)
		{
			targetTag = TermDefinition.Instance.PlayerTag;
		}

		if(collision.gameObject.tag == targetTag)
		{
			callback.Invoke();
			Destroy(gameObject);
		}
	}
}
