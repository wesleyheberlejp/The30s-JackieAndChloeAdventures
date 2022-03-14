using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mira : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    private void Update()
    {
        var posicaoMouse = MouseUtils.GetPosicaoMouse(mainCamera);
        posicaoMouse.z = 0f;
        transform.position = posicaoMouse;
    }
}
