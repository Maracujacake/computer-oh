using UnityEngine;
using UnityEngine.EventSystems;
using computeryo;

public class SlotZoomHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string nomeDoSlot; // Nome deve ser igual à chave usada no dicionário cartasNosSlots

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (FieldManager.Instance.cartasNosSlots.TryGetValue(nomeDoSlot, out Card carta))
        {
            ZoomManager.Instance.ShowZoom(carta);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ZoomManager.Instance.HideZoom();
    }
}
