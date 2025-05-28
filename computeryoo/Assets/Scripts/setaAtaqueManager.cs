using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SetaAtaqueManager : MonoBehaviour
{
    public Transform destino;
    public float velocidade = 5f;
    private Vector3 origemLocal;
    private Vector3 alvoLocal;
    private bool animando = false;


    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Converte a posição de um RectTransform para o espaço local do Canvas.
    /// </summary>
    public static Vector3 ObterPosicaoNoCanvas(RectTransform alvo, Canvas canvas)
    {

        Camera cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
        {
            cam = canvas.worldCamera;
        }

        Vector2 telaPos = RectTransformUtility.WorldToScreenPoint(cam, alvo.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), telaPos, canvas.worldCamera, out Vector2 localPos);
        return localPos;
    }

    /// <summary>
    /// Inicializa a seta com origem e destino já em espaço do Canvas.
    /// </summary>
    public void Inicializar(Vector3 origem, Vector3 alvo)
    {
        origemLocal = origem;
        alvoLocal = alvo;

        transform.localPosition = origemLocal;

        // Direção e distância
        Vector3 direcao = (alvoLocal - origemLocal).normalized;
        float distanciaTotal = Vector3.Distance(origemLocal, alvoLocal);

        // Rotaciona para apontar para o alvo
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        // Começa com tamanho pequeno (ex: largura 0)
        rt.sizeDelta = new Vector2(0f, rt.sizeDelta.y);

        float duracao = 0.5f;

        // Cria uma sequência de tweens
        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOLocalMove(alvoLocal, duracao).SetEase(Ease.OutQuad));
        seq.Join(rt.DOSizeDelta(new Vector2(distanciaTotal, rt.sizeDelta.y), duracao).SetEase(Ease.OutQuad));
        seq.OnComplete(() =>
        {
            Debug.Log("Seta do cemitério destruída após completar animação.");
            Destroy(gameObject);
        });
    }



    /// <summary>
    /// Método auxiliar para criar e animar a seta entre dois slots usando seus nomes.
    /// </summary>
    public static void CriarSetaDeAtaqueEntreSlots(string slotAtacante, string slotDefensor, GameObject prefabSeta)
    {
        // Referências obrigatórias
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas não encontrado!");
            return;
        }

        // Pega os RectTransform dos GameObjects dos slots
        GameObject slotGOAtacante = FieldManager.Instance.GetSlotGOByName(slotAtacante);
        GameObject slotGODefensor = FieldManager.Instance.GetSlotGOByName(slotDefensor);

        if(slotGOAtacante == null || slotGODefensor == null)
        {
            Debug.LogError("Slot não encontrado pelo nome: " + slotAtacante + " ou " + slotDefensor);
            return;
        }

        RectTransform rtOrigem = slotGOAtacante.GetComponent<RectTransform>();
        RectTransform rtAlvo = slotGODefensor.GetComponent<RectTransform>();

        // Converte posições para o espaço do Canvas
        Vector3 origemCanvas = ObterPosicaoNoCanvas(rtOrigem, canvas);
        Vector3 alvoCanvas = ObterPosicaoNoCanvas(rtAlvo, canvas);

        Debug.Log($"CriarSetaDeAtaqueEntreSlots: origem={origemCanvas}, alvo={alvoCanvas}");
        // Instancia a seta dentro do Canvas
        GameObject setaGO = GameObject.Instantiate(prefabSeta, canvas.transform);
        SetaAtaqueManager seta = setaGO.GetComponent<SetaAtaqueManager>();
        seta.Inicializar(origemCanvas, alvoCanvas);

    }
}
