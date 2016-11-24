using UnityEngine;
using System.Collections;

public class PersoGraphics : PersoListener
{
	public PersoPhysics p;

	[HideInInspector]
	public Vector3 baseScale = Vector3.one;
	public ANIM stand, run, rise, fall, dash, land;

	public ANIM currentAnim;

	[HideInInspector]
	public bool animAir, animUp, animRun;

	public void Awake()
	{
		baseScale = transform.localScale;
		currentAnim.pg = this;
	}

	public void Update()
	{
		transform.localScale = baseScale + (p.faceRight ? Vector3.zero : Vector3.left * 2f * baseScale.x);

		animRun = Mathf.Abs(p.velocity.x) > 0.2f;
		animUp = p.velocity.y > 0.0f;


		ChooseAnim ();


		currentAnim.sr.sprite = currentAnim.GetSprite ();
	}

	public void ChooseAnim()
	{
		if (p.dashInHeld) {
			ChangeAnim (dash);
			return;
		}


		if (animAir) {
			if (animUp)
				ChangeAnim (rise);
			else
				ChangeAnim (fall);
		} else {
			if (animRun)
				ChangeAnim (run);
			else if(currentAnim != land)
				ChangeAnim (stand);
		}
	}

	public void ChangeAnim(ANIM a)
	{
		if (a == currentAnim)
			return;

		currentAnim.gameObject.SetActive (false);

		currentAnim = a;
		currentAnim.pg = this;
		currentAnim.Reset ();
		currentAnim.gameObject.SetActive (true);
	}

	public override void OnJumpStart()
	{
		animAir = true;
	}

	public override void OnLanding()
	{
		animAir = false;
		ChangeAnim (land);
	}
}
