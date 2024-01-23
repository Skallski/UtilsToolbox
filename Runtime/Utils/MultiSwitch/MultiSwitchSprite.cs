using SkalluUtils.Extensions;
using SkalluUtils.Extensions.Collections;
using UnityEngine;

namespace SkalluUtils.Utils.MultiSwitch
{
    public class MultiSwitchSprite : BasicMultiSwitch
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [field: SerializeField] public Sprite[] Sprites { get; private set; }

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

            if (newValue < 0 || Sprites.Length <= newValue)
            {
                return;
            }

            _spriteRenderer.sprite = Sprites[newValue];
        }
    }
}