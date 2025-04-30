using UnityEngine;
using System.Collections.Generic;
using computeryo;

public class DeckManager : MonoBehaviour
{
    public DeckData deckDataJogador;
    public DeckData deckDataInimigo;

    public List<Card> deckJogador = new();
    public List<Card> deckInimigo = new();
    
    public HandManager handManagerJogador;
    public HandManager handManagerInimigo;
     
    public static DeckManager Instance;

    void Awake()
    {
        // Certifica-se de que exista apenas uma instância do DeckManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destrói o objeto se já existir uma instância
        }
    }

    void Start()
    {
        // Pega o deck escolhido no menu
        if (BattleIASettings.deckSelecionadoData != null)
        {
            deckDataJogador = BattleIASettings.deckSelecionadoData;
        }
        
        // Defina aqui o deck do inimigo (pode ser fixo ou aleatório)
        if (BattleIASettings.deckInimigoSelecionadoData != null)
        {
            deckDataInimigo = BattleIASettings.deckInimigoSelecionadoData;
        }

        InicializarDecks();
    }

    public void InicializarDecks()
    {
        if (deckDataJogador != null)
        {
            deckJogador = new List<Card>(deckDataJogador.cartas);
            Embaralhar(deckJogador);

            for (int i = 0; i < 5; i++)
            {
                PuxarCartaJogador();
            }
        }

        if (deckDataInimigo != null)
        {
            deckInimigo = new List<Card>(deckDataInimigo.cartas);
            Embaralhar(deckInimigo);

            for (int i = 0; i < 5; i++)
            {
                PuxarCartaInimigo();
            }
        }
    }

    void Embaralhar(List<Card> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void PuxarCartaJogador()
    {
        if (deckJogador.Count == 0)
            return;

        Card cartaSacada = deckJogador[0];
        deckJogador.RemoveAt(0);

        handManagerJogador.AdicionarCarta(cartaSacada);
    }

    public void PuxarCartaInimigo()
    {
        if (deckInimigo.Count == 0)
            return;

        Card cartaSacada = deckInimigo[0];
        deckInimigo.RemoveAt(0);

        handManagerInimigo.AdicionarCartaInimigo(cartaSacada);
    }

    public void AdicionarCartaAoDeckJogador(Card carta)
    {
        deckJogador.Add(carta);
    }

    public void AdicionarCartaAoDeckInimigo(Card carta)
    {
        deckInimigo.Add(carta);
    }

}
