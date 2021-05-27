using UnityEngine;
using UnityEngine.UI;
using System;



namespace MediaPlayer
{
    public enum ScreenScaleType
    {
        S_Nor,
        S_16_9,
        S_4_3,
        S_2_1,
        S_3_2,
        S_2351,
    }
    
    public class ScreenAdjustPanel : MonoBehaviour {

        public Slider ScreenSizeSlider;
        public Text ScreenSizeSliderTitleText;
        public Text ScreenSizeSliderText;
        public Image ScreenSizeSliderHandleArea;

        public CommonToggle Scale_normal;
        public CommonToggle Scale_169;
        public CommonToggle Scale_43;
        public CommonToggle Scale_21;
        public CommonToggle Scale_32;
        public CommonToggle Scale_2351;
        
        bool IsShow;

        public Action<bool> PointerEnterUICallback;
        public Action ChangeScreenAdjustSizeTypeCallback;
        public Action ChangeScreenAdjustScaleTypeCallback;

        public void Init()
        {
            IsShow = false;

            ScreenSizeSlider.onValueChanged.AddListener(ScreenSizeSliderValueChange);
            
            Scale_169.onValueChanged.AddListener(Select169ScaleBtn);
            Scale_43.onValueChanged.AddListener(Select43ScaleBtn);
            Scale_21.onValueChanged.AddListener(Select21ScaleBtn);
            Scale_32.onValueChanged.AddListener(Select32ScaleBtn);
            Scale_2351.onValueChanged.AddListener(Select2351ScaleBtn);
            Scale_normal.onValueChanged.AddListener(SelectNormalScaleBtn);

            EventTriggerListener.Get(gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(Scale_169.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(Scale_169.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(Scale_43.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(Scale_43.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(Scale_21.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(Scale_21.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(Scale_32.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(Scale_32.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(Scale_2351.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(Scale_2351.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(Scale_normal.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(Scale_normal.gameObject).OnPtExit = OnPointerExitUI;
        }

        #region ToggleEvent

        void Select169ScaleBtn(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (MediaPlayerGloabVariable.GetScreenScaleType() != ScreenScaleType.S_16_9)
            {
                MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_16_9);
                if (ChangeScreenAdjustScaleTypeCallback != null)
                {
                    ChangeScreenAdjustScaleTypeCallback();
                }
            }
        }
        
        void Select43ScaleBtn(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (MediaPlayerGloabVariable.GetScreenScaleType() != ScreenScaleType.S_4_3)
            {
                MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_4_3);
                if (ChangeScreenAdjustScaleTypeCallback != null)
                {
                    ChangeScreenAdjustScaleTypeCallback();
                }
            }
        }
        
        void Select21ScaleBtn(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (MediaPlayerGloabVariable.GetScreenScaleType() != ScreenScaleType.S_2_1)
            {
                MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_2_1);
                if (ChangeScreenAdjustScaleTypeCallback != null)
                {
                    ChangeScreenAdjustScaleTypeCallback();
                }
            }
        }
        
        void Select32ScaleBtn(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (MediaPlayerGloabVariable.GetScreenScaleType() != ScreenScaleType.S_3_2)
            {
                MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_3_2);
                if (ChangeScreenAdjustScaleTypeCallback != null)
                {
                    ChangeScreenAdjustScaleTypeCallback();
                }
            }
        }
        
        void Select2351ScaleBtn(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (MediaPlayerGloabVariable.GetScreenScaleType() != ScreenScaleType.S_2351)
            {
                MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_2351);
                if (ChangeScreenAdjustScaleTypeCallback != null)
                {
                    ChangeScreenAdjustScaleTypeCallback();
                }
            }
        }
        
        void SelectNormalScaleBtn(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (MediaPlayerGloabVariable.GetScreenScaleType() != ScreenScaleType.S_Nor)
            {
                MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_Nor);
                if (ChangeScreenAdjustScaleTypeCallback != null)
                {
                    ChangeScreenAdjustScaleTypeCallback();
                }
            }
        }
        
        #endregion

        void ScreenSizeSliderValueChange(float value)
        {
            ScreenSizeSliderText.text = (int)(value - 20f) + "%";
            MediaPlayerGloabVariable.SetScreenSizeRate(value);
            if (ChangeScreenAdjustSizeTypeCallback != null)
            {
                ChangeScreenAdjustSizeTypeCallback();
            }
        }

        void ScreenSizeValueChanged(int value)
        {
            ScreenSizeSliderText.text = (value - 20) + "%";
            MediaPlayerGloabVariable.SetScreenSizeRate(value);
            ScreenSizeSlider.value = value;
            if (ChangeScreenAdjustSizeTypeCallback != null)
            {
                ChangeScreenAdjustSizeTypeCallback();
            }
        }

        void OnPointerEnterUI(GameObject go)
        {
            if (PointerEnterUICallback != null)
                PointerEnterUICallback(true);
        }

        void OnPointerExitUI(GameObject go)
        {
            if (PointerEnterUICallback != null)
                PointerEnterUICallback(false);
        }

        public void Show()
        {
            if (IsShow)
                return;
            IsShow = true;
            
            ScreenScaleType scaleType = MediaPlayerGloabVariable.GetScreenScaleType();
            switch (scaleType)
            {
                case ScreenScaleType.S_Nor:
                    Scale_normal.isOn = true;
                    break;
                case ScreenScaleType.S_16_9:
                    Scale_169.isOn = true;
                    break;
                case ScreenScaleType.S_4_3:
                    Scale_43.isOn = true;
                    break;
                case ScreenScaleType.S_2_1:
                    Scale_21.isOn = true;
                    break;
                case ScreenScaleType.S_3_2:
                    Scale_32.isOn = true;
                    break;
                case ScreenScaleType.S_2351:
                    Scale_2351.isOn = true;
                    break;
            }
            gameObject.SetActive(true);
            ScreenSizeSlider.interactable = true;
            if (MediaPlayerGloabVariable.GetSceneModel() == SceneModel.IMAXTheater)
            {
                ScreenSizeSlider.minValue = MediaPlayerGloabVariable.IMAXTheaterScreenSizeSliderMin;
                ScreenSizeSlider.maxValue = MediaPlayerGloabVariable.IMAXTheaterScreenSizeSliderMax;
                ScreenSizeValueChanged((int)MediaPlayerGloabVariable.IMAXTheaterScreenSizeSliderMax);
                ScreenSizeSlider.interactable = false;
            }
            else
            if (MediaPlayerGloabVariable.GetSceneModel() == SceneModel.StarringNight
                || MediaPlayerGloabVariable.GetSceneModel() == SceneModel.Default)
            {
                ScreenSizeSlider.minValue = MediaPlayerGloabVariable.NormalScreenSizeSliderMin;
                ScreenSizeSlider.maxValue = MediaPlayerGloabVariable.NormalScreenSizeSliderMax;
                float sizeValue = MediaPlayerGloabVariable.GetScreenSizeRate();
                ScreenSizeValueChanged((int)sizeValue);
            }
        }

        public void UpdateScreenAdjustPanelUI()
        {
            float screen_size_value = MediaPlayerGloabVariable.GetScreenSizeRate();
            if (MediaPlayerGloabVariable.GetSceneModel() == SceneModel.Default || MediaPlayerGloabVariable.GetSceneModel() == SceneModel.StarringNight)
            {
                ScreenSizeValueChanged((int)screen_size_value);
            }
            else
            if (MediaPlayerGloabVariable.GetSceneModel() == SceneModel.IMAXTheater)
            {
                ScreenSizeValueChanged((int)screen_size_value);
            }
        
        }

        public void Hide()
        {
            if (!IsShow)
                return;
            IsShow = false;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Scale_169 = null;
            Scale_43 = null;
            Scale_21 = null;
            Scale_32 = null;
            Scale_2351 = null;
            Scale_normal = null;
            PointerEnterUICallback = null;
            ChangeScreenAdjustSizeTypeCallback = null;
            ChangeScreenAdjustScaleTypeCallback = null;
        }
    }

}
