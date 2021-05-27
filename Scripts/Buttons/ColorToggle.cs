using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class ColorToggle : Toggle
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private Image icon;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            switch (state)
            {
                case SelectionState.Normal:
                    if (background != null)
                    {
                        background.color = colors.normalColor;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.normalColor;
                    }
                    break;
                case SelectionState.Highlighted:
                    if (background != null)
                    {
                        background.color = colors.highlightedColor;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.highlightedColor;
                    }
                    break;
                case SelectionState.Pressed:
                    if (background != null)
                    {
                        background.color = colors.pressedColor;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.pressedColor;
                    }
                    break;
                case SelectionState.Disabled:
                    if (background != null)
                    {
                        icon.color = colors.disabledColor;
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

