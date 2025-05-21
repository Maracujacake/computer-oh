using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using computeryo;

public class TransicaoManager : MonoBehaviour
{
    public Image imagemJogador;
    public Image imagemInimigo;

    void Start()
    {
        if (TransicaoData.Instance != null)
        {
            imagemJogador.sprite = TransicaoData.Instance.imagemCartaJogador;
            imagemInimigo.sprite = TransicaoData.Instance.imagemCartaInimigo;
        }

        // Aqui inicia sua animação (talvez usando Animator ou Coroutine)
        StartCoroutine(AnimacaoETransicao());
    }

    private IEnumerator AnimacaoETransicao()
    {
        // Espera a animação acontecer (ex: 2 segundos)
        yield return new WaitForSeconds(2f);

        // Vai para a cena da batalha real
        SceneManager.LoadScene("SampleScene");
    }
}
