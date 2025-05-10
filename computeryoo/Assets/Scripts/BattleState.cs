using UnityEngine;
using System.Collections.Generic;

using computeryo;
public class BattleState : MonoBehaviour
{
    public static BattleState Instance { get; private set; }

    public Card cartaSelecionadaIA;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
