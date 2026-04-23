/********************************************
 * filename: UIDraggableTower.cs
 * Author: Santiago Caprarulo
 * Description: Contains the logic behind having towers that can be dragged
 * and dropped on the grid
 * ******************************************/
using TMPro;
using UnityEngine;


public class UIDraggableTower : MonoBehaviour
{
    [Tooltip("The tower that is instantiated when this tower is placed down")]
        [SerializeField] public GameObject TowerPrefab;
    [Tooltip("Sound that plays when selecting the tower")]
        [SerializeField] AudioSource pickupSound;
    [SerializeField] TextMeshProUGUI refCDText;
    [SerializeField] GameObject TowerShadowPrefab;
    [SerializeField] GameObject DraggingTowerVersion;

    [Header("Timer Stuffs")]
    [Tooltip("Cooldown inbetween tower placements")]
        [SerializeField] float towerCooldown = 0;
    [SerializeField] float OpacityAtMaxTimer = 0.1f;
    [SerializeField] float OpacityAt0Timer = 1;

    [Header("Sroll Stuff")]
    [SerializeField] Vector2 TopLeftOfScroll = Vector2.zero;
    [SerializeField] Vector2 BottomRightOfScroll = Vector2.zero;
    [Tooltip("The transparency of the object when the tower is selected. Where 1 is fully opaque and 0 is fully invisible")]
        [SerializeField] float SelectedTowerTint = 0.2f;
    public bool CanDrag = true;

    TowerGrid refTowerGrid;
    SpriteRenderer refRenderer;
    AudioManager RefAudioManager;
    Color baseColor => refRenderer.color;

    Vector2 initialPosition = Vector2.zero;
    public bool followingMouse { get; set; } = false;
    float timeToNextTowerPlacement = 0;
    Vector2Int TowerShadowPosition = new Vector2Int(-1, -1);
    GameObject TowerShadow;
    GameObject InactiveTowerThatFollowsMouse = null;

    // SPaghetti time
    public float SavedLastAtkCD = 0;

    public static Vector2Int forcedNextTowerPosition { get; private set; } = new Vector2Int(-1, -1);
    static AudioManager.Towers forcedNextTower = AudioManager.Towers.bunny;

    private void OnDrawGizmosSelected()
    {
        Vector2 topRight = new Vector2(BottomRightOfScroll.x, TopLeftOfScroll.y);
        Vector2 bottomLeft = new Vector2(TopLeftOfScroll.x, BottomRightOfScroll.y);
        Debug.DrawLine(topRight, TopLeftOfScroll, Color.red);
        Debug.DrawLine(TopLeftOfScroll, bottomLeft, Color.red);
        Debug.DrawLine(bottomLeft, BottomRightOfScroll, Color.red);
        Debug.DrawLine(BottomRightOfScroll, topRight, Color.red);
    }


    /// <summary>
    /// Sets initial variables
    /// </summary>
    private void Start()
    {
        initialPosition = transform.position;
        refTowerGrid = FindFirstObjectByType<TowerGrid>();
        RefAudioManager = FindFirstObjectByType<AudioManager>();
        if (refTowerGrid == null)
        {
            Debug.LogError("No Towergrid found! Make sure there is one on the scene");
        }
        if (refCDText == null && towerCooldown > 0)
        {
            Debug.LogError("No cd text assigned!");
        }
        refRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Reduces the timer to place the next tower down and
    /// Force the gameobject to follow the mouse if mouse is held down starting over it
    /// </summary>
    private void Update()
    {
        // Reduce the time to the next tower placement if it is not 0
        timeToNextTowerPlacement = timeToNextTowerPlacement > 0 ? timeToNextTowerPlacement - Time.deltaTime : 0;
        SavedLastAtkCD = SavedLastAtkCD > 0 ? SavedLastAtkCD - Time.deltaTime : 0;
        if (timeToNextTowerPlacement > 0)
        {
            // Change the cooldown to the next tower
            refCDText.text = $"{timeToNextTowerPlacement : 0}";
            // Slowly make the sprite fade back in
            float alpha = Mathf.Lerp(OpacityAt0Timer, OpacityAtMaxTimer, timeToNextTowerPlacement / towerCooldown);
            refRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            // Play an effect once it's ready
        }
        else if (refCDText != null)
        {
            refCDText.text = "";
        }
        // Check if the gameobject should be following the mouse
        if (followingMouse)
        {
            // Get the mouse pos
            Vector3 mousePos = Input.mousePosition;
            // Get the worldpoint
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            // Reset the z so it's visible in 2d space
            mousePos.z = 0;
            if (InactiveTowerThatFollowsMouse == null)
                // Set the gameObject to that point
                transform.position = mousePos;
            else
                // Move the instantiated tower version to the mouse
                InactiveTowerThatFollowsMouse.transform.position = mousePos;

            // Check if the tower should be changed to the actual tower instead of the portrait
            // Check if the tower is outside of the range of the scroll
            if (InactiveTowerThatFollowsMouse == null && !isInRange(TopLeftOfScroll, BottomRightOfScroll, mousePos))
            {
                // If so, return and hide the portrait
                transform.position = initialPosition;
                refRenderer.color = new Color(refRenderer.color.r, refRenderer.color.g, refRenderer.color.b, SelectedTowerTint);
                // Instantiate a new gameobject
                InactiveTowerThatFollowsMouse = Instantiate(DraggingTowerVersion, transform.position, Quaternion.identity);
                // Make that object follow the mouse instead
                InactiveTowerThatFollowsMouse.transform.position = mousePos;
            }

            // Shadow a drop position for the tower
            // Get the closest drop position
            Vector2Int closestPos = getClosestGridTile(mousePos);
            // If the position is different than the one last frame
            if (closestPos != TowerShadowPosition)
            {
                // Delete any other shadows
                Destroy(TowerShadow);
                // Save the position
                TowerShadowPosition = closestPos;
                // Check if the position is non-existent, if not:
                if (closestPos != new Vector2Int(-1,-1))
                    // Check if there is a tower there 
                    if (refTowerGrid.SpaceStatuses[closestPos.x, closestPos.y] == TowerGrid.SpaceStatus.unused)
                        // Instantiate a shadow there
                        TowerShadow = Instantiate(TowerShadowPrefab, refTowerGrid.SpacePositions[closestPos.x, closestPos.y], Quaternion.identity);
            }
            // Extra check for some reason
            if (!Input.GetMouseButton(0))
                PlaceTowerAtPosition();
        }
    }

    /// <summary>
    /// Starts the mousedrag procedure
    /// </summary>
    private void OnMouseDown()
    {
        // Do nothing if the tower can't be dragged
        if (timeToNextTowerPlacement > 0 || Time.timeScale == 0 || CanDrag == false)
        {
            return;
        }
        followingMouse = true;
        RefAudioManager?.PlayTowercClickSFX(gameObject);
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
        Vector2Int gridTile = getClosestGridTile(InactiveTowerThatFollowsMouse.transform.position);
        // Attempt to drop a tower there
        if (gridTile != new Vector2Int(-1, -1))
        {
            // Don't place the tower if it is not the forced tower (usually for tutuorial)
            if (forcedNextTowerPosition != new Vector2Int(-1, -1))
            {
                if (forcedNextTower == TowerPrefab.GetComponent<TowerAi>().TowerID) 
                    if (forcedNextTowerPosition == gridTile)
                        forcedNextTowerPosition = new Vector2Int(-1, -1);
            }
            // Place a tower if is allowed to be dropped there
            if (forcedNextTowerPosition == new Vector2Int(-1, -1))
            {
                // Try to drop a tower there
                bool placed = refTowerGrid.DropTower(TowerPrefab, gridTile, SavedLastAtkCD);
                // If successfully placed
                if (placed)
                {
                    // Reset the tower's cooldown
                    timeToNextTowerPlacement = towerCooldown;
                }
                else
                    RefAudioManager?.PlayTowerFailPlacementSFX(gameObject);
            }
            else
                RefAudioManager?.PlayTowerFailPlacementSFX(gameObject);
            // Update stuff for the towergrid
            Destroy(TowerShadow);
            TowerShadowPosition = new Vector2Int(-1, -1);
        }
        else
        {
            RefAudioManager?.PlayTowerFailPlacementSFX(gameObject);
        }
        // Stop following the mouse
        followingMouse = false;
        // Destroy the instantiated tower
        Destroy(InactiveTowerThatFollowsMouse);
        InactiveTowerThatFollowsMouse = null;
        // Re-enable the sprite
        refRenderer.color = new Color(refRenderer.color.r, refRenderer.color.g, refRenderer.color.b, 1);
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
                float distance = Vector2.Distance(position, SpacePositions[i, j]);

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

    /// <summary>
    /// Forces the next tower to be placed at a specific position
    /// </summary>
    /// <param name="towerType">The type of the tower to be placed</param>
    /// <param name="Position">The position of the tower</param>
    public void ForceNextPositionTower(AudioManager.Towers towerType, Vector2Int Position)
    {
        forcedNextTower = towerType;
        forcedNextTowerPosition = Position;
    }

    /// <summary>
    /// Determines if a vector2 is in a square of other vector2s
    /// </summary>
    /// <returns>true if it is, false otherwise</returns>
    bool isInRange(Vector2 topLeft, Vector2 bottomRight, Vector2 Position)
    {
        
        if (Position.x < topLeft.x && Position.x > bottomRight.x)
            if (Position.y > bottomRight.y && Position.y < topLeft.y)
                return true;
        return false;
    }
}
