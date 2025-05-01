using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using computeryo;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab;          // Prefab visual da carta
    public Transform handArea;             // Área onde as cartas da mão serão exibidas
    public Transform enemyHandArea;        // Área onde as cartas do inimigo serão exibidas 

    public List<Card> cartasNaMao = new(); // Lista lógica das cartas na mão
    public List<Card> cartasNaMaoInimigo = new(); // Lista lógica das cartas na mão do inimigo

    public int cartasIniciais = 5;         // Número de cartas no primeiro saque

    public Sprite versoCartaSprite;        // Sprite do verso da carta

    public static HandManager Instance;

    void Start()
    {

    }

    void Awake()
    {
        Instance = this;
    }


    // JOGADOR

    public void AdicionarCarta(Card novaCarta)
    {
        if (cartasNaMao.Count >= 10)
        {
            Debug.LogWarning("Não é possível adicionar mais cartas. A mão está cheia!");
            return;
        
        }
        // Instancia o visual da carta na UI, dentro da área da mão
        GameObject novaCartaGO = Instantiate(cardPrefab, handArea);
        CardDisplay display = novaCartaGO.GetComponent<CardDisplay>();
        display.cardData = novaCarta;
        display.AtualizarDono(DonoCarta.Jogador);
        display.updateCardDisplay();

        // Adiciona à lista lógica
        cartasNaMao.Add(novaCarta);

        ReorganizarCartasNaMao();

        display.AtualizarVisibilidadeVerso(estaNaMao: false);
    }


    public void ReorganizarCartasNaMao()
    {
        float spacing = 3f; // distância entre as cartas

        if (cartasNaMao.Count >= 8)
            spacing /= 2.5f; // diminui para caber melhor na tela

        float startX = -(cartasNaMao.Count - 1) * spacing / 2f;
        Debug.Log("Reorganizando cartas na mão. Total de cartas: " + cartasNaMao.Count);

        for (int i = 0; i < handArea.childCount; i++)
        {
            Transform cartaGO = handArea.GetChild(i);
            Transform canvasTransform = cartaGO.Find("CardCanvas"); // Nome exato do seu objeto filho

            if (canvasTransform != null)
            {
                RectTransform rectTransform = canvasTransform.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    float xPos = startX + i * spacing;
                    rectTransform.anchoredPosition = new Vector2(xPos, 0);
                }
            }
            else
            {
                Debug.LogWarning("CardCanvas não encontrado em: " + cartaGO.name);
            }
        }
    }

        public void RemoverCarta(GameObject cartaGO)
    {
        CardDisplay display = cartaGO.GetComponent<CardDisplay>();
        if (display != null)
        {
            cartasNaMao.Remove(display.cardData);
            Destroy(cartaGO); // ou desativa/move para o slot
            ReorganizarCartasNaMao();
            
        }
    }


    // INIMIGO

    public void AdicionarCartaInimigo(Card novaCarta)
    {
        if (cartasNaMaoInimigo.Count >= 10)
        {
            Debug.LogWarning("Mão do inimigo cheia!");
            return;
        }

        GameObject novaCartaGO = Instantiate(cardPrefab, enemyHandArea);
        CardDisplay display = novaCartaGO.GetComponent<CardDisplay>();
        
        display.cardData = novaCarta;
        display.AtualizarDono(DonoCarta.Inimigo);
        display.updateCardDisplay();
        
        // Adiciona imagem de verso por cima
        GameObject versoGO = new GameObject("VersoCarta");
        versoGO.transform.SetParent(novaCartaGO.transform, false); // pai = a carta
        Image img = versoGO.AddComponent<Image>();
        img.sprite = versoCartaSprite;
        img.rectTransform.anchorMin = Vector2.zero;
        img.rectTransform.anchorMax = Vector2.one;
        img.rectTransform.offsetMin = Vector2.zero;
        img.rectTransform.offsetMax = Vector2.zero;

        cartasNaMaoInimigo.Add(novaCarta);

        ReorganizarCartasNaMaoInimigo();

        display.AtualizarVisibilidadeVerso(estaNaMao: true);
    }


    public void ReorganizarCartasNaMaoInimigo()
    {
        float spacing = 3f;
        if (cartasNaMaoInimigo.Count >= 8)
            spacing /= 2.5f;

        float startX = -(cartasNaMaoInimigo.Count - 1) * spacing / 2f;

        for (int i = 0; i < enemyHandArea.childCount; i++)
        {
            Transform cartaGO = enemyHandArea.GetChild(i);
            Transform canvasTransform = cartaGO.Find("CardCanvas");

            if (canvasTransform != null)
            {
                RectTransform rectTransform = canvasTransform.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    float xPos = startX + i * spacing;
                    rectTransform.anchoredPosition = new Vector2(xPos, 0);
                }
            }
        }
    }


    public void RemoverCartaInimigo(GameObject cartaGO)
    {
        CardDisplay display = cartaGO.GetComponent<CardDisplay>();
        if (display != null)
        {
            cartasNaMaoInimigo.Remove(display.cardData);
            display.AtualizarVisibilidadeVerso(estaNaMao: false);
            Destroy(cartaGO); 
            ReorganizarCartasNaMaoInimigo();
        }
    }


}