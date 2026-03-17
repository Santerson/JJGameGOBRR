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
    [Tooltip("The height of the grid")]
    [SerializeField] uint GridHeight = 5;
    [Tooltip("The length of the grid")]
    [SerializeField] uint GridLength = 2;
    [Tooltip("The size of each square in the grid")]
    [SerializeField] float SquareSize = 2f;
    [Tooltip("The offset of the grid off of 0,0")]
    [SerializeField] Vector2 GridOffset = Vector2.zero;
    [Tooltip("The offset of the center point of every cell")]
    [SerializeField] Vector2 GridCenterOffset = Vector2.zero;
    [Tooltip("The position of every permanently unusable space in x,y. NOTE: This is 0 based indexing (bottom left is 0,0), and also, dont use a space that is out of the grid")]
    // NOTE: in code, due to 2d arrays being y,x make sure to use this vec2 in y,x.
    [SerializeField] List<Vector2Int> InaccessableSpaces;
    [Tooltip("The length of the lines in the editor")]
    [SerializeField] float EditorLineLength = 20;

    [Header("Debug")]
    [SerializeField] bool EnableLogs = false;
    [SerializeField] bool DrawGrid = false;
    [SerializeField] GameObject RefTower1;

    SpaceStatus[,] SpaceStatuses;
    /// <summary>
    /// A 2d array containing all placed towers.
    /// null objects are empty tower spots.
    /// </summary>
    [HideInInspector] public GameObject[,] TowersInSpots { get; private set; }
    Vector2[,] SpacePositions;

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
        for (int i = 0; i < GridLength; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                // Get the position of the grid
                Vector2 Pos = new Vector2(i * SquareSize, j * SquareSize) + GridOffset + GridCenterOffset;
                // Move to the center
                Pos += new Vector2(SquareSize / 2, SquareSize / 2);
                // Store the center
                SpacePositions[i, j] = Pos;
                // See if that space is usable and update the other grid
                // Note that InaccessableSpaces is in y, x instead of x, y
                if (InaccessableSpaces.Contains(new Vector2Int(i,j)))
                {
                    SpaceStatuses[i, j] = SpaceStatus.nonexistant;
                }
                else
                {
                    SpaceStatuses[i, j] = SpaceStatus.unused;
                }
            }
        }

        DropTower(RefTower1, new Vector2Int(2, 3));
        RemoveTowerAtPosition(new Vector2Int(2, 3));
    }

    /// <summary>
    /// Draws the debug grid if the option is enabled
    /// </summary>
    private void Update()
    {
        if (DrawGrid) DrawDebugGrid();
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
            // If so, add it to the grid and update all arrays with it
            SpaceStatuses[position.x, position.y] = SpaceStatus.used;
            // Instantiate a game object there and save it's reference
            GameObject refTower = Instantiate(tower, SpacePositions[position.x, position.y], Quaternion.identity);
            TowersInSpots[position.x, position.y] = refTower;
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
        // Draw Vertical Lines
        for (int i = 0; i <= GridLength; i++)
        {
            Debug.DrawLine(new Vector2(i * SquareSize, 0) + GridOffset, new Vector2(i * SquareSize, EditorLineLength) + GridOffset);
        }

        // Draw Horizontal Lines
        for (int i = 0; i <= GridHeight; i++)
        {
            Debug.DrawLine(new Vector2(0, i * SquareSize) + GridOffset, new Vector2(EditorLineLength, i * SquareSize) + GridOffset);
        }

        // Draw Crosses
        for (int i = 0; i < GridLength; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                // Get the position of the grid
                Vector2 Pos = new Vector2(i * SquareSize, j * SquareSize) + GridOffset + GridCenterOffset;
                // Move to the center
                Pos += new Vector2(SquareSize / 2, SquareSize / 2);

                Color crossColor = Color.green;
                if (InaccessableSpaces.Contains(new Vector2Int(i, j)))
                    crossColor = Color.red;
                // Draw a cross
                Debug.DrawLine(new Vector2(Pos.x - 0.1f, Pos.y), new Vector2(Pos.x + 0.1f, Pos.y), crossColor);
                Debug.DrawLine(new Vector2(Pos.x, Pos.y - 0.1f), new Vector2(Pos.x, Pos.y + 0.1f), crossColor);
            }
        }
    }
}
