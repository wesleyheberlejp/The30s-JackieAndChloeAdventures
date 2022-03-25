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
    private bool estaAbaixado = false;
    private bool estaPulando = false;
    private bool estaCaminando = false;

    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        var movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        var movimentoVertical = Input.GetAxisRaw("Vertical");
        var velocidadeFinal = velocidade;

        IniciaImputs();

        switch (estaPulando)
        {
            case false:
                AdicionaMovimentaçãoNoChacao(movimentoHorizontal, movimentoVertical);
                break;
            case true:
                AdicionaMovimentacaoPulo(movimentoHorizontal);
                break;
        }
    }

    public void IniciaImputs()
    {
        if (Input.GetKeyDown("space"))
        {
            estaPulando = true;
        }

        if (Input.GetKeyDown("left shift"))
        {
            estaCorrendo = true;
        }

        if (Input.GetKeyUp("left shift"))
        {
            AtivaAnimacao("Correndo", false);
            estaCorrendo = false;
        }

        if (Input.GetKeyUp("a") ||
            Input.GetKeyUp("s") ||
            Input.GetKeyUp("d") ||
            Input.GetKeyUp("w"))
        {
            estaCaminando = false;
        }
    }

    public void AdicionaMovimentaçãoNoChacao(float movimentoHorizontal, float movimentoVertical)
    {
        bool movimento = PersonagemEstaEmMovimento(movimentoHorizontal, movimentoVertical);
        var direcao = new Vector3(movimentoHorizontal, movimentoVertical, 0f).normalized;
        controladorPersonagem.Move(direcao * velocidade * Time.deltaTime);
        VerificaDirecaoMovimento(movimentoHorizontal);

        switch (estaAbaixado)
        {
            case true:
                AdicionaMovimentosAbaixado();
                break;
            case false:
                if (movimento)
                {
                    if (estaCorrendo)
                    {
                        AtivaAnimacao("Correndo", true);
                    }
                    else
                    {
                        estaCaminando = true;
                        AtivaAnimacao("Caminhando", true);
                    }
                }
                else
                {
                    AtivaAnimacao("Caminhando", false);
                    AtivaAnimacao("Correndo", false);
                }
                break;
        }
    }

    private void VerificaDirecaoMovimento(float movimentoHorizontal)
    {
        if (movimentoHorizontal < 0)
        {
            personagemIsFliped = true;
            FlipaSprite();
        }
        if (movimentoHorizontal > 0)
        {
            personagemIsFliped = false;
            FlipaSprite();
        }
    }

    public bool PersonagemEstaEmMovimento(float movimentoHorizontal, float movimentoVertical)
    {
        var movimento = false;
        if (movimentoHorizontal != 0 || movimentoVertical != 0)
        {
            movimento = true;
        }

        return movimento;
    }

    public void AdicionaMovimentacaoPulo(float movimentoHorizontal)
    {
        var direcao = new Vector3(movimentoHorizontal, 0f, 0f).normalized;

        controladorPersonagem.Move(direcao * velocidade * Time.deltaTime);

        AtivaAnimacao("Pulando", true);
    }

    public void AdicionaMovimentosAbaixado()
    {
        if (estaEmMovimento)
        {
            AtivaAnimacao("CaminhandoAbaixado", true);
        }
        else
        {
            AtivaAnimacao("Abaixado", true);
        }
    }

    public void AtivaAnimacao(string nomeAnimacao, bool status)
    {
        controladorAnimacao.SetBool(nomeAnimacao, status);
    }

    public void AlertaAnimação(string message)
    {
        Debug.Log(message.Equals("PuloFinalizado"));

        if (message.Equals("PuloFinalizado"))
        {
            estaPulando = false;
            AtivaAnimacao("Pulando", false);
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
