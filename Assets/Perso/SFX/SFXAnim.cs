using UnityEngine;
using System.Collections;

public class SFXAnim : MonoBehaviour
{

	public Sprite[] sprites;
	private int frame;
	private SpriteRenderer sr;
	public float time = 0.044f;
	private float timeCur;
	// Use this for initialization
	void Start () {
		frame = 0;
		timeCur = 0f;
		sr = GetComponent<SpriteRenderer> ();
	}

	public void Update()
	{
		timeCur += Time.deltaTime;
		while (timeCur > time) {
			frame++;
			timeCur -= time;
		}
		if (frame >= sprites.Length)
			sr.enabled = false;
		else
			sr.sprite = sprites [frame];
	}

}
