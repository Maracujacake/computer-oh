using UnityEngine;
using System.Collections.Generic;

using computeryo;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private CardDisplay cartaSelecionada;

    public FieldManager fieldManager;
    public HandManager handManager;

    public int vidaJogador = 2000;
    public int vidaInimigo = 2000;
    public CardDisplay CartaAtacante; // Referência à carta que foi clicada e vai atacar
    public Card cartaAtacanteSelecionada = null;
    public string atackSlot = null;

    public List<Card> deckJogador = new();
    public List<Card> deckInimigo = new();


    void Awake()
    {
        Instance = this;
    }

    public CardDisplay CartaSelecionada
    {
        get { return cartaSelecionada; }
    }

    public void SelecionarCarta(CardDisplay carta)
    {
        if (cartaSelecionada != null)
            return; // Já tem uma carta sendo selecionada

        cartaSelecionada = carta;
        Debug.Log("Dono da carta: " + cartaSelecionada.dono);
        // Destaca os slots possíveis
        fieldManager.HighlightSlotsMonstroDisponiveis();
    }

    public void TentarPosicionarCarta(string nomeSlot)
    {
        if (cartaSelecionada == null)
            return;

        if (!fieldManager.EstaDisponivel(nomeSlot))
        {
            Debug.Log("Slot ocupado ou inválido");
            return;
        }

        if (TurnManager.Instance.faseAtual != TurnManager.Fase.Preparacao || TurnManager.Instance.turnoAtual != TurnManager.Turno.Jogador)
        {
            Debug.Log("Não é a fase de preparação, você não pode posicionar cartas.");
            fieldManager.LimparHighlights();
            return;
        }

        // Marca o slot como ocupado
        fieldManager.OcupaSlot(nomeSlot);

        fieldManager.cartasNosSlots[nomeSlot] = cartaSelecionada.cardData;

        // Remove da mão
        handManager.RemoverCarta(cartaSelecionada.gameObject);

        // Clona a carta e posiciona no slot
        Transform slotTransform = GameObject.Find(nomeSlot).transform;
        GameObject cartaClone = Instantiate(cartaSelecionada.gameObject, slotTransform);
        cartaClone.transform.localPosition = Vector3.zero;
        cartaClone.transform.localScale = Vector3.one;

        CardDisplay cloneDisplay = cartaClone.GetComponent<CardDisplay>();
        cloneDisplay.cardData = cartaSelecionada.cardData; // Copia os dados da carta original
        cloneDisplay.updateCardDisplay();  // Atualiza a exibição do clone

        FieldManager.FieldSlot slot = fieldManager.slotsMonstros.Find(s => s.nome == nomeSlot) ?? fieldManager.slotsFeiticos.Find(s => s.nome == nomeSlot);
        if (slot != null)
        {
            slot.imagemSlot.sprite = cloneDisplay.imagemComponent.sprite; // Atualiza a imagem do slot com a imagem da carta
        }

        
        Destroy(cloneDisplay);

        // Limpa estado
        cartaSelecionada = null;
        fieldManager.LimparHighlights();
    }

   public void PosicionarCartaInimigo(Card cardData, string nomeSlot, GameObject cartaNaMaoGO = null)
    {
        if (!fieldManager.EstaDisponivel(nomeSlot, DonoCarta.Inimigo))
        {
            Debug.Log("Slot de inimigo ocupado ou inválido");
            return;
        }

        if (TurnManager.Instance.faseAtual != TurnManager.Fase.Preparacao || TurnManager.Instance.turnoAtual != TurnManager.Turno.Inimigo)
        {
            Debug.Log("Não é a fase de preparação.");
            fieldManager.LimparHighlights();
            return;
        }

        fieldManager.OcupaSlot(nomeSlot, DonoCarta.Inimigo);
        fieldManager.cartasNosSlots[nomeSlot] = cardData;

        // Remove visualmente da mão
        if (cartaNaMaoGO != null)
        {
            handManager.RemoverCartaInimigo(cartaNaMaoGO);
        }

        // Instancia a nova carta no slot
        Transform slotTransform = GameObject.Find(nomeSlot).transform;
        GameObject cartaGO = Instantiate(handManager.cardPrefab, slotTransform);
        cartaGO.transform.localPosition = Vector3.zero;
        cartaGO.transform.localScale = Vector3.one;

        CardDisplay display = cartaGO.GetComponent<CardDisplay>();
        display.cardData = cardData;
        display.AtualizarDono(DonoCarta.Inimigo);
        display.updateCardDisplay();

        Transform versoCard = cartaGO.transform.Find("CardCanvas/VersoCarta/VersoCard");
        if (versoCard != null)
        {
            versoCard.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("VersoCard não encontrado!");
        }

        FieldManager.FieldSlot slot = fieldManager.slotsMonstrosInimigo.Find(s => s.nome == nomeSlot);
        if (slot != null)
        {
            slot.imagemSlot.sprite = display.imagemComponent.sprite;
        }

        fieldManager.LimparHighlights();
    }

    public bool EstaSelecionandoCarta()
    {
        return cartaSelecionada != null;
    }

    // cancela a escolha de carta selecionada no campo
    public void CancelarEscolhaAtacante()
    {
        if (cartaAtacanteSelecionada != null)
        {
            Debug.Log($"Escolha de ataque cancelada: {cartaAtacanteSelecionada.nome}");
            cartaAtacanteSelecionada = null;
            atackSlot = null;
        }
        else
        {
            Debug.Log("Nenhuma carta atacante estava selecionada.");
        }
    }

}
