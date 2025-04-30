using UnityEngine;
using System.Collections.Generic;
using computeryo;

public class EnemyDeckManager : MonoBehaviour
{
    public DeckData deckData;
    public List<Card> deck = new();
    public HandManager handManager;

    void Start()
    {
        
    }

    public void InicializarDeck()
    {
        if (deckData != null)
        {
            deck = new List<Card>(deckData.cartas);
            Embaralhar();

            for (int i = 0; i < 5; i++)
            {
                PuxarCarta();
            }
        }
    }

    public void PuxarCarta()
    {
        if (deck.Count == 0)
        {
            Debug.Log("Deck do inimigo vazio!");
            return;
        }

        Card cartaSacada = deck[0];
        deck.RemoveAt(0);

        handManager.AdicionarCartaInimigo(cartaSacada);
    }

    public void Embaralhar()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int randIndex = Random.Range(i, deck.Count);
            (deck[i], deck[randIndex]) = (deck[randIndex], deck[i]);
        }
    }
}
