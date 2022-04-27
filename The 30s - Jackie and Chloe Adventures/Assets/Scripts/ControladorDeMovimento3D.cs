using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDeMovimento3D : MonoBehaviour
{

    public float velocidade = 5f;
    private float velocidadeInicial = 0f;
    public float multiplicadorCorrendo = 1.3f;
    public CharacterController controladorPersonagem;
    public Collider detectorChao;
    private float gravidade = -9.81f;
    public Animator controladorAnimacao;
    public GameObject spritePersonagem;
    public GameObject poeira;
    public Transform poeiraEmptyPosition;

    private bool personagemIsFliped = false;
    private bool estaCorrendo = false;
    private bool estaEmMovimento = false;
    public bool gravidadeHabilitada = false;
    private bool estaAgachado = false;
    private bool estaPulando = false;
    private bool estaNoChao = false;
    private bool estaCaminando = false;

    private Vector3 velocity;

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

        //verificaEstaNoChao();

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
        bool estaEmMovimento = PersonagemEstaEmMovimento(movimentoHorizontal, movimentoVertical);
        
        Vector3 movimento = transform.right * movimentoHorizontal + transform.forward * movimentoVertical;
        controladorPersonagem.Move(movimento * velocidade * Time.deltaTime);
        if (gravidadeHabilitada)
        {
            AplicaGravidade();
        }

        VerificaDirecaoMovimento(movimentoHorizontal);

        switch (estaAgachado)
        {
            case true:
                AdicionaMovimentosAgachado(estaEmMovimento);
                break;
            case false:
                if (estaEmMovimento)
                {
                    if (estaCorrendo)
                    {
                        SetAnimacao("Correndo", true);
                    }
                    else
                    {
                        estaCaminando = true;
                        //InvokeRepeating("IntanciaPoeira", 1, 0);
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

    //public void verificaEstaNoChao()
    //{
    //    detectorChao.
    //}

    public void AplicaGravidade()
    {
        velocity.y += gravidade * Time.deltaTime;
        controladorPersonagem.Move(velocity);

        if (estaNoChao)
        {
            velocity.y = 0f;
        }
    }

    public void IntanciaPoeira()
    {
        Instantiate(poeira, poeiraEmptyPosition,true);
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
            spritePersonagem.transform.rotation = Quaternion.Euler(-15f, 180f, 0f);
        }
        else
        {
            spritePersonagem.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
        }

    }
}
