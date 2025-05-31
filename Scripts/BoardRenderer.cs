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
    
    // 線を描画するメソッド。start から end までの LineRenderer を生成
    void CreateLine(Vector3 start, Vector3 end)
    {
        // Y座標を高くする（例：0.1f 上に持ち上げる）
        start.y += 0.1f;
        end.y += 0.1f;

        // 新しい空のゲームオブジェクトを作成し、親に設定
        GameObject line = new GameObject("GridLine");
        line.transform.SetParent(transform);

        // LineRenderer コンポーネントを追加して設定
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 2;

        // 線の始点と終点を設定（少し上に持ち上げた位置）
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}