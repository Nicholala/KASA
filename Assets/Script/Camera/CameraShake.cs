using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Animator mainCamera;
    public void Shake()
    {
        mainCamera.SetTrigger("Shake");
    }
}
