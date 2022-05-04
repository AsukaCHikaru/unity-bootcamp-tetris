using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour {
    private int level = 1;
    private int levelUpRequirement = 3;
    private int exp = 0;

    [SerializeField]
    private float fallInterval = 0.5f;

    TextMeshProUGUI levelText;

    private void Awake() {
        levelText = GameObject.Find("level").transform.Find("text").GetComponent<TextMeshProUGUI>();
    }

    public float GetFallInterfal () {
        return fallInterval;
    }

    public void LevelUp () {
        level++;
        fallInterval -= 0.03f;
        levelText.text = level.ToString();
    }

    public void GainExperience () {
        exp++;
        if (exp >= levelUpRequirement) {
            exp -= levelUpRequirement;
            LevelUp();
        }
    }
}
