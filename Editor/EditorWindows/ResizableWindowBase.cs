using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.EditorWindows
{
    /// <summary>
    /// Base class for editor windows with resizable content in horizontal layout
    /// </summary>
    public abstract class ResizableWindowBase : EditorWindow
    {
        protected float leftPanelWidthMin = 250f;
        protected float rightPanelWidthMin = 350f;
        protected float panelHeightMin = 400f;
        
        protected float leftPaneWidth = 250f;
        protected float rightPanelWidth = 350f;
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
            GUILayout.BeginArea(new Rect(0, 0, leftPaneWidth, position.height));
            OnGUILeftSide();
            GUILayout.EndArea();
        }

        protected void DrawRightPane()
        {
            GUILayout.BeginArea(new Rect(leftPaneWidth + SeparatorWidth, 0, rightPanelWidth, position.height));
            OnGuiRightSide();
            GUILayout.EndArea();
        }
        
        /// <summary>
        /// Vertical separator
        /// </summary>
        protected void DrawVerticalSeparator()
        {
            Rect separatorRect = new Rect(leftPaneWidth, 0, SeparatorWidth, position.height);
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
            Rect separatorRect = new Rect(leftPaneWidth, 0, SeparatorWidth, position.height);

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
                        float delta = e.delta.x;
                        
                        float newLeftWidth = Mathf.Clamp(leftPaneWidth + delta, leftPanelWidthMin,
                            position.width - rightPanelWidthMin - SeparatorWidth);
                        
                        float newRightWidth = Mathf.Clamp(position.width - newLeftWidth - SeparatorWidth,
                            rightPanelWidthMin, position.width - leftPanelWidthMin - SeparatorWidth);

                        leftPaneWidth = newLeftWidth;
                        rightPanelWidth = newRightWidth;
                        
                        Repaint();
                    }
                    break;
                }
            }
        }
    }
}