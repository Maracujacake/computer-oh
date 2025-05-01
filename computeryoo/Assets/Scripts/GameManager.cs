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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Garante que só tenha um GameManager
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    public void PosicionarCartaInimigo(GameObject prefabCarta, Card cardData, string nomeSlot)
    {
        if (!fieldManager.EstaDisponivel(nomeSlot))
        {
            Debug.Log("Slot de inimigo ocupado ou inválido");
            return;
        }

        if (TurnManager.Instance.faseAtual != TurnManager.Fase.Preparacao || TurnManager.Instance.turnoAtual != TurnManager.Turno.Inimigo)
        {
            Debug.Log("Não é a fase de preparação, você não pode posicionar cartas.");
            fieldManager.LimparHighlights();
            return;
        }

        // Marca o slot como ocupado
        fieldManager.OcupaSlot(nomeSlot);

        fieldManager.cartasNosSlots[nomeSlot] = cardData;

        handManager.RemoverCartaInimigo(cartaSelecionada.gameObject); // Remove da mão do inimigo
        
        // Instancia e posiciona a carta no slot
        Transform slotTransform = GameObject.Find(nomeSlot).transform;
        GameObject cartaGO = Instantiate(prefabCarta, slotTransform);
        cartaGO.transform.localPosition = Vector3.zero;
        cartaGO.transform.localScale = Vector3.one;

        var display = cartaGO.GetComponent<CardDisplay>();
        display.cardData = cardData;
        display.updateCardDisplay();

        // Atualiza o slot com a imagem da carta
        FieldManager.FieldSlot slot = fieldManager.slotsMonstrosInimigo.Find(s => s.nome == nomeSlot);
        if (slot != null)
        {
            slot.imagemSlot.sprite = display.imagemComponent.sprite;
        }

        // Remove interações da carta do inimigo, se necessário
        Destroy(display);

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
