using UnityEngine;

namespace computeryo{

    public class Card : ScriptableObject
    {
        public string nome;
        public int custo;
        public Sprite  imagem;
        public string descricao;

        // status de combate
        public bool atacou = false;
    }
}
