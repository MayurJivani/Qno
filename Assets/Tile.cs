using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 TilePosition { get; private set; } // The position of the tile.
    public bool IsAccessible { get; private set; }    // Whether the tile is accessible or not.

    [SerializeField]
    private int tileIndex = -1;  // An index to identify each tile (optional).

    // You can also add other properties or variables specific to your game.

    private void Start()
    {
        // Initialize the properties when the tile is created.
        TilePosition = transform.position;
        IsAccessible = true; // You can set this based on your game's rules.
    }

    public int TileIndex
    {
        get { return tileIndex; }
        private set { tileIndex = value; }
    }
}
