using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDeMovimento3D : MonoBehaviour
{

    public float velocidade = 5f;
    public float MultiplicadoVelocidadeNaCorrida = 2f;
    public CharacterController controladorPersonagem;
    private float gravidade = -9.81f;
    public Animator controladorAnimacao;
    public GameObject spritePersonagem;

    private bool personagemIsFliped = false;
    private bool estaCorrendo = false;
    private bool estaEmMovimento = false;
    private bool gravidadeHabilitada = false;

    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        var movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        var movimentoVertical = Input.GetAxisRaw("Vertical");
        var velocidadeFinal = velocidade;
        if (estaCorrendo)
        {
            velocidadeFinal = velocidade * MultiplicadoVelocidadeNaCorrida;
            Debug.Log("Aumentou a velocidade");
        }
        var direcao = new Vector3(movimentoHorizontal, movimentoVertical, 0f).normalized;

        AtualizaMovimento(movimentoHorizontal, movimentoVertical);

        AtualizaAnimacao(movimentoHorizontal, movimentoVertical);

        controladorPersonagem.Move(direcao * velocidadeFinal * Time.deltaTime);
    }

    public void AtualizaMovimento(float movimentoHorizontal, float movimentoVertical)
    {
        //controla o flip do sprite
        if (Input.GetKeyDown("a"))
        {
            if (!personagemIsFliped)
            {
                personagemIsFliped = true;
                FlipaSprite();
            }
        }

        if (Input.GetKeyDown("d"))
        {
            if (personagemIsFliped)
            {
                personagemIsFliped = false;
                FlipaSprite();
            }
        }
    }

    public void AtualizaAnimacao(float movimentoHorizontal, float movimentoVertical)
    {
        if (movimentoHorizontal != 0 || movimentoVertical != 0)
        {
            controladorAnimacao.SetBool("Caminhando", true);
        }
        else
        {
            controladorAnimacao.SetBool("Caminhando", false);
        }

        if (Input.GetKeyDown("left shift"))
        {
            controladorAnimacao.SetBool("Correndo", true);
            estaCorrendo = true;
        }
        else if (Input.GetKeyUp("left shift"))
        {
            controladorAnimacao.SetBool("Correndo", false);
            estaCorrendo = false;
        }
    }



    public void FlipaSprite()
    {
        if (personagemIsFliped)
        {
            this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
       
    }
}
