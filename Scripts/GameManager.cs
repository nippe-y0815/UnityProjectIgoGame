// GameManager.cs
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]


    public int boardSize = 19;

    //盤面の状態
    private int[,] boardState;

    //現在のターン
    private bool isBlackTurn = true;


    private BoardManager boardManager;

    //取った石の数
    private int blackCaptured = 0;
    private int whiteCaptured = 0;

    void Start()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
        boardState = new int[boardSize, boardSize];
    }

    public bool IsBlackTurn()
    {
        return isBlackTurn;
    }

    // 石を置けるかチェック
    public bool CanPlaceStone(int x, int z)
    {
        //盤面の状態が空の状態　かつ　有効の場合
        return boardState[x, z] == 0 && IsValidMove(x, z);
    }

    // 石を配置
    public void PlaceStone(int x, int z)
    {

        int currentPlayer = isBlackTurn ? 1 : 2;


        boardState[x, z] = currentPlayer;

        // 相手の石を取る処理
        CaptureOpponentStones(x, z);

        // ターン交代
        isBlackTurn = !isBlackTurn;

        UpdateUI();
    }

    bool IsValidMove(int x, int z)
    {
        // 基本的な有効性チェック
        // マウスが盤面の中にない場合はfalse
        if (x < 0 || x >= boardSize || z < 0 || z >= boardSize)
            return false;

        //盤面が空じゃない場合はfalse
        if (boardState[x, z] != 0)
            return false;

        // 自殺手のチェック（簡略化）


        return true;
    }

    //相手の石を取る処理
    void CaptureOpponentStones(int x, int z)
    {
        //現在のプレイヤ
        int currentPlayer = isBlackTurn ? 1 : 2;

        //現在のプレイヤーの相手
        int opponent = currentPlayer == 1 ? 2 : 1;

        //2次元の整数ベクトル配列
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };


        foreach (Vector2Int dir in directions)
        {

            int newX = x + dir.x;
            int newZ = z + dir.y;

            if (IsInBounds(newX, newZ) && boardState[newX, newZ] == opponent)
            {
                List<Vector2Int> group = GetGroup(newX, newZ, opponent);
                if (HasNoLiberties(group))
                {
                    CaptureGroup(group);
                }
            }
        }
    }

    List<Vector2Int> GetGroup(int startX, int startZ, int color)
    {
        List<Vector2Int> group = new List<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(new Vector2Int(startX, startZ));
        visited.Add(new Vector2Int(startX, startZ));

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            group.Add(current);

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = current + dir;

                if (IsInBounds(neighbor.x, neighbor.y) &&
                    !visited.Contains(neighbor) &&
                    boardState[neighbor.x, neighbor.y] == color)
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return group;
    }

    bool HasNoLiberties(List<Vector2Int> group)
    {
        foreach (Vector2Int stone in group)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (Vector2Int dir in directions)
            {
                Vector2Int neighbor = stone + dir;

                if (IsInBounds(neighbor.x, neighbor.y) && boardState[neighbor.x, neighbor.y] == 0)
                {
                    return false; // 自由度がある
                }
            }
        }

        return true; // 自由度がない
    }

    void CaptureGroup(List<Vector2Int> group)
    {
        foreach (Vector2Int stone in group)
        {
            boardState[stone.x, stone.y] = 0;

            // 盤面から石を物理的に削除
            IntersectionController intersection = boardManager.GetIntersection(stone.x, stone.y);
            intersection.RemoveStone();

            // 取った石の数をカウント
            if (isBlackTurn)
                blackCaptured++;
            else
                whiteCaptured++;
        }
    }

    //盤面の座標の中にあるかを判定
    bool IsInBounds(int x, int z)
    {
        return x >= 0 && x < boardSize && z >= 0 && z < boardSize;
    }

    void UpdateUI()
    {
        // UIの更新処理
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateTurnDisplay(isBlackTurn);
            uiManager.UpdateCaptureCount(blackCaptured, whiteCaptured);
        }
    }

    // GameManager.cs の最後に追加するメソッド
    // （UpdateUI()メソッドの後に追加）

    //パス
    public void Pass()
    {
        // ターンを交代
        isBlackTurn = !isBlackTurn;
        UpdateUI();

        Debug.Log(isBlackTurn ? "白がパス - 黒の番" : "黒がパス - 白の番");
    }
    
    // ゲームリセット
    public void ResetGame()
    {
        // 盤面状態をリセット
        boardState = new int[boardSize, boardSize];

        // ターンを黒に戻す
        isBlackTurn = true;

        // キャプチャカウントをリセット
        blackCaptured = 0;
        whiteCaptured = 0;

        // 盤面の石を物理的に削除
        if (boardManager != null)
        {
            boardManager.ResetBoard();
        }

        // UIを更新
        UpdateUI();

        Debug.Log("ゲームがリセットされました");
    }
}