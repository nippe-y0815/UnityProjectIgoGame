// BoardRenderer.cs
using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    [Header("Board Appearance")]
    public Material lineMaterial;
    public float lineWidth = 0.02f;
    
    void Start()
    {
        CreateGridLines();
    }
    
    void CreateGridLines()
    {
        int boardSize = GetComponent<BoardManager>().boardSize;
        float gridSpacing = GetComponent<BoardManager>().gridSpacing;
        
        // 縦線を作成
        for (int i = 0; i < boardSize; i++)
        {
            CreateLine(
                new Vector3((i - boardSize / 2f) * gridSpacing, 0, -boardSize / 2f * gridSpacing),
                new Vector3((i - boardSize / 2f) * gridSpacing, 0, (boardSize / 2f - 1) * gridSpacing)
            );
        }
        
        // 横線を作成
        for (int i = 0; i < boardSize; i++)
        {
            CreateLine(
                new Vector3(-boardSize / 2f * gridSpacing, 0, (i - boardSize / 2f) * gridSpacing),
                new Vector3((boardSize / 2f - 1) * gridSpacing, 0, (i - boardSize / 2f) * gridSpacing)
            );
        }
    }
    
    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject line = new GameObject("GridLine");
        line.transform.SetParent(transform);
        
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        // lr.width = lineWidth;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}