using ACTSkillEditor;
using UnityEditor;
using UnityEngine;

namespace ACTSkillDemo
{
    [CustomEditor(typeof(MachineController), true)]
    public class MachineControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Open ACT Skill Editor Window"))
            {
                MachineController controller = target as MachineController;
                if (controller)
                {
                    ACTSkillEditorWindow.ShowEditor(controller.gameObject, serializedObject.FindProperty("configAsset").objectReferenceValue as TextAsset);
                }
            }
        }
    }
}
