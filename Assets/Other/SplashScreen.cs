using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	public float timer = 5f, transitionTime = 1f;
	private float timerMax;
	public Texture splash;
	public int sceneName;

	public void Start()
	{
		timerMax = timer;
	}

	public void Update()
	{
		if (timer <= 0f)
			return;
		
		timer -= Time.deltaTime;

		if (timer <= 0f)
			SceneManager.LoadScene (sceneName);
	}

	public void OnGUI()
	{
		Rect r = new Rect (0, 0, Screen.width, Screen.height);
		float transition = Mathf.Clamp01(Mathf.Min (timer, timerMax - timer));

		GUI.color = new Color(transition, transition, transition, 1.0f);
		GUI.DrawTexture (r, splash);
		GUI.color = Color.white;

	}
}
