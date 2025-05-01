using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using computeryo;

public class BattleResolutionManager : MonoBehaviour
{
    public static BattleResolutionManager Instance { get; private set; }

    public int vidaJogador = 5;
    public int vidaInimigo = 5;

    public TMP_Text vidaJogadorText; 
    public TMP_Text vidaInimigoText; 

    public GameObject telaFimDeJogo; 
    public TMP_Text textoFimDeJogo; // Texto dentro da UI para dizer "Vitória" ou "Derrota"


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        AtualizarVidaUI();
    }

    public void PerderVidaJogador()
    {
        vidaJogador--;
        Debug.Log("Jogador perdeu uma vida! Vidas restantes: " + vidaJogador);
        AtualizarVidaUI();
        VerificarFimDeJogo();
    }

    public void PerderVidaInimigo()
    {
        vidaInimigo--;
        Debug.Log("Inimigo perdeu uma vida! Vidas restantes: " + vidaInimigo);
        AtualizarVidaUI();
        VerificarFimDeJogo();
    }

    private void AtualizarVidaUI()
    {
        if (vidaJogadorText != null)
            vidaJogadorText.text = "" + vidaJogador;

        if (vidaInimigoText != null)
            vidaInimigoText.text =  "" + vidaInimigo;
    }

     private void VerificarFimDeJogo()
    {
        if (vidaJogador <= 0)
        {
            MostrarFimDeJogo("Você perdeu!");
        }
        else if (vidaInimigo <= 0)
        {
            MostrarFimDeJogo("Você venceu!");
        }
    }

    private void MostrarFimDeJogo(string mensagem)
    {
        if (telaFimDeJogo != null)
        {
            telaFimDeJogo.SetActive(true);
            textoFimDeJogo.text = mensagem;
        }

        // Espera alguns segundos antes de trocar de cena
        StartCoroutine(VoltarParaOMenu());
    }

    private IEnumerator VoltarParaOMenu()
    {
        yield return new WaitForSeconds(3f); // Tempo para o jogador ler a mensagem
        SceneManager.LoadScene("MenuScene");
    }
}
