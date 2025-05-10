using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using computeryo;

public class FieldManager : MonoBehaviour
{

    public static FieldManager Instance;

    [System.Serializable]
    public class FieldSlot
    {
        public string nome;
        public Image imagemSlot;
        public bool ocupado;

        [HideInInspector]
        public Sprite spriteOriginal;
    }

    public List<FieldSlot> slotsMonstros = new(); // FieldMonster1 até 5
    public List<FieldSlot> slotsFeiticos = new(); // FieldSpell1 e 2
    public List<FieldSlot> slotsMonstrosInimigo = new();
    public List<FieldSlot> slotsFeiticosInimigo = new();

    public Color corOriginal = Color.white;
    public Color corHighlight = Color.yellow;

    // Dicionário para armazenar as cartas nos slots
    public Dictionary<string, Card> cartasNosSlots = new();

    // Salva os sprites originais dos slots
    void Start()
    {
        AdicionarHoverZoom(slotsMonstros);
        AdicionarHoverZoom(slotsFeiticos);
        AdicionarHoverZoom(slotsMonstrosInimigo);
        AdicionarHoverZoom(slotsFeiticosInimigo);
        SalvarSpritesOriginais(slotsMonstros);
        SalvarSpritesOriginais(slotsFeiticos);
        SalvarSpritesOriginais(slotsMonstrosInimigo);
        SalvarSpritesOriginais(slotsFeiticosInimigo);

    }

    void SalvarSpritesOriginais(List<FieldSlot> lista)
    {
        foreach (var slot in lista)
        {
            if (slot.imagemSlot != null)
                slot.spriteOriginal = slot.imagemSlot.sprite;
        }
    }

    void AdicionarHoverZoom(List<FieldSlot> lista)
    {
        foreach (var slot in lista)
        {
            if (slot.imagemSlot != null)
            {
                var zoom = slot.imagemSlot.GetComponent<SlotZoomHover>();
                if (zoom == null)
                    zoom = slot.imagemSlot.gameObject.AddComponent<SlotZoomHover>();

                zoom.nomeDoSlot = slot.nome;
            }
        }
    }


    void Awake()
    {
        Instance = this;
    }

    // Destaca os slots livres de monstro
    public void HighlightSlotsMonstroDisponiveis()
    {
        // Verifica se o dono da carta é o jogador ou inimigo e escolhe a lista correspondente
        List<FieldSlot> slots = GameManager.Instance.CartaSelecionada.dono == DonoCarta.Jogador ? slotsMonstros : slotsMonstrosInimigo;
        foreach(var slot in slots ){
           Debug.Log("Slot: " + slot.nome + " Ocupado: " + slot.ocupado);
        }
        // Para as cartas de feitico
        List<FieldSlot> slotsFeiticosCorrespondentes = GameManager.Instance.CartaSelecionada.dono == DonoCarta.Jogador ? slotsFeiticos : slotsFeiticosInimigo;

        // Destaca os slots de monstros
        foreach (var slot in slots)
        {
            if (!slot.ocupado)
                slot.imagemSlot.color = corHighlight;
        }

        // Destaca os slots de feiticos
        /*
        foreach (var slot in slotsFeiticosCorrespondentes)
        {
            if (!slot.ocupado)
                slot.imagemSlot.color = corHighlight;
        }
        */
    }

    // Limpa os destaques de todos os slots
    public void LimparHighlights()
    {
        foreach (var slot in slotsMonstros)
            slot.imagemSlot.color = corOriginal;
        foreach (var slot in slotsMonstrosInimigo)
            slot.imagemSlot.color = corOriginal;


        foreach (var slot in slotsFeiticos)
            slot.imagemSlot.color = corOriginal;
    }

    // Checa se um slot específico está disponível
    public bool EstaDisponivel(string nomeSlot, DonoCarta? dono = null)
        {
            List<FieldSlot> slots;

        if (dono.HasValue)
        {
            slots = dono.Value == DonoCarta.Jogador ? slotsMonstros : slotsMonstrosInimigo;
        }
        else
        {
            if (GameManager.Instance.CartaSelecionada == null)
            {
                Debug.LogError("CartaSelecionada está nula e nenhum dono foi fornecido!");
                return false;
            }

            slots = GameManager.Instance.CartaSelecionada.dono == DonoCarta.Jogador ? slotsMonstros : slotsMonstrosInimigo;
        }

        var slot = slots.Find(s => s.nome == nomeSlot);
        if (slot == null)
        {
            Debug.LogError("Slot não encontrado: " + nomeSlot);
            return false;
        }

        return !slot.ocupado;
    }

    // Marca um slot como ocupado
    public void OcupaSlot(string nomeSlot, DonoCarta? dono = null)
    {
        if (TurnManager.Instance.faseAtual != TurnManager.Fase.Preparacao)
        {
            Debug.Log("Não é a fase de preparação, você não pode posicionar cartas.");
            return;
        }

        DonoCarta donoFinal = dono ?? GameManager.Instance.CartaSelecionada.dono;

        List<FieldSlot> slots = donoFinal == DonoCarta.Jogador
            ? slotsMonstros
            : slotsMonstrosInimigo;

        var slot = slots.Find(s => s.nome == nomeSlot);
        if (slot != null)
        {   
            Debug.Log("Ocupando slot: " + nomeSlot);
            slot.ocupado = true;
            slot.imagemSlot.color = corOriginal;
        }
    }

    public FieldSlot ObterSlot(string nomeSlot)
    {
        return slotsMonstros.Find(s => s.nome == nomeSlot) ?? slotsFeiticos.Find(s => s.nome == nomeSlot);
    }

    public bool SlotPertenceAoJogador(string nomeDoSlot)
    {
        return slotsMonstros.Find(slot => slot.nome == nomeDoSlot) != null;
    }

    public bool SlotPertenceAoInimigo(string nomeDoSlot)
    {
        return slotsMonstrosInimigo.Find(slot => slot.nome == nomeDoSlot) != null;
    }


    // ****** FUNÇÕES PARA COMBATE *******

    public void HighlightSlotsMonstroInimigoOcupados()
    {
        foreach (var slot in slotsMonstrosInimigo)
        {
            if (slot.ocupado)
                slot.imagemSlot.color = Color.red;
        }
    }

    public Card ObterCartaNoSlot(string nomeDoSlot)
    {
        if (cartasNosSlots.TryGetValue(nomeDoSlot, out Card carta))
            return carta;
        return null;
    }

}


