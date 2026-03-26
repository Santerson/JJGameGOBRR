/********************************************
 * filename: TowerGrid.cs
 * Author: Santiago Caprarulo
 * Description: Contains all logic and setup behind the grid system
 * used for the tower dropping
 * ******************************************/

using UnityEngine;
using System.Collections.Generic;


public class TowerGrid : MonoBehaviour
{
    [Header("GridSize")]
    // Gridsizes
    [Tooltip("The height of the grid")]
        [SerializeField] uint GridHeight = 5;
    [Tooltip("The length of the grid")]
        [SerializeField] uint GridLength = 2;
    [Tooltip("The size of each square in the grid")]
        [SerializeField] float SquareSize = 2f;

    // Space Buffers
    [Tooltip("The space inbetween each space vertically")]
        [SerializeField] float VerticalSpaceBuffer = 0;
    [Tooltip("The space inbetween each space horizontally")]
        [SerializeField] float HorizontalSpaceBuffer = 0;

    // Grid Offset
    [Tooltip("The offset of the grid off of 0,0")]
        [SerializeField] Vector2 GridOffset = Vector2.zero;
    [Tooltip("The offset of the center point of every cell")]
        [SerializeField] Vector2 GridCenterOffset = Vector2.zero;
    
    // Special spaces
    [Tooltip("The position of every permanently unusable space in x,y. NOTE: This is 0 based indexing (bottom left is 0,0), and also, dont use a space that is out of the grid")]
        [SerializeField] List<Vector2Int> InaccessableSpaces;

    [Header("Max Distances")]
    // Max Distances
    [Tooltip("When placing towers, the distance from where the tower is dropped and the nearest grid must be less than this number in order for it to be successfully dropped.")]
        [SerializeField] float longestDistanceToBePlaced = 2f;
    [Tooltip("The minimum distance the tower must be away from an enemy to be placed")]
    [SerializeField] float MinDistanceAwayFromEnemies = 1f;

    [Header("Debug")]
        [SerializeField] bool EnableLogs = false;
        [SerializeField] bool DrawGridInGame = false;
        [SerializeField] bool AlwaysDrawGrid = false;
        [SerializeField] GameObject RefTower1;
    
    // An array of the SpaceStatus enum to contain the status of each space
    public SpaceStatus[,] SpaceStatuses { get; private set; }
    /// <summary>
    /// A 2d array containing all placed towers.
    /// null objects are empty tower spots.
    /// </summary>
    [HideInInspector] public GameObject[,] TowersInSpots { get; private set; }
    /// <summary>
    /// A 2d array containing positions for every space in the grid
    /// That can have a tower. Use this to run the functions "drop tower"
    /// "remove tower at location"
    /// </summary>
    public Vector2[,] SpacePositions { get; private set; }

    /// <summary>
    /// Contains different statuses for each space slot
    /// </summary>
    public enum SpaceStatus
    {
        /// <summary>
        /// There does not exist a spot at this point in the grid, ever
        /// </summary>
        nonexistant,
        /// <summary>
        /// An empty point in the grid that can have a tower dropped in it
        /// </summary>
        unused,
        /// <summary>
        /// An occupied point in the grid that currently has a tower in it
        /// </summary>
        used
    }

    /// <summary>
    /// Draws lines on the unity editor depicting the size of the grid
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        DrawDebugGrid();
    }

    private void OnDrawGizmos()
    {
        if (AlwaysDrawGrid) DrawDebugGrid();
    }

    /// <summary>
    /// Creates the lists to store the grids
    /// Also sets up the grid positions
    /// </summary>
    private void Start()
    {
        SpaceStatuses = new SpaceStatus[GridLength, GridHeight];
        TowersInSpots = new GameObject[GridLength, GridHeight];
        SpacePositions = new Vector2[GridLength, GridHeight];

        // Fill the space positions in the grid
        for (int x = 0; x < GridLength; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                // Calculate additional offsets
                float xAdditionalOffset = x * HorizontalSpaceBuffer;
                float yAdditionalOffset = y * VerticalSpaceBuffer;
                // Get the position of the grid
                Vector2 Pos = new Vector2(x * SquareSize + xAdditionalOffset, y * SquareSize + yAdditionalOffset) + GridOffset + GridCenterOffset;
                // Move to the center
                Pos += new Vector2(SquareSize / 2, SquareSize / 2);
                // Store the center
                SpacePositions[x, y] = Pos;
                // See if that space is usable and update the other grid
                // Note that InaccessableSpaces is in y, x instead of x, y
                if (InaccessableSpaces.Contains(new Vector2Int(x,y)))
                {
                    SpaceStatuses[x, y] = SpaceStatus.nonexistant;
                }
                else
                {
                    SpaceStatuses[x, y] = SpaceStatus.unused;
                }
            }
        }
    }

    /// <summary>
    /// Draws the debug grid if the option is enabled
    /// </summary>
    private void Update()
    {
        if (DrawGridInGame) DrawDebugGrid();
    }


    /// <summary>
    /// Try to place a tower at a given position
    /// </summary>
    /// <param name="tower">A PREFAB of the object you are trying to instantiatew</param>
    /// <param name="position">A position to place the tower in gridscale, in row col. NOTE: should be a vector2Int, using a different constructor.
    ///      SO CALL THAT ONE. >:( </param>
    /// <return>true if the tower was successfully placed, false otherwise</return>"
    public bool DropTower(GameObject tower, Vector2Int position)
    {
        // Check if there is a open slot (SpaceStatus = unused) at this point
        SpaceStatus refSpace = SpaceStatuses[position.x, position.y];
        if (refSpace == SpaceStatus.unused)
        {
            // Check if there is currently an enemy in the position
            EnemyAi[] enemies = FindObjectsByType<EnemyAi>(FindObjectsSortMode.None);
            foreach (EnemyAi enemy in enemies)
            {
                if (Vector2.Distance(enemy.transform.position, SpacePositions[position.x, position.y]) < MinDistanceAwayFromEnemies)
                {
                    return false;
                }
            }

            // If so, add it to the grid and update all arrays with it
            SpaceStatuses[position.x, position.y] = SpaceStatus.used;
            // Instantiate a game object there and save it's reference
            GameObject refTower = Instantiate(tower, SpacePositions[position.x, position.y], Quaternion.identity);
            TowersInSpots[position.x, position.y] = refTower;
            // Save the position in the towerai script of the tower
            refTower.GetComponent<TowerAi>().GridPosition = new Vector2Int(position.x, position.y);
            // return true
            if (EnableLogs) Debug.Log($"Placed a tower at {position.x}, {position.y}.");
            return true;
        }
        // otherwise, return false
        if (EnableLogs) Debug.Log($"Failed to place a tower at {position.x}, {position.y}.");
        return false;
    }

    /// <summary>
    /// Tries to remove a tower at a given position. Should it find a gameobject to remove, the object will be destroyed (if the
    /// parameter destroy is true), or will have it returned. if the object is destroyed, the function returns null.
    /// </summary>
    /// <param name="position">A position of the tower to remove (in grid scale, 0 based indexing)</param>
    /// <param name="destroy">Should the gameobject be destroyed on removal</param>
    /// <returns>The gameobject removed, or null if none was removed. NOTE: if destroy is on, this function will ALWAYS return null</returns>
    public GameObject RemoveTowerAtPosition(Vector2Int position, bool destroy = true)
    {
        // Check if there is no tower in postion
        if (SpaceStatuses[position.x,position.y] != SpaceStatus.used)
        {
            if (EnableLogs) Debug.Log($"Attempted to remove a tower at {position.x}, {position.y}, but couldn't as there are no towers there");
            return null;
        }

        // Remove the tower and get a reference to it
        GameObject removedObj = TowersInSpots[position.x, position.y];
        TowersInSpots[position.x, position.y] = null;

        // Remove it from other arrays
        SpaceStatuses[position.x, position.y] = SpaceStatus.unused;

        // Check if to remove it
        if (destroy)
        {
            Destroy(removedObj);
            if (EnableLogs) Debug.Log($"Destroyed a tower at {position.x}, {position.y}.");
            return null;
        }
        // Otherwise, return it
        if (EnableLogs) Debug.Log($"Removed a tower at {position.x}, {position.y}.");
        return removedObj;
    }

    /// <summary>
    /// Draws a debug grid for the towers and map. This is only visible on selection of the gameobject
    /// or if the option is enabled in the inspector. This is only visible with the gizmos on and does
    /// not impact gameplay at all.
    /// </summary>
    private void DrawDebugGrid()
    {
        // Draw Crosses
        for (int x = 0; x < GridLength; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                // Calculate additional offsets by the horizontal and vertical space buffers
                float xAdditionalOffset = x * HorizontalSpaceBuffer;
                float yAdditionalOffset = y * VerticalSpaceBuffer;
                // Get the position of the grid
                Vector2 Pos = new Vector2(x * SquareSize + xAdditionalOffset, y * SquareSize + yAdditionalOffset) + GridOffset + GridCenterOffset;
                // Move to the center
                Pos += new Vector2(SquareSize / 2, SquareSize / 2);

                Color crossColor = Color.green;
                if (InaccessableSpaces.Contains(new Vector2Int(x, y)))
                    crossColor = Color.red;
                // Draw a cross
                Debug.DrawLine(new Vector2(Pos.x - 0.1f, Pos.y), new Vector2(Pos.x + 0.1f, Pos.y), crossColor);
                Debug.DrawLine(new Vector2(Pos.x, Pos.y - 0.1f), new Vector2(Pos.x, Pos.y + 0.1f), crossColor);
            }
        }
    }

    /// <summary>
    /// Returns the Grid's length
    /// </summary>
    /// <returns>uint of the grid length</returns>
    public uint GetGridLength()
    {
        return GridLength;
    }

    /// <summary>
    /// Returns the Grid's Height
    /// </summary>
    /// <returns>uint of the grid height</returns>
    public uint GetGridHeight()
    {
        return GridHeight;
    }

    /// <summary>
    /// Returns the longest distance a tower can be from a square for it to still be placed down
    /// </summary>
    /// <returns>float of the longest distance</returns>
    public float GetLongestDistanceToBePlaced()
    {
        return longestDistanceToBePlaced;
    }
}
