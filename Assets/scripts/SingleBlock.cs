using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBlock : MonoBehaviour
{
    public int x;
    public int y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterBlockPos (Vector3 blockPosition) {
        x = (int)blockPosition.x;
        y = (int)blockPosition.y;
        transform.name = $"{x},{y}";
    }
}
