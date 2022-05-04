using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class GameOver : MonoBehaviour
{
    public bool isGameOver = false;
    GameObject OverlayUI;
    TextMeshProUGUI score_text;
    ScoreController scoreController;

    private void Awake() {
        OverlayUI = GameObject.Find("Canvas").transform.Find("game_over").gameObject;
        score_text = OverlayUI.transform.Find("score_text").GetComponent<TextMeshProUGUI>();
        scoreController = GetComponent<ScoreController>();
    }
    public void Over() {
        Debug.Log("game over");
        isGameOver = true;
        ShowOverlay();
    }

    async Task ShowOverlay () {
        await Task.Delay(500);
        score_text.text = scoreController.score.ToString();
        OverlayUI.SetActive(true);
    }
}
