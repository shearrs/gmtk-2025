#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shears
{
    internal class InterfaceReferenceUtil
    {
        private static GUIStyle labelStyle;

        public static void OnGUI(Rect position, SerializedProperty property, GUIContent label, InterfaceArgs args)
        {
            InitializeStyleIfNeeded();

            int controlID = GUIUtility.GetControlID(FocusType.Passive) - 1;
            bool isHovering = position.Contains(Event.current.mousePosition);
            string displayString =
                property.objectReferenceValue == null || isHovering
                ? $"({args.InterfaceType.Name})"
                : "*";

            DrawInterfaceNameLabel(position, displayString, controlID);
        }

        private static void DrawInterfaceNameLabel(Rect position, string displayString, int controlID)
        {
            if (Event.current.type == EventType.Repaint)
            {
                const int additionalLeftWidth = 3;
                const int verticalIndent = 1;

                GUIContent content = EditorGUIUtility.TrTextContent(displayString);
                Vector2 size = labelStyle.CalcSize(content);
                Rect labelPos = position;

                labelPos.width = size.x + additionalLeftWidth;
                labelPos.x += position.width - labelPos.width - 18;
                labelPos.height -= verticalIndent * 2;
                labelPos.y += verticalIndent;

                labelStyle.Draw(labelPos, content, controlID, DragAndDrop.activeControlID == controlID, false);
            }
        }

        private static void InitializeStyleIfNeeded()
        {
            if (labelStyle != null)
                return;

            var style = new GUIStyle(EditorStyles.label)
            {
                font = EditorStyles.objectField.font,
                fontSize = EditorStyles.objectField.fontSize,
                fontStyle = EditorStyles.objectField.fontStyle,
                alignment = TextAnchor.MiddleRight,
                padding = new RectOffset(0, 2, 0, 0)
            };

            labelStyle = style;
        }
    }
}
#endif