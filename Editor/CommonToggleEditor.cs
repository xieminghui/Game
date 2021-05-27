using UnityEditor;
using UnityEditor.UI;

namespace MediaPlayer
{
    [CustomEditor(typeof(CommonToggle),true)]
    [CanEditMultipleObjects]
    public class CommonToggleEditor : ToggleEditor
    {
        private SerializedProperty background;
        private SerializedProperty icon;
        private SerializedProperty label;
        
        private SerializedProperty hideLabelNormalState;
        
        private SerializedProperty useTextFluidEffect;
        private SerializedProperty textFluidEffect;
        private SerializedProperty languageKey;
        
 
        protected override void OnEnable()
        {
            base.OnEnable();
            background = serializedObject.FindProperty("background");
            icon = serializedObject.FindProperty("icon");
            label = serializedObject.FindProperty("label");
            
            hideLabelNormalState = serializedObject.FindProperty("hideLabelNormalState");
            
            useTextFluidEffect = serializedObject.FindProperty("useTextFluidEffect");
            textFluidEffect = serializedObject.FindProperty("textFluidEffect");
            languageKey = serializedObject.FindProperty("languageKey");
        }
	
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(background);
            EditorGUILayout.PropertyField(icon);
            EditorGUILayout.PropertyField(label);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(hideLabelNormalState);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(useTextFluidEffect);
            if (useTextFluidEffect.boolValue)
            {
                EditorGUILayout.PropertyField(textFluidEffect);
                EditorGUILayout.PropertyField(languageKey);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
}

