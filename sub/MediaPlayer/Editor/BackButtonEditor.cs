using UnityEditor;
using UnityEditor.UI;

namespace MediaPlayer
{
	[CustomEditor(typeof(BackButton),true)]
	[CanEditMultipleObjects]
	public class BackButtonEditor : ButtonEditor
	{
		private SerializedProperty background;
		private SerializedProperty icon;
		private SerializedProperty backgroundNormal;
		private SerializedProperty backgroundHighlighted;

		protected override void OnEnable()
		{
			base.OnEnable();
			background = serializedObject.FindProperty("background");
			icon = serializedObject.FindProperty("icon");
			backgroundNormal = serializedObject.FindProperty("backgroundNormal");
			backgroundHighlighted = serializedObject.FindProperty("backgroundHighlighted");
		}
	
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(background);
			EditorGUILayout.PropertyField(icon);
			EditorGUILayout.PropertyField(backgroundNormal);
			EditorGUILayout.PropertyField(backgroundHighlighted);
			serializedObject.ApplyModifiedProperties();
			EditorGUILayout.Space();
			base.OnInspectorGUI();
		}
	}
}