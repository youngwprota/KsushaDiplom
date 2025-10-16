using UnityEngine;
using UnityEngine.UI;

public class PuzzleGameManager : MonoBehaviour
{
    public static PuzzleGameManager Instance;
    public GameObject successPanel;
    private int totalPieces;
    private int placedPieces = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        totalPieces = FindObjectsOfType<PuzzlePiece>().Length;
        successPanel.SetActive(false);
    }

    public void RegisterCorrectPiece()
    {
        placedPieces++;
        if (placedPieces >= totalPieces)
        {
            successPanel.SetActive(true);
            Debug.Log("Puzzle completed!");
        }
    }
}