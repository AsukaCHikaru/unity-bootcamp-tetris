using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionBlock : MonoBehaviour {
    bool isMoving = false;
    ScoreController scoreController;
    SpawnBlock spawnBlock;

    private void Start() {
        scoreController = GetComponent<ScoreController>();
        spawnBlock = GetComponent<SpawnBlock>();
    }

    public void Move() {
        if (isMoving) {
            return;
        }
        isMoving = true;
        Debug.Log("ismoving");
        int? highestY = scoreController.highestCompletedY;
        int completedLines = scoreController.completedLines;
        Debug.Log($"{highestY},{completedLines}");
        Transform lockedBlocks = GameObject.Find("LockedBlocks").transform;

        foreach (Transform block in lockedBlocks) {
            SingleBlock singleBlock = block.GetComponent<SingleBlock>();
            if (singleBlock.y > highestY) {
                Vector3 newPos = new Vector3(singleBlock.x, singleBlock.y - completedLines, 0);
                singleBlock.transform.position = new Vector3(newPos.x + 0.5f, newPos.y + 0.5f, 0);
                singleBlock.RegisterBlockPos(newPos);
            }
        }

        scoreController.Reset();
        spawnBlock.resetIsSpawned();
        isMoving = false;
    }
}
