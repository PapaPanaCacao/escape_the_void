using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour
{
	public GameObject emptyTile, grassTile;
	private Log log = new Log("MAP-SPAWNER");

	public void SpawnMap(Sprite colData)
	{
		log.Exec ("Map Spawn Start !");
		log.Exec ("Data : " + colData.name);
		log.Exec ("Size : " + colData.rect.width + " x " + colData.rect.height);

		Color[] tiles = colData.texture.GetPixels();

		for (int y = 0; y < colData.rect.height; y++) {
			for (int x = 0; x < colData.rect.width; x++) {
				Color t = tiles [y * (int)colData.rect.width + x];

				if (t == Color.green)
					SpawnTile (grassTile, x, y);
				else
					SpawnTile (emptyTile, x, y);
			}
		}
	}

	public void SpawnTile(GameObject prefab, int x, int y)
	{
		GameObject t = (GameObject)Instantiate (prefab);
		t.transform.SetParent (transform);
		t.transform.localPosition = x*0.5f*Vector3.right + y*0.5f * Vector3.up;
	}
}
