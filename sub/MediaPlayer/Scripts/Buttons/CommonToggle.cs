using System;
using IVR.Language;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MediaPlayer
{
    [Serializable]
    public class CommonToggleEvent : UnityEvent<int, bool>
    {
    }
    
	public class CommonToggle : Toggle
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private Image icon;
        [SerializeField]
        public Text label;

        [SerializeField]
        private bool hideLabelNormalState;

        [SerializeField]
        private bool useTextFluidEffect;
        
        [SerializeField]
        private TextFluidEffect textFluidEffect;

        [SerializeField] 
        private string languageKey = "";
        
        private bool pointerEnter = false;

        public int id;

        public CommonToggleEvent onValueChangedWithID = new CommonToggleEvent();

        public CommonToggle()
		{
			onValueChanged.AddListener(stateChanged);
        }

        protected override void Start()
        {
            base.Start();
            if (useTextFluidEffect)
            {
                textFluidEffect.strip = true;
                
                UpdateLanguage();
                LanguageManager.Instance.OnLanguageChanged.AddListener(UpdateLanguage);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (useTextFluidEffect && textFluidEffect != null)
            {
                if(textFluidEffect.CanRoll)
                {
                    textFluidEffect.CanRoll = false;
                }

                if (!textFluidEffect.strip)
                {
                    textFluidEffect.strip = true;
                }
            }
        }

        protected override void OnDestroy()
        {
            if (useTextFluidEffect)
            {
                LanguageManager.Instance.OnLanguageChanged.RemoveListener(UpdateLanguage);
            }
            base.OnDestroy();
        }

        private void UpdateLanguage()
        {
            textFluidEffect.UpdateText(Language.Get(languageKey));
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            pointerEnter = true;
            
            if (useTextFluidEffect && textFluidEffect != null)
            {
                if(!textFluidEffect.CanRoll)
                {
                    textFluidEffect.CanRoll = true;
                }

                if (textFluidEffect.strip)
                {
                    textFluidEffect.strip = false;
                }
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            pointerEnter = false;

            if (useTextFluidEffect && textFluidEffect != null)
            {
                if(textFluidEffect.CanRoll)
                {
                    textFluidEffect.CanRoll = false;
                }

                if (!textFluidEffect.strip)
                {
                    textFluidEffect.strip = true;
                }
            }
        }

        private void stateChanged(bool _isOn)
        {
            DoStateTransition(currentSelectionState, false);
            if (onValueChangedWithID != null)
            {
                onValueChangedWithID.Invoke(id, _isOn);
            }
        }
		
		protected override void DoStateTransition(SelectionState state, bool instant)
        {
            switch (state)
            {
                case SelectionState.Normal:
                    if (isOn)
                    {
                        if (background != null)
                        {
                            background.color = Color.clear;
                        }
                        if (icon != null)
                        {
                            icon.color = colors.pressedColor;
                        }
                        if (label != null)
                        {
                            label.color = colors.pressedColor;
                        }
                    }
                    else
                    {
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
                            if (hideLabelNormalState)
                            {
                                label.color = Color.clear;
                            }
                            else
                            {
                                label.color = colors.normalColor;
                            }
                        }
                    }
                    break;
                case SelectionState.Highlighted:
                    if (isOn)
                    {
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
                    }
                    else
                    {
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
                        if (hideLabelNormalState)
                        {
                            label.color = Color.clear;
                        }
                        else
                        {
                            label.color = colors.disabledColor;
                        }
                    }
                    break;
            }
        }
	}
}

