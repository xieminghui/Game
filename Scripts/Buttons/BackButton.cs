using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class BackButton : Button
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private Sprite backgroundNormal;
        [SerializeField]
        private Sprite backgroundHighlighted;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            switch (state)
            {
                case SelectionState.Normal:
                    if (background != null)
                    {
                        background.sprite = backgroundNormal;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.normalColor;
                    }
                    break;
                case SelectionState.Highlighted:
                    if (background != null)
                    {
                        background.sprite = backgroundHighlighted;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.highlightedColor;
                    }
                    break;
                case SelectionState.Pressed:
                    if (background != null)
                    {
                        background.sprite = backgroundHighlighted;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.pressedColor;
                    }
                    break;
                case SelectionState.Disabled:
                    if (background != null)
                    {
                        background.sprite = backgroundNormal;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.disabledColor;
                    }
                    break;
            }
        }

    }
}
