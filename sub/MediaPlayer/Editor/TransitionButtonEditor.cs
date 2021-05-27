
using UnityEditor;
using UnityEditor.UI;

namespace MediaPlayer
{
	[CustomEditor(typeof(TransitionButton),true)]
	[CanEditMultipleObjects]
	public class CommonButtonEditor : ButtonEditor
	{
		private SerializedProperty background;
		private SerializedProperty icon;
		private SerializedProperty label;

		protected override void OnEnable()
		{
			base.OnEnable();
			background = serializedObject.FindProperty("background");
			icon = serializedObject.FindProperty("icon");
			label = serializedObject.FindProperty("label");
		}
	
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(background);
			EditorGUILayout.PropertyField(icon);
			EditorGUILayout.PropertyField(label);
			serializedObject.ApplyModifiedProperties();
			EditorGUILayout.Space();
			base.OnInspectorGUI();
		}
	}
}

