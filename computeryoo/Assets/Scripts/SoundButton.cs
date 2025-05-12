using UnityEngine;
using UnityEngine.EventSystems;
using computeryo;

public class SoundButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip somHover;
    public AudioClip somClick;
    private AudioSource audioSource;

    private void Awake()
    {
        // Pega ou adiciona um AudioSource no botão
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configura o áudio
        audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (somHover != null)
        {
            audioSource.PlayOneShot(somHover, 0.5f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (somClick != null)
        {
            audioSource.PlayOneShot(somClick); 
        }
    }
}
