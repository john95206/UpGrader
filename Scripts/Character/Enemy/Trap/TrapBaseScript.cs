namespace Character
{

	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	public class TrapBaseScript : MonoBehaviour
	{

		public bool isActive = false;

		protected virtual void Start()
		{

		}

		protected virtual void Update()
		{

		}

		public virtual bool GetIsActive()
		{
			return isActive;
		}

		public virtual void SetIsActive(bool active)
		{
			isActive = active;
		}
	}
}