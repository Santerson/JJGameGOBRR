using UnityEngine;
using System.Collections.Generic;


public class TowerGrid : MonoBehaviour
{
    [Header("GridSize")]
    [SerializeField] float SquareSize = 2f;
    [SerializeField] Vector2 GridOffset = Vector2.zero;
    [SerializeField] float EditorLineLength = 20;
    [SerializeField] GameObject RefTower1;

    List<List<SpaceStatus>> spaceStatuses = new List<List<SpaceStatus>>();
    List<List<GameObject>> objectsInSpaces = new List<List<GameObject>>();
    List<List<Vector2>> spacePositions = new List<List<Vector2>>();

    public enum SpaceStatus
    {
        nonexistant,
        unused,
        used
    }

    /// <summary>
    /// Draws lines on the unity editor depicting the size of the grid
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Draw horizontal lines
        for (float i = -5 * SquareSize; i < 5 * SquareSize; i += SquareSize)
        {
            Debug.DrawLine(new Vector2(-EditorLineLength / 2, i) + GridOffset, new Vector2(EditorLineLength / 2, i) + GridOffset);
            Debug.DrawLine(new Vector2(i, -EditorLineLength / 2) + GridOffset, new Vector2(i, EditorLineLength / 2) + GridOffset);
        }
    }

    /// <summary>
    /// Try to place a tower at a given position
    /// </summary>
    /// <param name="tower">A PREFAB of the object you are trying to instantiatew</param>
    /// <param name="position">A position to place the tower in gridscale, in row col</param>
    /// <return>true if the tower was successfully placed, false otherwise</return>"
    public bool DropTower(GameObject tower, Vector2Int position)
    {
        // Check if there is a open slot (SpaceStatus = unused) at this point
        SpaceStatus refSpace = spaceStatuses[position.x][position.y];
            // If so, add it to the grid and update all arrays with it
            // Instantiate a game object there
            // Save a reference to that object in the third 2d array
            // return true
        // otherwise, return false
        return false;
    }
}
