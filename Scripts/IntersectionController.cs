// IntersectionController.cs
using UnityEngine;

public class IntersectionController : MonoBehaviour
{
    [Header("Stone Prefabs")]
    public GameObject blackStonePrefab;
    public GameObject whiteStonePrefab;

    private int x, z;
    private GameObject currentStone;
    private BoardManager boardManager;

    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
    }

    void OnMouseDown()
    {
        if (currentStone == null)
        {
            PlaceStone();
        }
    }

    public void SetPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    void PlaceStone()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager.CanPlaceStone(x, z))
        {
            GameObject stonePrefab = gameManager.IsBlackTurn() ? blackStonePrefab : whiteStonePrefab;
            currentStone = Instantiate(stonePrefab, transform.position + Vector3.up * 0.1f, Quaternion.identity);

            gameManager.PlaceStone(x, z);
        }
    }

    public void RemoveStone()
    {
        if (currentStone != null)
        {
            DestroyImmediate(currentStone);
            currentStone = null;
        }
    }
}
