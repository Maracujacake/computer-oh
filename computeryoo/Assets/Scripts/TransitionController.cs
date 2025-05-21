using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using computeryo;

public class TransitionController : MonoBehaviour
{
    public Animator animator;
    private string cenaAlvo;
    public Image cartaEsquerda;
    public Image cartaDireita;

    
    void Start()
    {
        // Define a arte das cartas antes de animar
        cartaEsquerda.sprite = BattleIASettings.deckSelecionadoData.cartas[0].imagem;
        cartaDireita.sprite = BattleIASettings.deckInimigoSelecionadoData.cartas[0].imagem;

        // Começa animação de fechar
        animator.SetTrigger("Fechar");
    }

    
    // Chamado pelo Animation Event no fim da animação de "Fechar"
    public void CarregarCena()
    {
        StartCoroutine(CarregarCenaAsync());
    }

    private IEnumerator CarregarCenaAsync()
    {
        string cenaAlvo = PlayerPrefs.GetString("CenaDestino", "SampleScene");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(cenaAlvo);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
