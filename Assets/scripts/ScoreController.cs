using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetrominoes;
using TMPro;
using System.Threading.Tasks;

public class ScoreController : MonoBehaviour {
    TetrominoConstants tetrominoConsts = new TetrominoConstants();
    SpawnBlock spawnBlock;
    public int score = 0;
    TextMeshProUGUI text;
    TextMeshProUGUI gainedText;
    public int? highestCompletedY;
    public int completedLines = 0;
    public int highScore;
    LevelController levelController;
    float[] scoreCompletedLinesWeight = new float[4] { 1.0f, 2.0f, 4.0f, 8.0f };
    IEnumerator _gainedTextFadeOutCoroutine;

    void Start() {
        text = GameObject.Find("score_ui").transform.Find("text").GetComponent<TextMeshProUGUI>();
        gainedText = GameObject.Find("score_ui").transform.Find("gained").GetComponent<TextMeshProUGUI>();
        spawnBlock = GetComponent<SpawnBlock>();
        highScore = PlayerPrefs.GetInt("highscore");
        levelController = GameObject.Find("GameController").GetComponent<LevelController>();
    }

    public void CheckCompleteLine() {
        List<GameObject> all = new List<GameObject>();
        for (int y = tetrominoConsts.BOTTOM_WALL_Y + 1; y < tetrominoConsts.TOP_WALL_Y; y++) {
            List<GameObject> row = new List<GameObject>();
            for (int x = tetrominoConsts.LEFT_WALL_X + 1; x < tetrominoConsts.RIGHT_WALL_X; x++) {
                GameObject block = GameObject.Find($"{x},{y}");
                if (block != null) {
                    row.Add(block);
                }
            }
            if (row.Count == (tetrominoConsts.RIGHT_WALL_X - 1) - (tetrominoConsts.LEFT_WALL_X + 1) + 1) {
                highestCompletedY = y;
                all.AddRange(row);
            }
        }

        completedLines = all.Count / 10;
        if (completedLines >= 1) {
            CalculateScore(completedLines);
        }
        else {
            spawnBlock.resetIsSpawned();
        }

        foreach (GameObject block in all) {
            block.GetComponent<SingleBlock>().DestroyBlock();
        }
    }

    void CalculateScore(int numCompletedLines) {
        Debug.Log("calculate score");
        int highestY = highestCompletedY == null ? default(int) : highestCompletedY.Value + 10;
        float highestYWeight = ((((float)highestY - 1.0f) / 10.0f) + 1.0f);
        float gainedScore = numCompletedLines * 100 * (int)scoreCompletedLinesWeight[numCompletedLines - 1] * highestYWeight;
        score += (int)gainedScore;
        text.text = score.ToString();
        gainedText.text = $"+{(int)gainedScore}";
        gainedText.color = new Color(1, 1, 1, 1);
        StartGainedTextFadeOut();

        CalculateHighScore();
        for (int i = 0; i < numCompletedLines; i++) {
            levelController.GainExperience();
        }
    }

    async void StartGainedTextFadeOut() {
        await Task.Delay(500);
        _gainedTextFadeOutCoroutine = GainedTextFadeOutCoroutine();
        StartCoroutine(_gainedTextFadeOutCoroutine);
    }

    IEnumerator GainedTextFadeOutCoroutine() {
        while (gainedText.color.a > 0) {
            yield return new WaitForSeconds(0.08f);
            gainedText.color = new Color(1, 1, 1, gainedText.color.a - 0.1f);
            if (gainedText.color.a == 0) {
                StopCoroutine(_gainedTextFadeOutCoroutine);
            }
        }
    }

    void CalculateHighScore() {
        if (score > highScore) {
            PlayerPrefs.SetInt("highscore", score);
            GameObject.Find("score_ui").transform.Find("high").gameObject.SetActive(true);
        }
    }

    public void Reset() {
        highestCompletedY = null;
        completedLines = 0;
    }
}
