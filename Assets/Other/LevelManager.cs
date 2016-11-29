using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public static LevelManager i;
	public LevelSpawner spawner;

	public Sprite colData, entietyData;
	public MeshRenderer preview;

	public bool drawLevelGizmo = true;

	public void Awake()
	{
		i = this;
		spawner.SpawnMap (colData);
	}

	public void OnDrawGizmos()
	{
		if (!drawLevelGizmo)
			return;

		/*if (preview) {
			preview.material.mainTexture = colData.texture;
			preview.transform.localScale = Vector3.forward + Vector3.right * colData.rect.width / 20f + (Vector3.up * colData.rect.height / 20f);
			preview.transform.localPosition = Vector3.right * colData.rect.width / 4f + (Vector3.up * (colData.rect.height / 4f + 0.5f)) + Vector3.forward * 5;
		}*/

		Gizmos.color = Color.green;
		Color[] tiles = colData.texture.GetPixels();

		for (int y = 0; y < colData.rect.height; y++) {
			for (int x = 0; x < colData.rect.width; x++) {
				Color t = tiles [y * (int)colData.rect.width + x];

				if (t == Color.green)
					Gizmos.DrawWireCube (new Vector3 (x * 0.5f + 0.25f, y * 0.5f + 0.25f, 0f), new Vector3 (0.5f, 0.5f, 0.5f));
			}
		}

	}
}
