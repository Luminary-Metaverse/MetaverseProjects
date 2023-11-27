using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace UnityClassAttribute
{
    
    
    [CustomEditor(typeof(IdentifiableProp))]
    public class IdentifiablePropEditor : Editor {
        private SerializedProperty _property;
        private ReorderableList _list;
    
        private void OnEnable() {
            _property = serializedObject.FindProperty("");
            _list = new ReorderableList(serializedObject, _property, true, true, true, true) {
                drawHeaderCallback = DrawListHeader,
                drawElementCallback = DrawListElement
            };
        }
    
        private void DrawListHeader(Rect rect) {
            GUI.Label(rect, "");
        }
    
        private void DrawListElement(Rect rect, int index, bool isActive, bool isFocused) {
            var item = _property.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, item);
            
        }
    
        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.Space();
            _list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}