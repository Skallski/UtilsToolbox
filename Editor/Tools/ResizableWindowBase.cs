using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Tools
{
    /// <summary>
    /// Base class for editor windows with resizable content in horizontal layout
    /// </summary>
    public abstract class ResizableWindowBase : EditorWindow
    {
        private float _leftPaneWidth = 200f;
        private const float SeparatorWidth = 5f;
        private bool _isResizing;

        private void OnGUI()
        {
            DrawGUI();
        }

        protected virtual void DrawGUI()
        {
            HandleResizeEvents();

            EditorGUILayout.BeginHorizontal();
            
            DrawLeftPane();
            DrawVerticalSeparator();
            DrawRightPane();

            EditorGUILayout.EndHorizontal();
        }

        protected void DrawLeftPane()
        {
            GUILayout.BeginArea(new Rect(0, 0, _leftPaneWidth, position.height));
            OnGUILeftSide();
            GUILayout.EndArea();
        }

        protected void DrawRightPane()
        {
            GUILayout.BeginArea(new Rect(
                _leftPaneWidth + SeparatorWidth,
                0,
                position.width - (_leftPaneWidth + SeparatorWidth), position.height));
            
            OnGuiRightSide();
            GUILayout.EndArea();
        }
        
        /// <summary>
        /// Vertical separator
        /// </summary>
        protected void DrawVerticalSeparator()
        {
            Rect separatorRect = new Rect(_leftPaneWidth, 0, SeparatorWidth, position.height);
            EditorGUIUtility.AddCursorRect(separatorRect, MouseCursor.ResizeHorizontal);
            GUI.Box(separatorRect, GUIContent.none, EditorStyles.helpBox);
        }

        /// <summary>
        /// Left pane
        /// </summary>
        protected abstract void OnGUILeftSide();

        /// <summary>
        /// Right pane
        /// </summary>
        protected abstract void OnGuiRightSide();

        private void HandleResizeEvents()
        {
            Event e = Event.current;
            Rect separatorRect = new Rect(_leftPaneWidth, 0, SeparatorWidth, position.height);

            switch (e.type)
            {
                case EventType.MouseDown:
                {
                    if (separatorRect.Contains(e.mousePosition))
                    {
                        _isResizing = true;
                    }
                    break;
                }
                
                case EventType.MouseUp:
                {
                    _isResizing = false;
                    break;
                }
                
                case EventType.MouseDrag:
                {
                    if (_isResizing)
                    {
                        _leftPaneWidth += e.delta.x;
                        Repaint();
                    }
                    break;
                }
            }
        }
    }
}