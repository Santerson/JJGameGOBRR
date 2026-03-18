/********************************************
 * filename: UIDraggableTower.cs
 * Author: Santiago Caprarulo
 * Description: Contains the logic behind having towers that can be dragged
 * and dropped on the grid
 * ******************************************/
using UnityEngine;

public class UIDraggableTower : MonoBehaviour
{
    [Tooltip("The tower that is instantiated when this tower is placed down")]
    [SerializeField] GameObject TowerPrefab;
    [Tooltip("Sound that plays when selecting the tower")]
    [SerializeField] AudioSource pickupSound;

    TowerGrid refTowerGrid;
    Vector2 initialPosition = Vector2.zero;
    bool followingMouse = false;

    /// <summary>
    /// Sets initial variables
    /// </summary>
    private void Start()
    {
        initialPosition = transform.position;
        refTowerGrid = FindFirstObjectByType<TowerGrid>();
        if (refTowerGrid == null)
        {
            Debug.LogError("No Towergrid found! Make sure there is one on the scene");
        }
    }

    /// <summary>
    /// Force the gameobject to follow the mouse if mouse is held down starting over it
    /// </summary>
    private void Update()
    {
        // Check if the gameobject should be following the mouse
        if (followingMouse)
        {
            // Get the mouse pos
            Vector3 mousePos = Input.mousePosition;
            // Get the worldpoint
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            // Reset the z so it's visible in 2d space
            mousePos.z = 0;
            // Set the gameObject to that point
            transform.position = mousePos;
        }
    }

    /// <summary>
    /// Starts the mousedrag procedure
    /// </summary>
    private void OnMouseDown()
    {
        followingMouse = true;
        pickupSound.Play();
    }

    /// <summary>
    /// Attempts to place the tower wherever it is hovering
    /// </summary>
    private void OnMouseUp()
    {
        if (followingMouse)
            PlaceTowerAtPosition();
    }

    /// <summary>
    /// Attempts to place down the tower at the tower's current transform position
    /// </summary>
    void PlaceTowerAtPosition()
    {
        // Check (as safety) if the tower is following the mouse
        if (!followingMouse) return;
        // Nullcheck refTowergrid
        if (refTowerGrid == null) return;
        // Find the point on the TowerGrid closest to the mousePos
        Vector2Int gridTile = getClosestGridTile(transform.position);
        // Attempt to drop a tower there
        if (gridTile != new Vector2Int(-1, -1))
        {
            refTowerGrid.DropTower(TowerPrefab, gridTile);
        }
        // Stop following the mouse
        followingMouse = false;
        // Move the object back to it's original position
        transform.position = initialPosition;
    }

    /// <summary>
    /// Attempts to get the closest indexes to a position in the grid of the tower within the max range of tower placement
    /// Will return a vector2int with x and y as -1 if it could not find any within range
    /// </summary>
    /// <param name="position">A vector 2 of the position to get the nearest point in the grid</param>
    /// <returns> an index to the nearest point in the grid</returns>
    Vector2Int getClosestGridTile(Vector2 position)
    {
        // Get the positions in the grid
        Vector2[,] SpacePositions = refTowerGrid.SpacePositions;

        // Create variables to store
        Vector2Int ClosestPos = new Vector2Int(-1, -1);
        float smallestDistance = Mathf.Infinity;
        float lastPoint = Mathf.Infinity;
       
        // Get necessary variables from the towergrid
        uint gridHeight = refTowerGrid.GetGridHeight();
        uint gridLength = refTowerGrid.GetGridLength();
        float maxLen = refTowerGrid.GetLongestDistanceToBePlaced();

        // Traverse the 2d array
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                // Run distance formula between the current point and the targetted dropping position
                float distance = Vector2.Distance(transform.position, SpacePositions[i, j]);

                // If the space distance is shorter than the smallest
                if (distance < smallestDistance && distance <= maxLen)
                {
                    // Update the smallest with the new position and save the new position
                    smallestDistance = distance;
                    ClosestPos = new Vector2Int(i, j);
                }

                // If the previous space was closer than the current space
                if (distance > lastPoint)
                {
                    // Continue to the next row
                    break;
                }
                // Update last point for efficiency
                lastPoint = distance;
            }
            // Reset Last point for the next iteration
            lastPoint = Mathf.Infinity;
        }
        // Return the closest position
        return ClosestPos;
    }
}
