using UnityEngine;
using System.Collections;

public class PersoAnim : MonoBehaviour
{
	public SpriteRenderer sr;

	public ANIM stand, run, rise, fall, dash;

	public ANIM currentAnim;

	public UnityStandardAssets._2D.PlatformerCharacter2D p;

	public void Update()
	{
		if (p.animAir) {
			if (p.animUp)
				ChangeAnim (rise);
			else
				ChangeAnim (fall);
		} else {
			if (p.animRun)
				ChangeAnim (run);
			else
				ChangeAnim (stand);
		}

		currentAnim.sr.sprite = currentAnim.GetSprite ();
	}

	public void ChangeAnim(ANIM a)
	{
		if (a == currentAnim)
			return;

		currentAnim.gameObject.SetActive (false);
		currentAnim.Reset ();

		currentAnim = a;
		currentAnim.gameObject.SetActive (true);
	}
}