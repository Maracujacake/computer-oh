using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using computeryo;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public enum Turno { Jogador, Inimigo }
    public enum Fase { Preparacao, Combate, Final }

    public Turno turnoAtual;
    public Fase faseAtual;

    private bool turnoAtivo;

    public float tempoDeTurno = 60f;

    public int contadorTurnos = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        IniciarTurno(Turno.Jogador);
    }

    public void IniciarTurno(Turno jogador)
    {
        turnoAtual = jogador;
        turnoAtivo = true;
        faseAtual = Fase.Preparacao;
        ProximoTurno();

        Debug.Log($"Turno do {turnoAtual}. Fase: {faseAtual}");

        // Puxa carta e inicia preparação para o jogador ou inimigo
        IniciarFaseDePreparacao();

        if (turnoAtual == Turno.Inimigo)
        {
            StartCoroutine(ExecutarPreparacaoIA());
        }
    }

    public void ProximoTurno()
    {
        contadorTurnos++;
    }

    public void IniciarFaseDePreparacao()
    {
        // Puxar uma carta para o jogador e inimigo
        if (turnoAtual == Turno.Jogador)
        {
            DeckManager.Instance.PuxarCartaJogador();
        }
        else if (turnoAtual == Turno.Inimigo)
        {
             DeckManager.Instance.PuxarCartaInimigo();
        }
    }



    public void PassarFase()
    {   

        if (faseAtual == Fase.Preparacao)
        {
            faseAtual = Fase.Combate;
            Debug.Log("Fase de Combate Iniciada!");

            if (turnoAtual == Turno.Inimigo)
            {
                StartCoroutine(ExecutarCombateIA());
            }
        }
        else if (faseAtual == Fase.Combate)
        {
            faseAtual = Fase.Final;
            FinalizarTurno();
        }
    }

    public void FinalizarTurno()
    {
        turnoAtivo = false;

        // Alterna o turno
        turnoAtual = turnoAtual == Turno.Jogador ? Turno.Inimigo : Turno.Jogador;
        CombatManager.Instance.ResetarAtaques();
        // Inicia o próximo turno
        IniciarTurno(turnoAtual);
    }


    // AÇOES DA I.A

    private IEnumerator ExecutarPreparacaoIA()
    {
        yield return new WaitForSeconds(1f);
        IAController.Instance.RealizarPreparacaoIA();
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator ExecutarCombateIA()
    {
        yield return new WaitForSeconds(1f);
        IAController.Instance.RealizarCombateIA();
        yield return new WaitForSeconds(1f);
        PassarFase(); // só se for a hora de passar o turno
    }


    

}
