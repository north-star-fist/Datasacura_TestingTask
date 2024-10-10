using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Datasacura.TestTask.ZooWorld
{

    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor
    {
        private const string BoundingBoxPropertyPath = "_boundingBox";
        private const string ExistenceBoxPropertyPath = "_existenceBox";

        readonly BoxBoundsHandle _boxBoundsHandle = new BoxBoundsHandle();
        readonly BoxBoundsHandle _existBoundsHandle = new BoxBoundsHandle();

        private void OnSceneGUI()
        {
            handleBox(BoundingBoxPropertyPath, _boxBoundsHandle, Color.green);
            handleBox(ExistenceBoxPropertyPath, _existBoundsHandle, Color.blue);


            void handleBox(string fieldPath, BoxBoundsHandle boxHandle, Color color)
            {
                Handles.color = color;
                var boxProperty = serializedObject.FindProperty(fieldPath);
                boxHandle.size = boxProperty.boundsValue.size;
                boxHandle.center = Selection.activeTransform.position + boxProperty.boundsValue.center;

                EditorGUI.BeginChangeCheck();
                boxHandle.DrawHandle();
                serializedObject.ApplyModifiedProperties();
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(serializedObject.targetObject, $"Changed Bounding Box {fieldPath}");
                    boxProperty.boundsValue = new(
                        boxHandle.center - Selection.activeTransform.position,
                        boxHandle.size
                    );
                }
            }
        }
    }
}
