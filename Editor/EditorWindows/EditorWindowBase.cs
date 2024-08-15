using UnityEditor;

namespace UtilsToolbox.Editor.EditorWindows
{
    public abstract class EditorWindowBase : EditorWindow
    {
        protected virtual void OnEnable()
        {
            SetSize();
        }

        protected abstract void SetSize();
    }
}