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

    void Start()
    {
        DeckData[] decksEncontrados = Resources.LoadAll<DeckData>("Decks");
        deckOptions = new List<DeckData>(decksEncontrados);
        if(deckOptions.Count == 0)
        {
            Debug.LogError("Nenhum deck encontrado na pasta 'Decks'.");
            return;
        }
        deckPadraoIA = deckOptions[0]; // Pega o primeiro deck como padrão para a IA

        // Popular dropdown
        List<string> nomesDecks = new();
        foreach (DeckData deck in deckOptions)
        {
            nomesDecks.Add(deck.nomeDoDeck); // supondo que você tenha um campo "nomeDoDeck" no DeckData
        }
        deckDropdown.AddOptions(nomesDecks);

        EscolherDeck();
        // Garante que só o menu principal aparece no início
        menuPrincipal.gameObject.SetActive(true);
        menuJogo.gameObject.SetActive(false);
        versusIA.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        menuPrincipal.gameObject.SetActive(false);
        menuJogo.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

    // MODO VS. IA
    public void VSIA()
    {
        menuPrincipal.gameObject.SetActive(false);
        menuJogo.gameObject.SetActive(false);
        versusIA.gameObject.SetActive(true);
    }

    public void VoltarIA()
    {
        menuPrincipal.gameObject.SetActive(false);
        menuJogo.gameObject.SetActive(true);
        versusIA.gameObject.SetActive(false);
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
        }
        else
        {
            Debug.LogError("Índice de deck inválido: " + selectedIndex);
        }
    }

    public void IniciarBatalha()
    {
        // Troca o canvas
        menuPrincipal.gameObject.SetActive(false);
        versusIA.gameObject.SetActive(false);
        menuJogo.gameObject.SetActive(false);

        SceneManager.LoadScene("SampleScene"); // inicia o campo de batalha
    }

    // MODO MULTIPLAYER

    public void Multiplayer()
    {
        Debug.Log("Modo Multiplayer selecionado.");
    }

    public void Voltar()
    {
        menuJogo.gameObject.SetActive(false);
        menuPrincipal.gameObject.SetActive(true);
    }

    public void StartDebugBattle()
    {
        SceneManager.LoadScene("SampleScene"); // inicia o campo de batalha direto
    }
}
