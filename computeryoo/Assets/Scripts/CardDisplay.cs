using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        var turno = TurnManager.Instance.turnoAtual;

        // Impede seleção fora do turno do dono da carta
        if ((dono == DonoCarta.Jogador && turno != TurnManager.Turno.Jogador) ||
            (dono == DonoCarta.Inimigo && turno != TurnManager.Turno.Inimigo))
        {
            Debug.Log("Você não pode selecionar cartas fora do seu turno.");
            return;
        }

        if (!GameManager.Instance.EstaSelecionandoCarta())
        {
            GameManager.Instance.SelecionarCarta(this);
        }
    }
}
