using UnityEngine;
using computeryo;

public class AttackDirectButtonController : MonoBehaviour
{
    public GameObject botaoAtaqueDireto;

    void Update()
    {
        if (TurnManager.Instance.faseAtual != TurnManager.Fase.Combate)
        {
            botaoAtaqueDireto.SetActive(false);
            return;
        }

        var turno = TurnManager.Instance.turnoAtual;
        bool turnoDoJogador = turno == TurnManager.Turno.Jogador;

        bool atacanteTemCarta = turnoDoJogador
            ? FieldManager.Instance.slotsMonstros.Exists(slot => slot.ocupado)
            : FieldManager.Instance.slotsMonstrosInimigo.Exists(slot => slot.ocupado);

        bool defensorSemCartas = turnoDoJogador
            ? !FieldManager.Instance.slotsMonstrosInimigo.Exists(slot => slot.ocupado)
            : !FieldManager.Instance.slotsMonstros.Exists(slot => slot.ocupado);

        botaoAtaqueDireto.SetActive(atacanteTemCarta && defensorSemCartas);
    }

    public void ExecutarAtaqueDireto()
    {
        var turno = TurnManager.Instance.turnoAtual;
        CombatManager.Instance.AtaqueDireto(turno == TurnManager.Turno.Jogador);
    }
}