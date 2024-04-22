using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edit : MonoBehaviour
{
    [SerializeField]
    private Transform tileParent;
    [SerializeField]
    private GameObject[] tilesPrefab;
    [SerializeField]
    private GameObject currentTile;

    private int currentIndexTile;
    private Quaternion currentRotation;

    public void Rotate(int direction)
    {
        if (currentTile == null) currentTile = tileParent.GetChild(0).gameObject;
        currentTile.transform.Rotate(Vector3.up, 90f * direction);
        currentRotation = currentTile.transform.rotation;

        if (transform.parent.GetComponent<Tile>().TileInformations.Type == TileType.Road) 
            currentTile.GetComponentInChildren<Road>().ReconnectThisRoad();
    }

    public void ChangeTile()
    {
        if (currentTile == null) currentTile = tileParent.GetChild(0).gameObject;
        currentIndexTile++;
        if (currentIndexTile > tilesPrefab.Length - 1) currentIndexTile = 0;
        GameObject newTile = tilesPrefab[currentIndexTile];

        newTile = Instantiate(newTile, currentTile.transform.position, currentRotation, tileParent);
        Destroy(currentTile);
        currentTile = newTile;
        if (transform.parent.GetComponent<Tile>().TileInformations.Type == TileType.Road)
            currentTile.GetComponentInChildren<Road>().ReconnectThisRoad();
    }
}
