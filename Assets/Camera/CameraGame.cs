using UnityEngine;
using System.Collections;

// Maybe try lerping individual offsets
public class CameraGame : MonoBehaviour
{
	[HideInInspector]
	public Camera cam;

	public static CameraGame i;

	[HideInInspector]
	public CameraZone zone, zoneBase;

	public float lerpCoefSize, lerpCoefMov;

	private float yPLastGrounded;

	public void Awake()
	{
		i = this;
		cam = GetComponent<Camera> ();
		zoneBase = GetComponent<CameraZone> ();
		zone = zoneBase;
	}

	public void Start()
	{
		yPLastGrounded = PersoPhysics.i.transform.position.y;
	}

	public void LateUpdate()
	{
		Vector2 targetPosition = Vector2.zero;
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zone.size / 2f, Time.deltaTime*lerpCoefSize);
		Vector3 persoPos = PersoPhysics.i.transform.position;

		// Y Position in relation to the ground
		if (PersoPhysics.i.grounded)
			yPLastGrounded = persoPos.y;

		// X position + offsets (fixed, facing and speed)
		targetPosition = new Vector2 (persoPos.x, yPLastGrounded) + zone.offset;

		if (PersoPhysics.i.faceRight)
			targetPosition += zone.lookaheadFacing;
		else
			targetPosition += new Vector2 (-zone.lookaheadFacing.x, zone.lookaheadFacing.y);

		targetPosition += new Vector2 (PersoPhysics.i.velocity.x * zone.lookaheadSpeed.x, PersoPhysics.i.velocity.y * zone.lookaheadSpeed.y);

		// Is the character still in the camera ?
		if (Mathf.Abs (targetPosition.x - persoPos.x) > zone.pdiffX)
			targetPosition.x = persoPos.x + zone.pdiffX * Mathf.Sign (targetPosition.x - persoPos.x);
		if (Mathf.Abs (targetPosition.y - persoPos.y) > zone.pdiffY)
			targetPosition.y = persoPos.y + zone.pdiffY * Mathf.Sign (targetPosition.y - persoPos.y);

		// Final Operations
		targetPosition = new Vector2 (Mathf.Clamp (targetPosition.x, zone.xMin, zone.xMax), Mathf.Clamp (targetPosition.y, zone.yMin, zone.yMax));
		Vector3 finalPos = new Vector3 (targetPosition.x, targetPosition.y, transform.position.z);

		transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime*lerpCoefMov);

		zone = zoneBase;
	}
}
