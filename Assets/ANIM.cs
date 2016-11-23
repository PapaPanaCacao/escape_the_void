using UnityEngine;
using System.Collections;

public class ANIM : MonoBehaviour
{
	public Sprite[] frames;
	public SpriteRenderer sr;

	public int loopStart, loopEnd;
	public float time;
	private float frameTime = 0.0f;
	private int curFrame = 0;

	public Sprite GetSprite()
	{
		frameTime += Time.deltaTime;
		while (frameTime > time) {
			frameTime -= time;
			NextFrame ();
		}

		return GetCurFrame ();
	}

	public Sprite GetCurFrame()
	{
		return frames [curFrame];
	}

	private void NextFrame()
	{
		if (curFrame == loopEnd)
			curFrame = loopStart;
		else
			curFrame++;
	}

	public void Reset()
	{
		curFrame = 0;
		frameTime = 0.0f;
	}
}
