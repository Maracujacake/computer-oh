using UnityEngine;
using computeryo;
using System.Collections;
using System.Linq;


public class BattleIASettings
{
    public static DeckData deckSelecionadoData; // deck da IA
    public static DeckData deckInimigoSelecionadoData; // deck do jogador
    public static int dificuldade;

    public HandManager handManagerIA;

    public BattleIASettings(HandManager handManager)
    {
        this.handManagerIA = handManager;
    }


    // seleciona uma carta 
    public void selecionarCartaMao()
    {
        var maoIA = handManagerIA.cartasNaMaoInimigo;
        if (maoIA.Count == 0)
        {
            Debug.LogWarning("IA não tem cartas na mão.");
            return;
        }

        // Escolhe aleatoriamente um Card da lista (dados da carta, não GameObject)
        Card cartaEscolhida = maoIA[Random.Range(0, maoIA.Count)];

        // Encontra um slot disponível
        foreach (var slot in FieldManager.Instance.slotsMonstrosInimigo)
        {
            string nomeSlot = slot.nome;

            if (!FieldManager.Instance.cartasNosSlots.ContainsKey(nomeSlot))
            {
                Debug.Log($"IA posicionando {cartaEscolhida.nome} no slot {nomeSlot}");

                // Instancia o GameObject da carta a partir do prefab
                GameObject cartaGO = GameObject.Instantiate(handManagerIA.cardPrefab); // ou onde estiver o prefab
                CardDisplay display = cartaGO.GetComponent<CardDisplay>();
                display.cardData = cartaEscolhida;
                display.AtualizarDono(DonoCarta.Inimigo);
                display.updateCardDisplay();

                // Posiciona no campo
                GameManager.Instance.PosicionarCartaInimigo(cartaEscolhida, nomeSlot, cartaGO);

                // Remove da mão (somente os dados)
                handManagerIA.cartasNaMaoInimigo.Remove(cartaEscolhida);
                UnityEngine.GameObject.Destroy(cartaGO); // Destrui a carta que foi instanciada no método selecionarCartaMao
                break;
            }
        }
    }


    public void selecionarSlot(){

        
        var cartasCampoIA = FieldManager.Instance.slotsMonstrosInimigo
            .Where(slot => FieldManager.Instance.cartasNosSlots.ContainsKey(slot.nome) &&
                        FieldManager.Instance.cartasNosSlots[slot.nome] != null)
            .Select(slot => FieldManager.Instance.cartasNosSlots[slot.nome])
            .ToList();

        if (cartasCampoIA.Count == 0) return;

        Card cartaParaAgir = cartasCampoIA[Random.Range(0, cartasCampoIA.Count)];

        // deve selecionar uma carta do jogador e começar a batalha, se nao houver carta
        // no campo do jogador, ele ataca diretamente
        BattleState.Instance.cartaSelecionadaIA = cartaParaAgir;
        Debug.Log($"IA selecionou carta {cartaParaAgir.nome} para agir.");
    }


    public void atacarCartaJogador()
    {
        // Busca cartas do campo da IA (slotsMonstrosInimigo)
        var cartasCampoIA = FieldManager.Instance.slotsMonstrosInimigo
            .Where(slot => FieldManager.Instance.cartasNosSlots.ContainsKey(slot.nome) &&
                        FieldManager.Instance.cartasNosSlots[slot.nome] != null)
            .Select(slot => FieldManager.Instance.cartasNosSlots[slot.nome])
            .ToList();

        if (cartasCampoIA.Count == 0)
        {
            Debug.Log("IA não tem cartas para atacar.");
            return;
        }

        Debug.Log($"IA tem {cartasCampoIA.Count} cartas no campo.");

        // Seleciona uma carta atacante aleatória da IA
        Card cartaAtacante = cartasCampoIA[Random.Range(0, cartasCampoIA.Count)];
        Debug.Log($"IA selecionou carta {cartaAtacante.nome} para atacar.");
        //BattleState.Instance.cartaSelecionadaIA = cartaAtacante;

        // Busca cartas do campo do jogador (slotsMonstros)
        var cartasCampoJogador = FieldManager.Instance.slotsMonstros
            .Where(slot => FieldManager.Instance.cartasNosSlots.ContainsKey(slot.nome) &&
                        FieldManager.Instance.cartasNosSlots[slot.nome] != null)
            .Select(slot => FieldManager.Instance.cartasNosSlots[slot.nome])
            .ToList();

        // Se jogador não tem cartas, ataque direto
        if (cartasCampoJogador.Count == 0)
        {
            Debug.Log("Campo do jogador vazio. IA fará ataque direto.");
            CombatManager.Instance.AtaqueDireto(atacanteEhJogador: false);
            return;
        }
        Debug.Log($"IA tem {cartasCampoJogador.Count} cartas no campo do jogador.");

        // Seleciona uma carta defensora aleatória do jogador
        Card cartaDefensora = cartasCampoJogador[Random.Range(0, cartasCampoJogador.Count)];
        Debug.Log($"IA selecionou carta {cartaDefensora.nome} para atacar.");

        // Localiza os slots das cartas atacante e defensora
        string slotAtacante = FieldManager.Instance.cartasNosSlots.First(par => par.Value == cartaAtacante).Key;
        string slotDefensor = FieldManager.Instance.cartasNosSlots.First(par => par.Value == cartaDefensora).Key;

        // Executa o combate
        Debug.Log($"IA atacou {cartaDefensora.nome} com {cartaAtacante.nome}");
        CombatManager.Instance.ResolverCombate(slotAtacante, slotDefensor);
    }

}
