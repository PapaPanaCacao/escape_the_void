using UnityEngine;
using System.Collections;

public class ANIM : MonoBehaviour
{
	public Sprite[] frames;
	public SpriteRenderer sr;

	[HideInInspector]
	public PersoGraphics pg;

	public int loopStart, loopEnd;
	public float time;
	private float frameTime = 0.0f;
	private int curFrame = 0;

	public float[] timeMultPerFrame;
	public ANIMEvent[] events;

	public ANIM nextAnim;

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
		if (curFrame == loopEnd) {
			if (nextAnim)
				pg.ChangeAnim (nextAnim);
			else
				curFrame = loopStart;
		}
		else
			curFrame++;

		ActivateFrameEvents ();
	}

	public void ActivateFrameEvents()
	{
		foreach (ANIMEvent e in events) {
			if (e.frame == curFrame)
				e.Effect (pg);
		}
	}

	public void Reset()
	{
		curFrame = 0;
		frameTime = 0.0f;
		ActivateFrameEvents ();
	}
}
