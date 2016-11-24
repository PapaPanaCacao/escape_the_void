using UnityEngine;
using System.Collections;

public class ANIMEventPrefab : ANIMEvent
{
	public GameObject prefab;
	public Vector3 offset;

	public override void Effect (PersoGraphics pg)
	{
		GameObject go = (GameObject)Instantiate (prefab, pg.p.transform.position + offset, Quaternion.identity);
		go.transform.localScale += (pg.p.faceRight ? Vector3.zero : Vector3.left * 2f);
	}
}
