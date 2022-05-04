using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetrominoes;
using TMPro;

public class ScoreController : MonoBehaviour {
    TetrominoConstants tetrominoConsts = new TetrominoConstants();
    SpawnBlock spawnBlock;
    public int score = 0;
    TextMeshProUGUI text;
    public int? highestCompletedY;
    public int completedLines = 0;
    public int highScore;

    void Start() {
        text = GameObject.Find("score_ui").transform.Find("text").GetComponent<TextMeshProUGUI>();
        spawnBlock = GetComponent<SpawnBlock>();
        highScore = PlayerPrefs.GetInt("highscore");
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
        score += numCompletedLines * 100;
        text.text = score.ToString();
        CalculateHighScore();
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
