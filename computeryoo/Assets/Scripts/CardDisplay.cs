using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using computeryo;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Card cardData;

    [Header("Verso da Carta")]
    public Image versoImage;

    public DonoCarta dono; 
    public TMP_Text custoTexto;
    public TMP_Text poderTexto;
    public TMP_Text vidaTexto;
    public TMP_Text DescricaoTexto;
    public TMP_Text NomeTexto;

    public Image cardBackgroundImage; // fundo da carta (CardImage) MUDA A COR
    public Image imagemComponent; // imagem da pessoa (Imagem)

    public Image[] tipoImagens = new Image[4];

    [Header("Highlight e Zoom")]
    public GameObject highlightImage;  // imagem de destaque no prefab
    public Image zoomImage;            // imagem no painel de zoom lateral

    void Start()
    {   
        updateCardDisplay();
        if (highlightImage != null)
            highlightImage.SetActive(false);
        if (zoomImage != null)
            zoomImage.gameObject.SetActive(false);
    }

    public void AtualizarDono(DonoCarta novoDono)
    {
        dono = novoDono;
    }

    public void AtualizarVisibilidadeVerso(bool estaNaMao)
    {
        if (versoImage != null)
            versoImage.gameObject.SetActive(estaNaMao);
    }

    public void updateCardDisplay()
    {
        custoTexto.text = cardData.custo.ToString();
        DescricaoTexto.text = cardData.descricao;
        NomeTexto.text = cardData.nome;

        // desativa todos
        foreach (var img in tipoImagens)
        {
            img.gameObject.SetActive(false);
        }

        // Verifica se é uma carta com poder e vida (ex: PersonCard)
        if (cardData is PersonCard person)
        {
            poderTexto.text = person.poder.ToString();
            vidaTexto.text = person.vida.ToString();

            // Muda a cor de fundo de acordo com o tipo
            switch (person.tipo)
            {
                case PersonCard.TipoCarta.Teorico:
                    cardBackgroundImage.color = new Color(0.0588f, 0.5686f, 0.5373f);
                    tipoImagens[0].gameObject.SetActive(true);
                    break;
                case PersonCard.TipoCarta.Engenheiro:
                    cardBackgroundImage.color = new Color(0.7569f, 0.7529f, 0.3059f);
                    tipoImagens[1].gameObject.SetActive(true);
                    break;
                case PersonCard.TipoCarta.Hacker:
                    cardBackgroundImage.color = new Color(0f, 0.5294f, 0.0118f);
                    tipoImagens[2].gameObject.SetActive(true);
                    break;
                case PersonCard.TipoCarta.Visionario:
                    cardBackgroundImage.color = new Color(0f, 0.3843f, 0.9372f);
                    tipoImagens[3].gameObject.SetActive(true);
                    break;
            }
        }

        // Muda o sprite da imagem principal da carta
        if (cardData.imagem != null)
        {
            imagemComponent.sprite = cardData.imagem;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightImage != null)
            highlightImage.SetActive(true);

        bool estaNaMaoInimigo = dono == DonoCarta.Inimigo;
        if (cardData != null)
            ZoomManager.Instance.ShowZoom(cardData, estaNaMaoInimigo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightImage != null)
            highlightImage.SetActive(false);

        ZoomManager.Instance.HideZoom();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var cartaSelecionada = GameManager.Instance.cartaAtacanteSelecionada;

        // Se estivermos na fase de combate...
        if (TurnManager.Instance.faseAtual == TurnManager.Fase.Combate)
        {
            if (cartaSelecionada == null)
            {
                Debug.Log("Você não pode selecionar cartas durante a fase de combate.");
                return;
            }

            // Verifica se a carta clicada está em um dos slots do inimigo
            foreach (var par in FieldManager.Instance.cartasNosSlots)
            {
                if (par.Value == cardData && FieldManager.Instance.slotsMonstrosInimigo.Any(slot => slot.nome == par.Key))
                {
                    string nomeDoSlot = par.Key;

                    GameObject slotGO = GameObject.Find(nomeDoSlot);
                    if (slotGO != null && slotGO.TryGetComponent(out SlotClickHandler slotHandler))
                    {
                        // Redireciona o clique para o slot
                        slotHandler.OnPointerClick(eventData);
                    }
                    else
                    {
                        Debug.LogWarning($"Slot '{nomeDoSlot}' não encontrado ou não possui SlotClickHandler.");
                    }
                    return;
                }
            }

            Debug.Log("Durante o combate, só é possível clicar em cartas no campo do inimigo como alvo.");
            return;
        }

        // Fora da fase de combate: comportamento normal
        GameManager.Instance.SelecionarCarta(this);
    }

}
