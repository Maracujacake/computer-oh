#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using computeryo;

[CustomEditor(typeof(EnemyDeckManager))]
public class EnemyDeckManagerEditor : Editor
{
    /*
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyDeckManager enemyDeckManager = (EnemyDeckManager)target;
        if (GUILayout.Button("Puxar Carta do Inimigo (Editor)"))
        {
            enemyDeckManager.PuxarCarta();
        }
    }
    */
}
#endif
