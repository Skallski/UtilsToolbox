using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UtilsToolbox.Editor.EditorWindows
{
    public class AnimatorControllerStatesDisplay : EditorWindowBase
    {
        private AnimatorController _animationController;
        private Vector2 _scrollPosition = Vector2.zero;

        private bool[] _foldoutExpandedArray;

        protected override void SetSize()
        {
            minSize = new Vector2(275, 400);
        }

        private void OnGUI()
        {
            _animationController = EditorGUILayout.ObjectField("Animator Controller", _animationController,
                typeof(AnimatorController), false) as AnimatorController;

            if (_animationController != null)
            {
                if (_foldoutExpandedArray == null || _foldoutExpandedArray.Length != _animationController.layers.Length)
                {
                    _foldoutExpandedArray = new bool[_animationController.layers.Length];
                    for (int i = 0; i < _foldoutExpandedArray.Length; i++)
                    {
                        _foldoutExpandedArray[i] = true;
                    }
                }

                EditorGUILayout.Space();

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                {
                    AnimatorControllerLayer[] layers = _animationController.layers;
                    for (int i = 0; i < layers.Length; i++)
                    {
                        AnimatorControllerLayer layer = layers[i];

                        _foldoutExpandedArray[i] = EditorGUILayout.Foldout(_foldoutExpandedArray[i],
                            $"{layer.name}:", EditorStyles.foldoutHeader);

                        if (_foldoutExpandedArray[i])
                        {
                            EditorGUI.indentLevel += 2;
                            
                            AnimatorStateMachine stateMachine = layer.stateMachine;
                            ChildAnimatorState[] states = stateMachine.states;
                            foreach (ChildAnimatorState childState in states)
                            {
                                AnimatorState state = childState.state;

                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField($"{state.name}: {state.nameHash}");
                                    if (GUILayout.Button("copy", GUILayout.Width(50)))
                                    {
                                        EditorGUIUtility.systemCopyBuffer = state.nameHash.ToString();
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                            }

                            EditorGUI.indentLevel -= 2;
                        }

                        EditorGUILayout.Space();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
        }
    }
}
