using System;
using UnityEngine;

namespace UtilsToolbox.Utils.UI.UiPanel
{
    /// <summary>
    /// Base panel class with opening and closing animations handling
    /// </summary>
    public abstract class UiPanelAnimated : UiPanel
    {
        [SerializeField] private bool _animatedOpen = true;
        [SerializeField] private bool _animatedClose = true;

        #region BEFORE ANIMATION
        /// <summary>
        /// Called before opening animation is being triggered
        /// </summary>
        protected virtual void OnOpenedBeforeAnimationTriggered() {}
        
        /// <summary>
        /// Called before closing animation is being triggered
        /// </summary>
        protected virtual void OnClosedBeforeAnimationTriggered() {}
        #endregion

        #region AFTER ANIMATION
        /// <summary>
        /// Called after opening animation is finished
        /// </summary>
        protected virtual void OnOpenedAfterAnimationFinished() {}
        
        /// <summary>
        /// Called after closing animation is finished
        /// </summary>
        protected virtual void OnClosedAfterAnimationFinished() {}
        #endregion

        /// <summary>
        /// Handles animated opening (e.g. plays opening animation then executes panel logic)
        /// <remarks>
        /// Make sure to invoke callback when the animation is finished!
        /// </remarks>
        /// </summary>
        protected abstract void HandleAnimatedOpening(Action onOpeningFinished);
        
        /// <summary>
        /// Handles animated closing (e.g. plays closing animation then executes panel logic)
        /// <remarks>
        /// Make sure to invoke callback when the animation is finished!
        /// </remarks>
        /// </summary>
        protected abstract void HandleAnimatedClosing(Action onClosingFinished);

        protected sealed override void OpenSelf()
        {
            if (_animatedOpen)
            {
                ForceOpen();
                OnOpenedBeforeAnimationTriggered();

                HandleAnimatedOpening(OnOpened);
            }
            else
            {
                base.OpenSelf();
            }
        }

        protected sealed override void CloseSelf()
        {
            if (_animatedClose)
            {
                OnClosedBeforeAnimationTriggered();
                HandleAnimatedClosing(() =>
                {
                    OnClosed();
                    ForceClose();
                });
            }
            else
            {
                base.CloseSelf();
            }
        }

        protected sealed override void OnOpened()
        {
            OnOpenedAfterAnimationFinished();
            base.OnOpened();
        }

        protected sealed override void OnClosed()
        {
            OnClosedAfterAnimationFinished();
            base.OnClosed();
        }
    }
}