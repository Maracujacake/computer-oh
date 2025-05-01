using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using computeryo;

public class CemiterioManager : MonoBehaviour
{
    public static CemiterioManager Instance;

    [Header("Referências dos cemitérios")]
    public Image imagemCemiterioJogador;
    public Image imagemCemiterioInimigo;

    [Header("Canvas e zoom")]
    public GameObject canvasZoomCemiterio; // <-- este deve estar desativado inicialmente
    public Transform zoomArea;
    public GameObject cardPrefab;

    [Header("Botões")]
    public Button botaoProximo;
    public Button botaoAnterior;
    public Button botaoFechar;

    private GameObject zoomCardGO;
    private List<Card> cartasJogador = new();
    private List<Card> cartasInimigo = new();
    private int indiceCartaAtual = 0;
    private bool visualizandoJogador;

    private void Awake()
    {
        Instance = this;

        // Garantir que o canvas está desativado ao iniciar
        if (canvasZoomCemiterio != null)
            canvasZoomCemiterio.SetActive(false);

        // Adiciona listeners de clique nas imagens dos cemitérios
        AddClickListener(imagemCemiterioJogador, true);
        AddClickListener(imagemCemiterioInimigo, false);

        // Adiciona listeners aos botões
        botaoProximo.onClick.AddListener(ProximaCarta);
        botaoAnterior.onClick.AddListener(CartaAnterior);
        botaoFechar.onClick.AddListener(FecharZoom);
    }

    private void AddClickListener(Image imagem, bool doJogador)
    {
        EventTrigger trigger = imagem.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = imagem.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { AbrirZoomCemiterio(doJogador); });

        trigger.triggers.Add(entry);
    }

    public void AdicionarAoCemiterio(Card carta, Sprite imagemCarta, bool doJogador)
    {
        if (doJogador)
        {
            cartasJogador.Add(carta);
            imagemCemiterioJogador.sprite = imagemCarta;
            imagemCemiterioJogador.color = Color.white;
        }
        else
        {
            cartasInimigo.Add(carta);
            imagemCemiterioInimigo.sprite = imagemCarta;
            imagemCemiterioInimigo.color = Color.white;
        }
    }

    public void AbrirZoomCemiterio(bool doJogador)
    {
        visualizandoJogador = doJogador;
        var lista = doJogador ? cartasJogador : cartasInimigo;

        if (lista.Count == 0)
        {
            Debug.Log("Cemitério está vazio.");
            return;
        }

        indiceCartaAtual = lista.Count - 1;

        if (canvasZoomCemiterio != null)
            canvasZoomCemiterio.SetActive(true);

        MostrarZoomCartaAtual();
    }

    public void MostrarZoomCartaAtual()
    {
        var lista = visualizandoJogador ? cartasJogador : cartasInimigo;

        if (zoomCardGO != null)
            Destroy(zoomCardGO);

        zoomCardGO = Instantiate(cardPrefab, zoomArea);
        CardDisplay display = zoomCardGO.GetComponent<CardDisplay>();
        display.cardData = lista[indiceCartaAtual];
        display.updateCardDisplay();
        zoomCardGO.transform.localScale = Vector3.one * 3f;
    }

    public void ProximaCarta()
    {
        var lista = visualizandoJogador ? cartasJogador : cartasInimigo;
        if (lista.Count == 0) return;

        if (indiceCartaAtual < lista.Count - 1)
        {
            indiceCartaAtual++;
            MostrarZoomCartaAtual();
        }
        else
        {
            Debug.Log("Já está na última carta do cemitério.");
        }
    }

    public void CartaAnterior()
    {
        var lista = visualizandoJogador ? cartasJogador : cartasInimigo;
        if (lista.Count == 0) return;

        if (indiceCartaAtual > 0)
        {
            indiceCartaAtual--;
            MostrarZoomCartaAtual();
        }
        else
        {
            Debug.Log("Já está na primeira carta do cemitério.");
        }
    }

    public void FecharZoom()
    {
        if (zoomCardGO != null)
            Destroy(zoomCardGO);

        if (canvasZoomCemiterio != null)
            canvasZoomCemiterio.SetActive(false);
    }
}
