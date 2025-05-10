using UnityEngine;
using UnityEngine.EventSystems;
using computeryo;

public class SlotClickHandler : MonoBehaviour, IPointerClickHandler
{
    public string nomeDoSlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        var carta = GameManager.Instance.CartaSelecionada;

        // Se alguma carta foi selecionada da mão do jogador
        if (carta != null)
        {
            var turno = TurnManager.Instance.turnoAtual;
            if (carta.dono == DonoCarta.Jogador && turno == TurnManager.Turno.Jogador)
            {
                GameManager.Instance.TentarPosicionarCarta(nomeDoSlot);
            }
        }
        else
        {
            // Verifica se há uma carta neste slot
            if (FieldManager.Instance.cartasNosSlots.TryGetValue(nomeDoSlot, out Card cardNoSlot))
            {
                var turno = TurnManager.Instance.turnoAtual;
                bool slotDoJogador = FieldManager.Instance.SlotPertenceAoJogador(nomeDoSlot);

                // Primeiro clique: Seleção da carta atacante (somente do jogador no turno dele)
                if (GameManager.Instance.cartaAtacanteSelecionada == null)
                {
                    if (turno == TurnManager.Turno.Jogador && slotDoJogador)
                    {
                        GameManager.Instance.cartaAtacanteSelecionada = cardNoSlot;
                        GameManager.Instance.atackSlot = nomeDoSlot;
                        Debug.Log($"Carta atacante selecionada: {cardNoSlot.nome}");
                    }
                    else
                    {
                        Debug.Log("Você só pode selecionar cartas do seu lado durante seu turno.");
                    }
                }
                // Segundo clique: Seleção do alvo da IA para atacar
                else
                {
                    bool slotAtacanteJogador = FieldManager.Instance.SlotPertenceAoJogador(GameManager.Instance.atackSlot);
                    bool slotDefensorJogador = FieldManager.Instance.SlotPertenceAoJogador(nomeDoSlot);

                    // Pode atacar apenas se o atacante for do jogador e o defensor da IA
                    if (slotAtacanteJogador && !slotDefensorJogador)
                    {
                        Debug.Log($"Iniciando combate entre {GameManager.Instance.cartaAtacanteSelecionada.nome} e {cardNoSlot.nome}");

                        if (CombatManager.Instance == null || GameManager.Instance == null)
                        {
                            Debug.LogError("CombatManager ou GameManager não foram instanciados.");
                            return;
                        }

                        if (string.IsNullOrEmpty(GameManager.Instance.atackSlot))
                        {
                            Debug.LogWarning("Slot atacante não foi selecionado!");
                            return;
                        }

                        CombatManager.Instance.ResolverCombate(
                            GameManager.Instance.atackSlot, nomeDoSlot
                        );

                        GameManager.Instance.cartaAtacanteSelecionada = null;
                        GameManager.Instance.atackSlot = null;
                    }
                    else
                    {
                        Debug.Log("O alvo deve estar no lado oposto ao da carta atacante.");
                        GameManager.Instance.cartaAtacanteSelecionada = null;
                        GameManager.Instance.atackSlot = null;
                    }
                }
            }
            else
            {
                Debug.Log($"Slot {nomeDoSlot} está vazio.");
            }
        }
    }

}
