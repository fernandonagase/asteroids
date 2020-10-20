using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Image contentLifebar = null;
    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    private Text ammoText = null;

    public void UpdateLifeBar(int life, int maxLife)
    {
        contentLifebar.fillAmount = (float)life / maxLife;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateAmmo(int ammo)
    {
        ammoText.text = $"Ammo: {ammo}";
    }
}
