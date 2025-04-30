using UnityEngine;
using System.Collections.Generic;
using computeryo; 

[CreateAssetMenu(fileName = "NovoDeck", menuName = "Cartas/Deck")]
public class DeckData : ScriptableObject
{
    public string nomeDoDeck;
    public List<Card> cartas;
}
