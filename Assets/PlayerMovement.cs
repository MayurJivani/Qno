using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f; // Adjust the speed as needed.
    private int currentTileIndex = 0; // Starting tile index.
    private Tile[] tiles; // Array to store all tiles.

    private void Start()
    {
        // Populate the 'tiles' array with all tile GameObjects.
        tiles = FindObjectsOfType<Tile>().OrderBy(tile => tile.TileIndex).ToArray();
        for (int i = 0; i < tiles.Length; i++)
        {
            Debug.Log(tiles[i].TileIndex);
        }
    }
    
    public void MoveToNextTile()
    {
        // Check if there is a next tile to move to.
        Debug.Log("Current Tile Index:" + currentTileIndex);
        Debug.Log("Total Tiles:" + tiles.Length);
        if (currentTileIndex < tiles.Length - 1)
        {
            ++currentTileIndex;
            Transform nextTileTransform = tiles[currentTileIndex].transform;
            Debug.Log(nextTileTransform.position);
            // Move the player to the next tile.
            transform.position = nextTileTransform.position + new Vector3(0,0.5f,0);
        }
    }
}
