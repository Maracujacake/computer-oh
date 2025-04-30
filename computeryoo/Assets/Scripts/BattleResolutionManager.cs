using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using computeryo;

public class BattleResolutionManager : MonoBehaviour
{
    public static BattleResolutionManager Instance { get; private set; }

    public int vidaJogador = 5;
    public int vidaInimigo = 5;

    public TMP_Text vidaJogadorText; 
    public TMP_Text vidaInimigoText; 

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
            Debug.Log("Jogador perdeu o jogo!");
            // Aqui você pode chamar tela de derrota
        }
        else if (vidaInimigo <= 0)
        {
            Debug.Log("Jogador venceu o jogo!");
            // Aqui você pode chamar tela de vitória
        }
    }
}
