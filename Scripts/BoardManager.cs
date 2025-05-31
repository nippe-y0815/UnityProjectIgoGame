// BoardManager.cs
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]

    //盤面のサイズ
    public int boardSize = 19;

    //格子の間隔
    public float gridSpacing = 1f;
    public GameObject intersectionPrefab;
    public Material boardMaterial;
    
    //各交点オブジェクト
    private GameObject[,] intersections;

    //盤面の状態
    private int[,] boardState; // 0: 空, 1: 黒, 2: 白
    
    void Start()
    {
        CreateBoard();
        InitializeBoardState();
    }
    
    //盤面を生成する処理
    // ボード（碁盤など）の格子点を生成するメソッド
    void CreateBoard()
    {
        // 格子点を格納する2次元配列を初期化（サイズ：boardSize × boardSize）
        intersections = new GameObject[boardSize, boardSize];
        
        // x軸方向とz軸方向にループして格子点を生成
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                // 中心を原点とした位置を計算（碁盤の中心が (0, 0, 0) になるように）
                Vector3 position = new Vector3(
                    (x - boardSize / 2f) * gridSpacing,  // x座標
                    0,                                   // y座標（地面に設置）
                    (z - boardSize / 2f) * gridSpacing   // z座標
                );

                // プレハブを指定位置に生成（回転なし）
                GameObject intersection = Instantiate(intersectionPrefab, position, Quaternion.identity);

                // 生成したオブジェクトをこのオブジェクトの子に設定（階層整理のため）
                intersection.transform.SetParent(transform);

                // オブジェクト名をわかりやすく設定（例: "Intersection_3_5"）
                intersection.name = $"Intersection_{x}_{z}";

                // IntersectionControllerコンポーネントを取得して位置情報を設定
                IntersectionController controller = intersection.GetComponent<IntersectionController>();
                controller.SetPosition(x, z);

                // 生成したオブジェクトを配列に保存（後から参照するため）
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