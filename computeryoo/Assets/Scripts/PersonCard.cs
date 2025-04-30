using UnityEngine;

namespace computeryo
{
    [CreateAssetMenu(fileName = "New Person Card", menuName = "Cards/Person Card")]
    public class PersonCard : Card
    {
        public enum TipoCarta
        {
            Teorico,
            Engenheiro,
            Hacker,
            Visionario
        }

        public TipoCarta tipo;
        public int poder;
        public int vida;
    }
}
