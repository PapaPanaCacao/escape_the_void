using UnityEngine;
using System.Collections;

public class PersoListenerMultiplex : PersoListener
{
	public PersoListener[] listeners;

	public override void OnJumpStart()
	{
		foreach (PersoListener l in listeners)
			l.OnJumpStart ();
	}

	public override void OnJumpEnd()
	{
		foreach (PersoListener l in listeners)
			l.OnJumpEnd ();
	}

	public override void OnLanding()
	{
		foreach (PersoListener l in listeners)
			l.OnLanding ();
	}
}
