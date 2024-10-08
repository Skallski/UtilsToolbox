﻿using UnityEngine;

namespace UtilsToolbox.Utils.MultiSwitch
{
    public class MultiSwitchSprite : MultiSwitchWithParams<SpriteRenderer, Sprite>
    {
        protected override void SetStateInternalAction(SpriteRenderer spriteRenderer, Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}