using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class TitleSceneController : MonoBehaviour
{
    SpawnBlock spawnBlock;
    GameOver gameOver;

    private void Awake() {
        spawnBlock = GameObject.Find("GameController").GetComponent<SpawnBlock>();
        gameOver = GameObject.Find("GameController").GetComponent<GameOver>();
    }

    public void HandleStartGameClick () {
        SceneManager.LoadScene("play_scene");
    }

    public void HandleQuitGameClick () {
        Application.Quit();
    }

    public async void HandlePlayAgainClick () {
        Debug.Log("play again");

        gameOver.HideOverlay();
        gameOver.ResetGameOver();

        Transform blocksTransform = GameObject.Find("Blocks").transform;
        foreach(Transform tetromino in blocksTransform) {
            GameObject.Destroy(tetromino.gameObject);
        }

        Transform lockedBlocksTransform = GameObject.Find("LockedBlocks").transform;
        foreach (Transform block in lockedBlocksTransform) {
            GameObject.Destroy(block.gameObject);
        }

        // Wait for destroy to finish
        await Task.Delay(300);

        spawnBlock.ClearNextList();
        spawnBlock.resetIsSpawned();

        for (int i = 0; i < 5; i++) {
            spawnBlock.Queue(i + 1);
        }
    }
}
