using UnityEngine;
using UnityEngine.EventSystems;
using computeryo;

public class SlotClickHandler : MonoBehaviour, IPointerClickHandler
{
    public string nomeDoSlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        var carta = GameManager.Instance.CartaSelecionada;
        

        // se alguma carta foi selecionada da mão
        if (carta != null){
            var turno = TurnManager.Instance.turnoAtual;
            if (carta.dono == DonoCarta.Jogador && turno == TurnManager.Turno.Jogador)
            {
                GameManager.Instance.TentarPosicionarCarta(nomeDoSlot);
            }
            else if (carta.dono == DonoCarta.Inimigo && turno == TurnManager.Turno.Inimigo)
            {
                GameManager.Instance.PosicionarCartaInimigo(carta.gameObject, carta.cardData, nomeDoSlot);
            }
        }
        else{
            if (FieldManager.Instance.cartasNosSlots.TryGetValue(nomeDoSlot, out Card cardNoSlot))
                {

                    // Primeiro clique (seleção da carta atacante)
                    if (GameManager.Instance.cartaAtacanteSelecionada == null)
                    {
                        bool slotDoJogador = FieldManager.Instance.SlotPertenceAoJogador(nomeDoSlot);
                        var turno = TurnManager.Instance.turnoAtual;

                        if ((turno == TurnManager.Turno.Jogador && slotDoJogador) ||
                            (turno == TurnManager.Turno.Inimigo && !slotDoJogador))
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
                    else
                    {

                        bool slotAtacanteJogador = FieldManager.Instance.SlotPertenceAoJogador(GameManager.Instance.atackSlot);
                        bool slotDefensorJogador = FieldManager.Instance.SlotPertenceAoJogador(nomeDoSlot);
                        // Segundo clique (tentativa de atacar)
                        if (slotAtacanteJogador != slotDefensorJogador)
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
