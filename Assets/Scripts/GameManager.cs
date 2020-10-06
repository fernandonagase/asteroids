using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score = 0;

    public void Score(int amount)
    {
        score += amount;
        GameObject.FindWithTag("HUD")
            .GetComponent<HUDManager>()
            .UpdateScore(score);
    }
}
