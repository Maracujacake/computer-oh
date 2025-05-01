using System.Collections.Generic;
using UnityEngine;
using computeryo;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;
    public List<string> slotsQueJaAtacaram = new List<string>();

    public void ResetarAtaques()
    {
        slotsQueJaAtacaram.Clear();
    }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Realiza o combate entre duas cartas em slots diferentes.
    /// </summary>
    /// <param name="slotAtacante">Nome do slot da carta atacante</param>
    /// <param name="slotDefensor">Nome do slot da carta defensora</param>
   public void ResolverCombate(string slotAtacante, string slotDefensor)
    {

        if(slotsQueJaAtacaram.Contains(slotAtacante))
        {
            Debug.Log("Essa carta já atacou nesta rodada.");
            return;
        }
        
        if (TurnManager.Instance.faseAtual != TurnManager.Fase.Combate)
        {
            Debug.Log("Não é a fase de combate, não é possível atacar.");
            return;
        }

        
        Debug.Log($"Iniciando combate");
        if (!FieldManager.Instance.cartasNosSlots.TryGetValue(slotAtacante, out Card atacanteBase) ||
            !FieldManager.Instance.cartasNosSlots.TryGetValue(slotDefensor, out Card defensorBase))
        {
            Debug.Log("Um dos slots está vazio!");
            return;
        }

        if (atacanteBase is not PersonCard atacante || defensorBase is not PersonCard defensor)
        {
            Debug.Log("Uma das cartas não é do tipo PersonCard. Combate não pode ocorrer.");
            return;
        }

        Debug.Log($"Atacante: {atacante.nome} (ATK {atacante.poder}) VS Defensor: {defensor.nome} (DEF {defensor.vida})");

        if (atacante.poder > defensor.vida)
        {
            Sprite imagemDefensor = EncontrarSlot(slotDefensor).imagemSlot.sprite;
            bool defensorEhJogador = FieldManager.Instance.SlotPertenceAoJogador(slotDefensor);
            
            CemiterioManager.Instance.AdicionarAoCemiterio(defensor, imagemDefensor, defensorEhJogador);

            if (FieldManager.Instance.SlotPertenceAoJogador(slotDefensor))
            {
                BattleResolutionManager.Instance.PerderVidaJogador();
            }
            else if (FieldManager.Instance.SlotPertenceAoInimigo(slotDefensor))
            {
                BattleResolutionManager.Instance.PerderVidaInimigo();
            }

            RemoverCartaDoSlot(slotDefensor);
            
        }
        else if (atacante.poder < defensor.vida)
        {
            int novaVida = defensor.vida - atacante.poder;
            Debug.Log($"{defensor.nome} sobreviveu com {novaVida} de defesa!");

            defensor.vida = novaVida;
            AtualizarCartaVisual(slotDefensor, defensor);
        }
        else
        {
            Sprite imagemAtacante = EncontrarSlot(slotAtacante).imagemSlot.sprite;
            Sprite imagemDefensor = EncontrarSlot(slotDefensor).imagemSlot.sprite;

            bool atacanteEhJogador = FieldManager.Instance.SlotPertenceAoJogador(slotAtacante);
            bool defensorEhJogador = FieldManager.Instance.SlotPertenceAoJogador(slotDefensor);

            CemiterioManager.Instance.AdicionarAoCemiterio(atacante, imagemAtacante, atacanteEhJogador);
            CemiterioManager.Instance.AdicionarAoCemiterio(defensor, imagemDefensor, defensorEhJogador);

            
            Debug.Log("Empate! Ambas as cartas foram destruídas.");
            Debug.Log("Testando um log aqui, ignorar");
            RemoverCartaDoSlot(slotAtacante);
            RemoverCartaDoSlot(slotDefensor);
        }
        slotsQueJaAtacaram.Add(slotAtacante);
    }

    private void RemoverCartaDoSlot(string nomeSlot)
    {
        FieldManager.FieldSlot slot = EncontrarSlot(nomeSlot);

        if (slot != null)
        {
            slot.imagemSlot.sprite = slot.spriteOriginal;
            slot.ocupado = false;
        }

        FieldManager.Instance.cartasNosSlots.Remove(nomeSlot);
    }



    private FieldManager.FieldSlot EncontrarSlot(string nomeSlot)
    {
        var listas = new List<List<FieldManager.FieldSlot>>
        {
            FieldManager.Instance.slotsMonstros,
            FieldManager.Instance.slotsFeiticos,
            FieldManager.Instance.slotsMonstrosInimigo,
            FieldManager.Instance.slotsFeiticosInimigo
        };

        foreach (var lista in listas)
        {
            var slot = lista.Find(s => s.nome == nomeSlot);
            if (slot != null)
                return slot;
        }

        Debug.Log("Slot não encontrado: " + nomeSlot);
        return null;
    }


    private void AtualizarCartaVisual(string slot, Card novaCarta)
    {
        // Aqui você pode atualizar o visual da carta no slot (poder/vida atualizados, etc.)
        // Isso depende de como seu prefab da carta está estruturado.
        // Exemplo: procurar o GameObject da carta e chamar um update no CardDisplay, se estiver acessível
        Transform slotTransform = GameObject.Find(slot)?.transform;
        if (slotTransform != null)
        {
            CardDisplay display = slotTransform.GetComponentInChildren<CardDisplay>();
            if (display != null)
            {
                display.cardData = novaCarta;
                display.updateCardDisplay();
            }
        }
    }


    public void AtaqueDireto(bool atacanteEhJogador)
    {
        if (TurnManager.Instance.contadorTurnos <= 1)
        {
            Debug.Log("Ataque direto não é permitido no primeiro round.");
            return;
        }

        if (TurnManager.Instance.faseAtual != TurnManager.Fase.Combate)
        {
            Debug.Log("Não é a fase de combate.");
            return;
        }

        var slotsAtacantes = atacanteEhJogador 
            ? FieldManager.Instance.slotsMonstros 
            : FieldManager.Instance.slotsMonstrosInimigo;

        var slotsDefensores = atacanteEhJogador 
            ? FieldManager.Instance.slotsMonstrosInimigo 
            : FieldManager.Instance.slotsMonstros;

        if (!slotsDefensores.Exists(slot => slot.ocupado))
        {
            foreach (var slot in slotsAtacantes)
            {
                if (slot.ocupado && !slotsQueJaAtacaram.Contains(slot.nome))
                {
                    slotsQueJaAtacaram.Add(slot.nome);
                    Debug.Log("Ataque direto realizado por " + (atacanteEhJogador ? "jogador" : "inimigo") + " com slot: " + slot.nome);

                    if (atacanteEhJogador)
                    {
                        BattleResolutionManager.Instance.PerderVidaInimigo();
                    }
                    else
                    {
                        BattleResolutionManager.Instance.PerderVidaJogador();
                    }
                }
            }
        }
        else
        {
            Debug.Log("Ataque direto não permitido: defensor ainda possui monstros.");
        }
    }
}
