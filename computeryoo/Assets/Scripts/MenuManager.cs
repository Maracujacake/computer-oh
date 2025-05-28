using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using computeryo;

public class MenuManager : MonoBehaviour
{
    public Canvas menuPrincipal;
    public Canvas menuJogo;
    public Canvas versusIA;
    public List<DeckData> deckOptions = new();
    public Dropdown deckDropdown;
    public Dropdown dificuldadeDropdown;

    public DeckData deckPadraoIA;
    public DeckManager deckManagerJogador;
    public EnemyDeckManager deckManagerIA;

    private const float delayTransicao = 0.7f;

    public DoorTransitionAnimator doorAnimator;


    void Start()
    {
        DeckData[] decksEncontrados = Resources.LoadAll<DeckData>("Decks");
        deckOptions = new List<DeckData>(decksEncontrados);
        if (deckOptions.Count == 0)
        {
            Debug.LogError("Nenhum deck encontrado na pasta 'Decks'.");
            return;
        }
        deckPadraoIA = deckOptions[0];

        List<string> nomesDecks = new();
        foreach (DeckData deck in deckOptions)
        {
            nomesDecks.Add(deck.nomeDoDeck);
        }
        deckDropdown.AddOptions(nomesDecks);

        EscolherDeck();

        menuPrincipal.gameObject.SetActive(true);
        menuJogo.gameObject.SetActive(false);
        versusIA.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        doorAnimator.Play(() =>
        {
            // Isso será chamado exatamente no timing certo via Animation Event
            menuPrincipal.gameObject.SetActive(false);
            menuJogo.gameObject.SetActive(true);
        });
    }

    public void ExitGame()
    {
        DelayedExecutor.Execute(() =>
        {
            Debug.Log("Saindo do jogo...");
            Application.Quit();
        }, delayTransicao);
    }

    public void VSIA()
    {
        
        doorAnimator.Play(() =>
        {
            menuPrincipal.gameObject.SetActive(false);
            menuJogo.gameObject.SetActive(false);
            versusIA.gameObject.SetActive(true);
        });
        
    }

    public void VoltarIA()
    {
        doorAnimator.Play(() =>
        {
            menuPrincipal.gameObject.SetActive(false);
            menuJogo.gameObject.SetActive(true);
            versusIA.gameObject.SetActive(false);
        });
    }

    public void EscolherDificuldade()
    {
        BattleIASettings.dificuldade = dificuldadeDropdown.value;
        Debug.Log("Dificuldade selecionada: " + BattleIASettings.dificuldade);
    }

    public void EscolherDeck()
    {
        int selectedIndex = deckDropdown.value;
        if (selectedIndex >= 0 && selectedIndex < deckOptions.Count)
        {
            BattleIASettings.deckSelecionadoData = deckOptions[selectedIndex];
            BattleIASettings.deckInimigoSelecionadoData = deckPadraoIA;
            Debug.Log("Deck selecionado: " + BattleIASettings.deckSelecionadoData.nomeDoDeck);

            // Gambiarra que resolve o sumiço visual
            versusIA.gameObject.SetActive(false);
            versusIA.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Índice de deck inválido: " + selectedIndex);
        }
    }

    public void IniciarBatalha()
    {
        doorAnimator.Play(null);
        DelayedExecutor.Execute(() =>
        {
            // Carrega o deck
            List<Card> deckJogador = BattleIASettings.deckSelecionadoData.cartas;
            List<Card> deckInimigo = BattleIASettings.deckInimigoSelecionadoData.cartas;

            if (deckJogador.Count > 0 && deckInimigo.Count > 0)
            {
                if (TransicaoData.Instance == null)
                {
                    GameObject obj = new GameObject("TransicaoData");
                    obj.AddComponent<TransicaoData>();
                }

                TransicaoData.Instance.imagemCartaJogador = deckJogador[0].imagem;
                TransicaoData.Instance.imagemCartaInimigo = deckInimigo[0].imagem;
            }

            // Vai para a cena de transição
            SceneManager.LoadScene("TransitionScene");
        }, delayTransicao);
    }

    public void Multiplayer()
    {
        DelayedExecutor.Execute(() =>
        {
            Debug.Log("Modo Multiplayer selecionado.");
        }, delayTransicao);
    }

    public void Voltar()
    {
        doorAnimator.Play(() =>
        {
            menuJogo.gameObject.SetActive(false);
            menuPrincipal.gameObject.SetActive(true);
        });
    }

    public void StartDebugBattle()
    {
        DelayedExecutor.Execute(() =>
        {
            SceneManager.LoadScene("SampleScene");
        }, delayTransicao);
    }
}
