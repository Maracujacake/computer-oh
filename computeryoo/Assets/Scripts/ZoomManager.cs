using UnityEngine;
using computeryo;

public class ZoomManager : MonoBehaviour
{
    public static ZoomManager Instance;

    public Transform zoomArea; // local no Canvas onde o zoom será mostrado
    public GameObject zoomCardGO; // referência da carta de zoom atualmente instanciada

    public GameObject cardPrefab; // mesmo prefab usado nas cartas da mão

    void Awake()
    {
        Instance = this;
    }

    public void ShowZoom(Card cardData, bool estaNaMaoInimigo)
    {
        if (zoomCardGO != null) Destroy(zoomCardGO);

        zoomCardGO = Instantiate(cardPrefab, zoomArea);
        CardDisplay display = zoomCardGO.GetComponent<CardDisplay>();
        display.cardData = cardData;
        display.AtualizarVisibilidadeVerso(estaNaMao: estaNaMaoInimigo);
            
        display.updateCardDisplay();

        // Ajuste opcional de escala
        zoomCardGO.transform.localScale = Vector3.one * 3f;
    }

    public void HideZoom()
    {
        if (zoomCardGO != null)
        {
            Destroy(zoomCardGO);
            zoomCardGO = null;
        }
    }
}
