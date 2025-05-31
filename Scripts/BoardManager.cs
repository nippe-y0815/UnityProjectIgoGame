// BoardManager.cs
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    public int boardSize = 19;
    public float gridSpacing = 1f;
    public GameObject intersectionPrefab;
    public Material boardMaterial;
    
    private GameObject[,] intersections;
    private int[,] boardState; // 0: 空, 1: 黒, 2: 白
    
    void Start()
    {
        CreateBoard();
        InitializeBoardState();
    }
    
    void CreateBoard()
    {
        intersections = new GameObject[boardSize, boardSize];
        
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                Vector3 position = new Vector3(
                    (x - boardSize / 2f) * gridSpacing,
                    0,
                    (z - boardSize / 2f) * gridSpacing
                );
                
                GameObject intersection = Instantiate(intersectionPrefab, position, Quaternion.identity);
                intersection.transform.SetParent(transform);
                intersection.name = $"Intersection_{x}_{z}";
                
                // IntersectionControllerコンポーネントを追加
                IntersectionController controller = intersection.GetComponent<IntersectionController>();
                controller.SetPosition(x, z);
                
                intersections[x, z] = intersection;
            }
        }
    }
    
    void InitializeBoardState()
    {
        boardState = new int[boardSize, boardSize];
    }
    
    public IntersectionController GetIntersection(int x, int z)
    {
        if (x >= 0 && x < boardSize && z >= 0 && z < boardSize)
        {
            return intersections[x, z].GetComponent<IntersectionController>();
        }
        return null;
    }
    
    public int[,] GetBoardState()
    {
        return boardState;
    }
    
    public void SetBoardState(int x, int z, int value)
    {
        if (x >= 0 && x < boardSize && z >= 0 && z < boardSize)
        {
            boardState[x, z] = value;
        }
    }
    
    public void ResetBoard()
    {
        // 盤面の状態をリセット
        boardState = new int[boardSize, boardSize];
        
        // 全ての石を削除
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                IntersectionController controller = intersections[x, z].GetComponent<IntersectionController>();
                controller.RemoveStone();
            }
        }
    }
}