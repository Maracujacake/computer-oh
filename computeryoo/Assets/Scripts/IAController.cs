using UnityEngine;
using System.Collections;

public class IAController : MonoBehaviour
{
    public static IAController Instance;
    private BattleIASettings iaSettings;
    public HandManager handManagerIA; // <- arraste o EnemyHandArea aqui no Inspector

    private void Awake()
    {
        Instance = this;
        iaSettings = new BattleIASettings(handManagerIA); // passa a referência correta
    }


    public void RealizarPreparacaoIA()
    {
        iaSettings.selecionarCartaMao(); // coloca carta no campo
        Debug.Log("IA fez a preparação");
    }

    public void RealizarCombateIA()
    {
        iaSettings.atacarCartaJogador(); // realiza ataque
    }
}
