using UnityEngine;
using System.Collections;

public abstract class PersoListener : MonoBehaviour
{
	public virtual void OnJumpStart() {}
	public virtual void OnJumpEnd() {}
	public virtual void OnLanding() {}
	public virtual void OnDashStart() {}
	public virtual void OnDashEnd() {}
}
