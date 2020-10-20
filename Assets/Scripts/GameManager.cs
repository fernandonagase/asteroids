using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    public bool IsGameOver { get; private set; }

    private HUDManager hudManager;

    private void Start()
    {
        hudManager = GameObject.FindWithTag("HUD")
            .GetComponent<HUDManager>();
    }

    public void Score(int amount)
    {
        score += amount;
        hudManager.UpdateScore(score);
    }

    public void ToggleGameOver()
    {
        IsGameOver = !IsGameOver;
        hudManager.ToggleGameOver(IsGameOver);
    }

    public void ResetGame()
    {
        score = 0;
        hudManager.UpdateScore(score);
    }
}
