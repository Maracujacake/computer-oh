using UnityEngine;

namespace computeryo
{
    [CreateAssetMenu(fileName = "New Algorithm Card", menuName = "Cards/Algorithm Card")]
    public class AlgorithmCard : Card // funcionam como cartas magia
    {

        public enum Rank
        {
            Comum,
            Avancado,
            Especial,
            Unico // só pode usar uma vez na partida
        }

        public Rank rank;
        public string efeito; 
    }
}
