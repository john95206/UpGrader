using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内で使う文字列
/// </summary>
public class TermDefinition {

	static TermDefinition term;

	TermDefinition()
	{
#if UNITY_EDITOR
		Debug.Log("===Create TermDefinition===");
#endif
	}

	// インスタンス生成
	public static TermDefinition Instance
	{
		get
		{
			if(term == null)
			{
				term = new TermDefinition();
			}
			return term;
		}
	}

	// === Tag ===
	// Untagged
	public string Untagged { get { return "Untagged"; } }
	// EditorOnly
	public string EditorOnly { get { return "EditorOnly"; } }
	// Respawn
	public string Respawn { get { return "Respawn"; } }
	// Finish
	public string Finish { get { return "Finish"; } }
	// Main Camera
	public string MainCameraTag { get { return "Main Camera"; } }
	// Player
	public string PlayerTag { get { return "Player"; } }
	// GameController
	public string GameController { get { return "GameController"; } }
	// Stage
	public string StageTag { get { return "Stage"; } }
	// Wall
	public string WallTag { get { return "Wall"; } }

	// === LayerMaskName ===
	// Default
	public string DefaultLayer { get { return "Default"; } }
	// TransparentFX
	public string TransparentFX { get { return "TransparentFX"; } }
	// Ignore Raycast
	public string IgnoreRayCast { get { return "Ignore Raycast"; } }
	// Water
	public string WaterLayer { get { return "Water";} }
	// OnlyGround
	public string OnlyGroundLayer { get { return "OnlyGround"; } }
	// GroundAndEvent
	public string GroundAndEventLayer { get { return "GroundAndEvent"; } }
	// Ground
	public string GroundLayer { get { return "Ground"; } }
	// Character
	public string CharacterLayer { get { return "Character"; } }
	// Wall
	public string WallLayer { get { return "Wall"; } }

	// === SceneName ===
	// Test
	public string TestScene { get { return "Test"; } }

	// === ObjectName ===
	// Main Camera ===
	public string MainCameraName { get { return "Main Camera"; } }
}
