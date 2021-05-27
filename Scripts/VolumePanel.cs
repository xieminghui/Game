using UnityEngine;
using UnityEngine.UI;
using System;

namespace MediaPlayer
{
    public class VolumePanel : MonoBehaviour
    {
        public Slider VolumeSlider;
        public Text VolumePercentText;
        public TransitionButton VolumeAddBtn;
        public TransitionButton VolumeDecreaseBtn;

        Vector3 HoverPointStartPos;
        Vector3 HoverPointShowVector;
        bool IsEnter;
        bool IsVoluntary; // 是否物理按键主动
        bool IsMute;//是否静音
        bool IsChangedVolume; // 是否已经修改过一次
        bool IsShow;

        int MaxVolume;
        float VolumeStep;
        float OldSliderValue;

        RectTransform rectTrans; //slider rect
        Vector3 localPos; //slider position

        public Action<float> ChangVolumePercentCallback;
        public Action<bool> PointerEnterUICallback;

        private float currentSliderValue=0f;
        public void Init ()
        {
            IsEnter = false;
            IsVoluntary = false;
            IsMute = false;
            IsChangedVolume = false;
            IsShow = false;

            MaxVolume = 1;
            VolumeStep = 1.0f / 15.0f;
            OldSliderValue = 0;

            rectTrans = VolumeSlider.GetComponent<RectTransform>();
            localPos = VolumeSlider.transform.localPosition - VolumeSlider.transform.right * rectTrans.rect.width / 2;

            VolumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        
            EventTriggerListener.Get(gameObject).OnPtEnter += OnPointerEnterPanel;
            EventTriggerListener.Get(gameObject).OnPtExit += OnPointerExitPanel;
            EventTriggerListener.Get(VolumeSlider.gameObject).OnPtEnter += OnPointerEnterPanel;
            EventTriggerListener.Get(VolumeSlider.gameObject).OnPtExit += OnPointerExitPanel;
            EventTriggerListener.Get(VolumeSlider.gameObject).OnPtUp += OnPointerUpPanel;
            
            VolumeAddBtn.onClick.AddListener(VolumeAdd);
            VolumeDecreaseBtn.onClick.AddListener(VolumeDecrease);
        }

        void OnPointerEnterPanel(GameObject go)
        {
            if (IsEnter)
                return;

            IsEnter = true;

            if (PointerEnterUICallback != null)
                PointerEnterUICallback(true);
        }

        void OnPointerExitPanel(GameObject go)
        {
            if (!IsEnter)
                return;

            IsEnter = false;

            if (PointerEnterUICallback != null)
                PointerEnterUICallback(false);
        }

        void VolumeAdd()
        {
            OldSliderValue = VolumeSlider.value;
            float t = OldSliderValue + VolumeStep;
            if (t > MaxVolume)
                t = MaxVolume;
            ChangeVolume(t);
        }

        void VolumeDecrease()
        {
            OldSliderValue = VolumeSlider.value;
            float t = OldSliderValue - VolumeStep;
            if (t < 0)
                t = 0;
            ChangeVolume(t);
        }

        void OnPointerUpPanel(GameObject go)
        {
            if (IsChangedVolume)
            {
                IsChangedVolume = false;
                return;
            }

            IsVoluntary = true;

            SetCurrentVolume(currentSliderValue);
        }

        void OnSliderValueChanged(float f)
        {
            currentSliderValue = f;
        }
    
        public void ChangeVolume(float f)
        {
            if (IsChangedVolume)
            {
                IsChangedVolume = false;
                return;
            }

            IsVoluntary = true;

            SetCurrentVolume(f);
        }

        public void ChangeVolumeByDevice(float f)
        {
            IsVoluntary = false;

            SetCurrentVolume(f);
        }
        
        void SetCurrentVolume(float volume)
        {
            if (volume < 0)
                volume = 0;
            else if (volume > MaxVolume)
                volume = MaxVolume;

            float value = volume / MaxVolume;
            IsChangedVolume = true;
            VolumeSlider.value = value; 
            int valuePercent = (int)(value * 100);
            VolumePercentText.text = valuePercent + "%";

            VolumeDecreaseBtn.interactable = value > 0;
            VolumeAddBtn.interactable = value < MaxVolume;
            
            if (value == 0)
                IsMute = true;
            else
                IsMute = false;

            IsChangedVolume = false;
            if (ChangVolumePercentCallback != null)
                ChangVolumePercentCallback(value);
        }
    
        public void Show()
        {
            if (IsShow)
                return;

            IsShow = true;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            IsShow = false;
            gameObject.SetActive(false);
        }
    }
}


