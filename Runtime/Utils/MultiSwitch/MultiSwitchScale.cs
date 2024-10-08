﻿using UnityEngine;

namespace UtilsToolbox.Utils.MultiSwitch
{
    public class MultiSwitchScale : MultiSwitchWithParams<Transform, Vector3>
    {
        protected override void SetStateInternalAction(Transform tr, Vector3 scale)
        {
            tr.localScale = scale;
        }
    }
}