using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private float fadeAlpha = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fadeAlpha -= 0.01f;
        GetComponent<CanvasGroup>().alpha = fadeAlpha;
    }
}
