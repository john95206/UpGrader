using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{

	public interface ICharacter
	{
		bool IsActive { get; set; }
		int Hp { get; set; }
		int AttackDamage { get; set; }
		float MoveSpeed { get; set; }
		float JumpPower { get; set; }
		void Attack();
		void Move();
		void Damage();
		void Dead();
	}
}
