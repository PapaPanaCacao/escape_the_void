using UnityEngine;
using System.Collections;

public class TimeBomb : MonoBehaviour
{
	public float timer = 1f;

	public void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0f)
			Destroy (gameObject);
	}
}
