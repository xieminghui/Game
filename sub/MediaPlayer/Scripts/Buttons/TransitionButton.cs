using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class TransitionButton : Button
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private Text label;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            switch (state)
            {
                case SelectionState.Normal:
                    if (background != null)
                    {
                        background.color = Color.clear;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.normalColor;
                    }
                    if (label != null)
                    {
                        label.color = Color.clear;
                    }
                    break;
                case SelectionState.Highlighted:
                    if (background != null)
                    {
                        background.color = Color.white;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.highlightedColor;
                    }
                    if (label != null)
                    {
                        label.color = colors.highlightedColor;
                    }
                    break;
                case SelectionState.Pressed:
                    if (background != null)
                    {
                        background.color = Color.white;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.pressedColor;
                    }
                    if (label != null)
                    {
                        label.color = colors.pressedColor;
                    }
                    break;
                case SelectionState.Disabled:
                    if (background != null)
                    {
                        background.color = Color.clear;
                    }
                    if (icon != null)
                    {
                        icon.color = colors.disabledColor;
                    }
                    if (label != null)
                    {
                        label.color = Color.clear;
                    }
                    break;
            }
        }

    }
}
