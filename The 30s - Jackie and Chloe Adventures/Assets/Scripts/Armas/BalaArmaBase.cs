using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaArma : MonoBehaviour
{
    public float Velocidade = 20f;
    public Rigidbody2D rb;

    public void IniciaTiro(Transform transform)
    {
        rb.velocity = transform.right * Velocidade;
    }
}
