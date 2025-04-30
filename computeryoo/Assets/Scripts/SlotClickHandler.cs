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
            if (carta.dono == DonoCarta.Jogador)
            {
                GameManager.Instance.TentarPosicionarCarta(nomeDoSlot);
            }
            else if (carta.dono == DonoCarta.Inimigo)
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
                        // Só pode selecionar carta do jogador como atacante
                        if (FieldManager.Instance.SlotPertenceAoJogador(nomeDoSlot))
                        {
                            GameManager.Instance.cartaAtacanteSelecionada = cardNoSlot;
                            GameManager.Instance.atackSlot = nomeDoSlot;
                            Debug.Log($"Carta atacante selecionada: {cardNoSlot.nome}");
                        }
                    }
                    else
                    {
                        // Segundo clique (tentativa de atacar)
                        if (FieldManager.Instance.SlotPertenceAoInimigo(nomeDoSlot))
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
                            Debug.Log("Você só pode atacar cartas do inimigo.");
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
