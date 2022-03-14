using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaBase : MonoBehaviour
{
    public Camera CameraMain;
    public Transform OrigemDisparo;
    public GameObject Projetil;
    private Vector3 Direcao;
    private void Update()
    {
        Direcao = MouseUtils.GetDirecaoMouse(CameraMain, this.transform);
        Direcao.z = 0f;
        this.transform.right = Direcao;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Atira");
            Atira();
        }
    }

    public void Atira()
    {
        Instantiate(Projetil, OrigemDisparo.position, OrigemDisparo.rotation);
    }
}
