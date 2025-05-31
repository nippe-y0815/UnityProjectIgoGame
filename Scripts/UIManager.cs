// UIManager.cs
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text turnText;
    public Text blackCaptureText;
    public Text whiteCaptureText;
    public Button passButton;
    public Button resetButton;
    
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        passButton.onClick.AddListener(PassTurn);
        resetButton.onClick.AddListener(ResetGame);
        
        UpdateTurnDisplay(true);
    }
    
    public void UpdateTurnDisplay(bool isBlackTurn)
    {
        turnText.text = isBlackTurn ? "黒の番" : "白の番";
    }
    
    public void UpdateCaptureCount(int blackCaptured, int whiteCaptured)
    {
        blackCaptureText.text = $"黒: {blackCaptured}";
        whiteCaptureText.text = $"白: {whiteCaptured}";
    }
    
    void PassTurn()
    {
        gameManager.Pass();
    }
    
    void ResetGame()
    {
        gameManager.ResetGame();
    }
}