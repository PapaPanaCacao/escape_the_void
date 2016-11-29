using UnityEngine;
using System.Collections;

public class CameraZone : MonoBehaviour
{
	public int priority;
	public float sizeX, sizeY;
	public Vector2 offset, lookaheadFacing, lookaheadSpeed;
	public float yMin, yMax, xMin, xMax;
	public float size;
	public float pdiffX, pdiffY;

	public void Update()
	{
		CameraGame cg = CameraGame.i;

		if (!cg || cg.zone.priority >= priority)
			return;

		PersoPhysics p = PersoPhysics.i;

		if (p.transform.position.x <= transform.position.x + sizeX / 2f && p.transform.position.x >= transform.position.x - sizeX / 2f
		   && p.transform.position.y <= transform.position.y + sizeY / 2f && p.transform.position.y >= transform.position.y - sizeY / 2f)
			cg.zone = this;
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube (transform.position, new Vector3 (sizeX, sizeY, 5f));
	}
}
