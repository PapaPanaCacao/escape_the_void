using UnityEngine;
using System.Collections;

public class PersoAnim : MonoBehaviour
{
	public SpriteRenderer sr;

	public ANIM stand, run, rise, fall, dash;

	public ANIM currentAnim;

	public void Update()
	{
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