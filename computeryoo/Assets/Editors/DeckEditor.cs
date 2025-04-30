#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using computeryo;

[CustomEditor(typeof(DeckManager))]
public class DeckManagerEditor : Editor
{
/*
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DeckManager deckManager = (DeckManager)target;
        if (GUILayout.Button("Puxar Carta (Editor)"))
        {
            deckManager.PuxarCarta();
        }
    }
*/
}
#endif
