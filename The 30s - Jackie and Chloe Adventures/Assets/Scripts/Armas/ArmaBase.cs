using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaBase : MonoBehaviour
{
    public Camera CameraMain;
    public Transform OrigemDisparo;
 
    private Vector3 Direcao;
    private void Update()
    {
        Direcao = MouseUtils.GetDirecaoMouse(CameraMain, this.transform);
        Direcao.z = 0f;
        this.transform.right = Direcao;
    }

    public virtual void Atira(GameObject Projetil)
    {
        Instantiate(Projetil, OrigemDisparo.position, OrigemDisparo.rotation);
    }

    public void MiraPrecisa()
    {

    }
}
