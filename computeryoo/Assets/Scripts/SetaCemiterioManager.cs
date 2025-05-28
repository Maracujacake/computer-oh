using UnityEngine;
using DG.Tweening;
using computeryo;

public class SetaCemiterioManager : MonoBehaviour
{
    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Inicializar(Vector3 origem, Vector3 destino)
    {
        /*
        if (rt == null)
        {
            Debug.LogError("RectTransform não encontrado no objeto SetaCemiterioManager.");
            return;
        }
        Debug.Log($"Inicializando seta do cemitério de {origem} para {destino}.");
        */

        // Define a posição inicial
        transform.localPosition = origem;

        // Calcula direção e distância
        Vector3 direcao = (destino - origem).normalized;
        float distancia = Vector3.Distance(origem, destino);
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        // Rotaciona a seta em direção ao destino
        // + 180f para compensar a imagem apontando para a esquerda
        transform.rotation = Quaternion.Euler(0, 0, angulo + 180f);

        // Começa com a seta com comprimento 0
        rt.sizeDelta = new Vector2(0, rt.sizeDelta.y);

        // Cria a sequência de animação
        Sequence seq = DOTween.Sequence();
        float duracao = 0.5f;

        // Move a seta até o destino e aumenta seu tamanho horizontal (X)
        seq.Append(rt.DOSizeDelta(new Vector2(distancia, rt.sizeDelta.y), duracao).SetEase(Ease.OutSine));
        seq.Join(rt.DOLocalMove(destino, duracao).SetEase(Ease.InOutSine));
        seq.OnComplete(() =>
        {
            Destroy(gameObject); // Seta some ao chegar
            Debug.Log("Seta do cemitério destruída após completar animação.");
        });
    }


    /// <summary>
    /// Cria a seta da posição de origem até o cemitério do jogador ou inimigo.
    /// </summary>
    public static void CriarSetaParaCemiterio(Vector3 origemCanvas, bool pertenceAoJogador, GameObject prefabSeta)
    {
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas não encontrado.");
            return;
        }

        GameObject cemiterioGO = pertenceAoJogador
            ? GameObject.Find("CemiterioJogador/CemiterioFundo")
            : GameObject.Find("CemiterioInimigo/CemiterioFundo");

        if (cemiterioGO == null)
        {
            Debug.LogError("Cemitério não encontrado.");
            return;
        }

        RectTransform rtCemiterio = cemiterioGO.GetComponent<RectTransform>();
        if (rtCemiterio == null)
        {
            Debug.LogError("RectTransform não encontrado no cemitério.");
            return;
        }

        Vector3 destinoCanvas = SetaAtaqueManager.ObterPosicaoNoCanvas(rtCemiterio, canvas);

        // Instancia e anima
        GameObject setaGO = GameObject.Instantiate(prefabSeta, canvas.transform);
        SetaCemiterioManager seta = setaGO.GetComponent<SetaCemiterioManager>();
        /*
        if(seta == null)
        {
            Debug.LogError("SetaCemiterioManager não encontrado no prefab.");
            return;
        }
        */
        seta.Inicializar(origemCanvas, destinoCanvas);
    }
}
