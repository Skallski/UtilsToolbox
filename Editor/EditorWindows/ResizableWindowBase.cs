using UnityEditor;
using UnityEngine;

namespace SkalluUtils.Editor.EditorWindows
{
    /// <summary>
    /// Base class for editor windows with resizable content in horizontal layout
    /// </summary>
    public abstract class ResizableWindowBase : EditorWindowBase
    {
        protected virtual float LeftPanelWidthMin => 250f;
        protected virtual float RightPanelWidthMin => 350f;
        protected virtual float PanelHeightMin => 400f;

        private float _leftPanelWidth;
        private float _rightPanelWidth;
        
        private const float SeparatorWidth = 5f;
        private bool _isResizing;

        protected override void SetSize()
        {
            _leftPanelWidth = LeftPanelWidthMin;
            _rightPanelWidth = RightPanelWidthMin;
            
            minSize = new Vector2(LeftPanelWidthMin + RightPanelWidthMin, PanelHeightMin);
        }

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
            GUILayout.BeginArea(new Rect(0, 0, _leftPanelWidth, position.height));
            OnGUILeftSide();
            GUILayout.EndArea();
        }

        protected void DrawRightPane()
        {
            GUILayout.BeginArea(new Rect(_leftPanelWidth + SeparatorWidth, 0, _rightPanelWidth, position.height));
            OnGuiRightSide();
            GUILayout.EndArea();
        }
        
        /// <summary>
        /// Vertical separator
        /// </summary>
        protected void DrawVerticalSeparator()
        {
            Rect separatorRect = new Rect(_leftPanelWidth, 0, SeparatorWidth, position.height);
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
            Rect separatorRect = new Rect(_leftPanelWidth, 0, SeparatorWidth, position.height);

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
                        
                        float newLeftWidth = Mathf.Clamp(_leftPanelWidth + delta, LeftPanelWidthMin,
                            position.width - RightPanelWidthMin - SeparatorWidth);
                        
                        float newRightWidth = Mathf.Clamp(position.width - newLeftWidth - SeparatorWidth,
                            RightPanelWidthMin, position.width - LeftPanelWidthMin - SeparatorWidth);

                        _leftPanelWidth = newLeftWidth;
                        _rightPanelWidth = newRightWidth;
                        
                        Repaint();
                    }
                    break;
                }
            }
        }
    }
}