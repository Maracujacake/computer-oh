using UnityEngine;
using UnityEngine.UI;
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
        faseAtual = Fase.Preparacao;  // Inicia com a fase de preparação
        ProximoTurno();

        // Adicionar lógica para iniciar a fase de preparação (puxar cartas, etc.)
        Debug.Log($"Turno do {turnoAtual}. Fase: {faseAtual}");
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

}
