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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
