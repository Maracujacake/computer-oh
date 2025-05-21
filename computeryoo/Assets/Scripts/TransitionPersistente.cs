using UnityEngine;

public class TransicaoData : MonoBehaviour
{
    public static TransicaoData Instance;

    public Sprite imagemCartaJogador;
    public Sprite imagemCartaInimigo;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
