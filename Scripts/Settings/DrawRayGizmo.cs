using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRayGizmo : MonoBehaviour {

	/// <summary>
	/// Draws a debug ray in 2D and does the actual raycast
	/// </summary>
	/// <returns>The raycast hit.</returns>
	/// <param name="rayOriginPoint">Ray origin point.</param>
	/// <param name="rayDirection">Ray direction.</param>
	/// <param name="rayDistance">Ray distance.</param>
	/// <param name="mask">Mask.</param>
	/// <param name="debug">If set to <c>true</c> debug.</param>
	/// <param name="color">Color.</param>
	public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
	{
		if (drawGizmo)
		{
			Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
		}
		return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
	}


	public static RaycastHit2D MonoRayCastNonAlloc(RaycastHit2D[] array, Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
	{
		if (drawGizmo)
		{
			Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
		}
		if (Physics2D.RaycastNonAlloc(rayOriginPoint, rayDirection, array, rayDistance, mask) > 0)
		{
			return array[0];
		}
		return new RaycastHit2D();
	}

	/// <summary>
	/// Draws a debug ray in 3D and does the actual raycast
	/// </summary>
	/// <returns>The raycast hit.</returns>
	/// <param name="rayOriginPoint">Ray origin point.</param>
	/// <param name="rayDirection">Ray direction.</param>
	/// <param name="rayDistance">Ray distance.</param>
	/// <param name="mask">Mask.</param>
	/// <param name="debug">If set to <c>true</c> debug.</param>
	/// <param name="color">Color.</param>
	/// <param name="drawGizmo">If set to <c>true</c> draw gizmo.</param>
	public static RaycastHit Raycast3D(Vector3 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
	{
		if (drawGizmo)
		{
			Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
		}
		RaycastHit hit;
		Physics.Raycast(rayOriginPoint, rayDirection, out hit, rayDistance, mask);
		return hit;
	}

	/// <summary>
	/// Draws a gizmo arrow going from the origin position and along the direction Vector3
	/// </summary>
	/// <param name="origin">Origin.</param>
	/// <param name="direction">Direction.</param>
	/// <param name="color">Color.</param>
	public static void GizmosDrawArrow(Vector3 origin, Vector3 direction, Color color)
	{
		float arrowHeadLength = 3.00f;
		float arrowHeadAngle = 25.0f;

		Gizmos.color = color;
		Gizmos.DrawRay(origin, direction);

		DrawArrowEnd(true, origin, direction, color, arrowHeadLength, arrowHeadAngle);
	}

	/// <summary>
	/// Draws a debug arrow going from the origin position and along the direction Vector3
	/// </summary>
	/// <param name="origin">Origin.</param>
	/// <param name="direction">Direction.</param>
	/// <param name="color">Color.</param>
	public static void DebugDrawArrow(Vector3 origin, Vector3 direction, Color color)
	{
		float arrowHeadLength = 0.20f;
		float arrowHeadAngle = 35.0f;

		Debug.DrawRay(origin, direction, color);

		DrawArrowEnd(false, origin, direction, color, arrowHeadLength, arrowHeadAngle);
	}


	private static void DrawArrowEnd(bool drawGizmos, Vector3 arrowEndPosition, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 40.0f)
	{
		if (direction == Vector3.zero)
		{
			return;
		}
		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
		Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
		Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;
		if (drawGizmos)
		{
			Gizmos.color = color;
			Gizmos.DrawRay(arrowEndPosition + direction, right * arrowHeadLength);
			Gizmos.DrawRay(arrowEndPosition + direction, left * arrowHeadLength);
			Gizmos.DrawRay(arrowEndPosition + direction, up * arrowHeadLength);
			Gizmos.DrawRay(arrowEndPosition + direction, down * arrowHeadLength);
		}
		else
		{
			Debug.DrawRay(arrowEndPosition + direction, right * arrowHeadLength, color);
			Debug.DrawRay(arrowEndPosition + direction, left * arrowHeadLength, color);
			Debug.DrawRay(arrowEndPosition + direction, up * arrowHeadLength, color);
			Debug.DrawRay(arrowEndPosition + direction, down * arrowHeadLength, color);
		}
	}

	/// <summary>
	/// Draws a gizmo sphere of the specified size and color at a position
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="size">Size.</param>
	/// <param name="color">Color.</param>
	public static void DrawGizmoPoint(Vector3 position, float size, Color color)
	{
		Gizmos.color = color;
		Gizmos.DrawWireSphere(position, size);
	}

	public static void DrawGizmoRectangle(Vector2 center, Vector2 size, Color color)
	{
		Gizmos.color = color;

		Vector3 v3TopLeft = new Vector3(center.x - size.x / 2, center.y + size.y / 2, 0);
		Vector3 v3TopRight = new Vector3(center.x + size.x / 2, center.y + size.y / 2, 0); ;
		Vector3 v3BottomRight = new Vector3(center.x + size.x / 2, center.y - size.y / 2, 0); ;
		Vector3 v3BottomLeft = new Vector3(center.x - size.x / 2, center.y - size.y / 2, 0); ;

		Gizmos.DrawLine(v3TopLeft, v3TopRight);
		Gizmos.DrawLine(v3TopRight, v3BottomRight);
		Gizmos.DrawLine(v3BottomRight, v3BottomLeft);
		Gizmos.DrawLine(v3BottomLeft, v3TopLeft);
	}
}
