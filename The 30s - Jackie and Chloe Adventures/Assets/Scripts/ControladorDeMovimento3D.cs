using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDeMovimento3D : MonoBehaviour
{

    public float velocidade = 5f;
    private float velocidadeInicial = 0f;
    public float multiplicadorCorrendo = 1.3f;
    public float MultiplicadoVelocidadeNaCorrida = 2f;
    public CharacterController controladorPersonagem;
    private float gravidade = -9.81f;
    public Animator controladorAnimacao;
    public GameObject spritePersonagem;

    private bool personagemIsFliped = false;
    private bool estaCorrendo = false;
    private bool estaEmMovimento = false;
    private bool gravidadeHabilitada = false;
    private bool estaAgachado = false;
    private bool estaPulando = false;
    private bool estaCaminando = false;

    Vector3 velocity;

    void Start()
    {
        velocidadeInicial = velocidade;
    }

    // Update is called once per frame
    void Update()
    {
        var movimentoHorizontal = Input.GetAxisRaw("Horizontal");
        var movimentoVertical = Input.GetAxisRaw("Vertical");
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

        if (Input.GetKeyDown("left ctrl"))
        {
            var agachado = controladorAnimacao.GetBool("Agachado");
            Debug.Log(agachado);
            if (agachado)
            {
                estaAgachado = false;
                SetAnimacao("Agachado", false);
            }
            else
            {
                estaAgachado = true;
            }
        }

        if (Input.GetKeyDown("left shift"))
        {
            estaCorrendo = true;
            if (estaAgachado)
            {
                estaAgachado = false;
                SetAnimacao("Agachado", false);
            }
            velocidade = velocidade * multiplicadorCorrendo;
        }

        if (Input.GetKeyUp("left shift"))
        {
            SetAnimacao("Correndo", false);
            estaCorrendo = false;
            velocidade = velocidadeInicial;
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

        switch (estaAgachado)
        {
            case true:
                AdicionaMovimentosAgachado(movimento);
                break;
            case false:
                if (movimento)
                {
                    if (estaCorrendo)
                    {
                        SetAnimacao("Correndo", true);
                    }
                    else
                    {
                        estaCaminando = true;
                        SetAnimacao("Caminhando", true);
                    }
                }
                else
                {
                    SetAnimacao("Caminhando", false);
                    SetAnimacao("Correndo", false);
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

        SetAnimacao("Pulando", true);
    }

    public void AdicionaMovimentosAgachado(bool movimento)
    {
        if (movimento)
        {
            SetAnimacao("Caminhando", true);
            SetAnimacao("Agachado", true);
        }
        else
        {
            SetAnimacao("Caminhando", false);
            SetAnimacao("Agachado", true);
        }
    }

    public void SetAnimacao(string nomeAnimacao, bool status)
    {
        controladorAnimacao.SetBool(nomeAnimacao, status);
    }

    public void AlertaAnimação(string message)
    {
        if (message.Equals("PuloFinalizado"))
        {
            estaPulando = false;
            SetAnimacao("Pulando", false);
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
