
namespace Character
{
	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections;
	using UniRx;
	using UniRx.Triggers;

    public class BaseEnemy : MonoBehaviour
    {
		/// <summary>
		/// ボスが死んだ時に呼び出す。
		/// </summary>
		protected virtual void BossDefeated()
		{
		}
    }
}
