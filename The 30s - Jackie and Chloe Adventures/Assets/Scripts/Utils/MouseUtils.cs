using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUtils : MonoBehaviour
{
    public static Vector3 GetPosicaoMouse(Camera camera)
    {
        return camera.ScreenToWorldPoint(Input.mousePosition);
    }

    public static Vector3 GetDirecaoMouse(Camera camera, Transform transform)
    {
        var posicaoMouse = GetPosicaoMouse(camera);
        var objPosition = transform.position; 

        return new Vector3( posicaoMouse.x - objPosition.x, posicaoMouse.y -  objPosition.y, posicaoMouse.z - objPosition.z);
    }
}
