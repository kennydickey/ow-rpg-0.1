using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] float cloudSpeed = 0.4f;

    private void Start()
    {
        
    }

    private void Update()
    {
        // skybox rotate
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * cloudSpeed);
    }
}
