

using UnityEditor;
using UnityEditor.UI;

namespace MediaPlayer
{
	[CustomEditor(typeof(ColorToggle),true)]
	[CanEditMultipleObjects]
	public class ColorToggleEditor : ToggleEditor
	{
		private SerializedProperty background;
		private SerializedProperty icon;

		protected override void OnEnable()
		{
			base.OnEnable();
			background = serializedObject.FindProperty("background");
			icon = serializedObject.FindProperty("icon");
		}
	
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.PropertyField(background);
			EditorGUILayout.PropertyField(icon);
			serializedObject.ApplyModifiedProperties();
			EditorGUILayout.Space(); 
			base.OnInspectorGUI();
		}
	}
}

