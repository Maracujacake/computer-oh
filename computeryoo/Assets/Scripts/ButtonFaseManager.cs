using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button botaoPassarDeFase;

    private void Start()
    {
        // Atribui a função de passar de fase ao evento de clique do botão
        botaoPassarDeFase.onClick.AddListener(PassarDeFase);
    }

    private void PassarDeFase()
    {
        Debug.Log("Passando de fase...");
        TurnManager.Instance.PassarFase(); // Chama o método PassarFase do TurnManager
    }
}
