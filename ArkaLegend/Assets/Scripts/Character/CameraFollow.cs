using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour {
    private Transform target;

    private float xMin, xMax, yMin, yMax;

    [SerializeField]
    private Tilemap tilemap;

    private Player player;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = target.GetComponent<Player>();

        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);

        player.SetLimits(minTile, maxTile);
        SetLimits(minTile, maxTile,0.4f,0.4f);
    }
	
	// Update is called once per frame
	private void LateUpdate () {
        float currX = Mathf.Clamp(target.position.x, xMin, xMax);
        float currY = Mathf.Clamp(target.position.y, yMin, yMax);

        transform.position = new Vector3(currX, currY, -10);
	}

    private void SetLimits(Vector3 minTile, Vector3 maxTile, float xOffset, float yOffset)
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        xMin = minTile.x + width / 2 + xOffset;
        xMax = maxTile.x - width / 2 - xOffset;

        yMin = minTile.y + height / 2 + yOffset;
        yMax = maxTile.y - height / 2 - yOffset;
    }
}
