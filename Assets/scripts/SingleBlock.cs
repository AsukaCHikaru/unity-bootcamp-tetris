using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBlock : MonoBehaviour {
    public int x;
    public int y;

    Color spriteColor;
    SpriteRenderer spriteRenderer;
    float colorTransparentInterval = 30;
    IEnumerator _transparentCoroutine;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void RegisterBlockPos(Vector3 blockPosition) {
        x = (int)blockPosition.x;
        y = (int)blockPosition.y;
        transform.name = $"{x},{y}";
    }


    public void DestroyBlock() {
        _transparentCoroutine = ColorTransparentCoroutine();
        StartCoroutine(_transparentCoroutine);
    }

    IEnumerator ColorTransparentCoroutine() {
        while (spriteRenderer.color.a > 0) {
            yield return new WaitForSeconds(1 / colorTransparentInterval);
            spriteColor = spriteRenderer.color;
            colorTransparentInterval *= 1.05f;
            if (spriteColor.a < 0.1) {
                spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0);
                StopCoroutine(_transparentCoroutine);
                Destroy(this.gameObject);
            }
            else {
                spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, spriteColor.a - 0.05f);
                Debug.Log($"opacity {spriteColor.a}");
            }
        }
    }
}
