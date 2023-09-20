﻿using SkalluUtils.Extensions;
using SkalluUtils.Extensions.Collections;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchSprite : BasicMultiSwitch
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite[] _sprites;

        protected SpriteRenderer SpriteRenderer
        {
            get => _spriteRenderer;
            set => _spriteRenderer = value;
        }

        protected override void SetStateInternal(int oldValue, int newValue)
        {
            if (oldValue == newValue || _spriteRenderer == null)
            {
                return;
            }

            if (newValue < 0 || _sprites.Length <= newValue)
            {
                return;
            }

            _spriteRenderer.sprite = _sprites[newValue];
        }

        public int GetRandomState() => _sprites.IndexOf(_sprites.RandomItem());
    }
}