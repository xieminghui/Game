using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using IVR.Language;

namespace MediaPlayer
{
    public enum LoopTypeEnum
    {
        PlayOne,//单个播放，播放完毕退出播放列表
        PlayOneLoop,//当个循环播放
        PlayList,//列表播放，播放完毕退出播放列表
        PlayListLoop,//列表循环
    }

    public enum VideoControlButtonType
    {
        
    }

    public class VideoControlPanel : MonoBehaviour
    {
        public IMediaPlayerUIData uiData;
        
        public TransitionButton LoopBtn;
        public BackButton BackBtn;
        public TransitionButton PlayBtn;
        public TransitionButton RePlayBtn;
        public TransitionButton PauseBtn;
        public TransitionButton PreviousBtn;
        public TransitionButton NextBtn;
        public CommonToggle VolumeBtn;
        public CommonToggle SettingBtn;
        public TransitionButton FocusBtn;
        public TransitionButton LockBtn;
        public CommonToggle DramBtn;
        public CommonToggle SeatBtn;
        public StripIconNameLengthWithSuffix VideoNameText;

        public PlayProgressBarPanel PlayPBPanel;
        public VolumePanel VolumePanel;
        public VideoDramaPanel VideoDramaPanel;
        public VideoSettingsPanel SettingsPanel;
        public AnimationCurve AnimCurve;

        public ToggleGroup toggleGroup;
    
        [SerializeField]
        private List<Sprite> loopBtnIconList = new List<Sprite>();
        [SerializeField]
        private TextFluidEffect loopBtnLabel;
        [SerializeField]
        private Image loopBtnImage;
    
        Vector3 ZoomOut;//缩小值

        bool IsLive = false;
        /// <summary>
        /// 是否hover了视角调节按钮
        /// </summary>
        bool IsEnterFocus = false;
        bool IsShow;
        /// <summary>
        /// 锁定视角按钮是否可用
        /// </summary>
        bool IsEnableLockBtn = false;

        public Action ClickBackBtnCallback;
        public Action ClickLoopBtnCallback;
        public Action ClickPreviousBtnCallback;
        public Action ClickPlayBtnCallback;
        public Action ClickRePlayBtnCallBack;
        public Action ClickPauseBtnCallback;
        public Action ClickNextBtnCallback;
        public Action ClickFocusBtnCallback;
        public Action ClickLockBtnCallback;
        
        public Action<bool> PointerEnterUICallback;
        
        public Action<float> VolumeValueChangedByUICallback;
        
        public Action<bool> UseOrNotStereoCallback;
        
        public Action<bool,bool> OnUIShow;

        public void SetMediaPlayerUIData(IMediaPlayerUIData _uiData)
        {
            uiData = _uiData;
        }

        public void Init()
        {
            SettingsPanel.Init(uiData);
            VolumePanel.Init();
            PlayPBPanel.Init();
            VideoDramaPanel.Init(uiData);
            
            ZoomOut = Vector3.one * 0.8f;
            IsLive = false;

            BackBtn.onClick.AddListener(ClickBackBtn);
            LoopBtn.onClick.AddListener(ClickLoopBtn);
        
            PreviousBtn.onClick.AddListener(ClickPreviousBtn);
        
            PlayBtn.onClick.AddListener(ClickPlayBtn);
            
            RePlayBtn.onClick.AddListener(ClickRePlayBtn);
        
            PauseBtn.onClick.AddListener(ClickPauseBtn);
        
            NextBtn.onClick.AddListener(ClickNextBtn);
        
            FocusBtn.onClick.AddListener(ClickFocusBtn);
        
            LockBtn.onClick.AddListener(ClickLockBtn);
            
            DramBtn.onValueChanged.AddListener(DramToggleChanged);
            
            SeatBtn.onValueChanged.AddListener(SeatToggleChanged);
        
            VolumeBtn.onValueChanged.AddListener(VolumeToggleChanged);
        
            SettingBtn.onValueChanged.AddListener(SettingToggleChanged);
        
        
            EventTriggerListener.Get(LoopBtn.gameObject).OnPtEnter = OnPointerEnterLoopBtn;
            EventTriggerListener.Get(LoopBtn.gameObject).OnPtExit = OnPointerExitLoopBtn;
            EventTriggerListener.Get(PreviousBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(PreviousBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(BackBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(BackBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(PlayBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(PlayBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(RePlayBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(RePlayBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(PauseBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(PauseBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(NextBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(NextBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(VolumeBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(VolumeBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(VolumePanel.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(VolumePanel.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(PlayPBPanel.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(PlayPBPanel.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(SettingBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(SettingBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(DramBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(DramBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(SeatBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(SeatBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(FocusBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(FocusBtn.gameObject).OnPtExit = OnPointerExit;
            EventTriggerListener.Get(LockBtn.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(LockBtn.gameObject).OnPtExit = OnPointerExit;

            CheckLoopBtn();
            SetLockBtnInteractable(IsEnableLockBtn);

            LanguageManager.Instance.OnLanguageChanged.AddListener(CheckLoopBtn); 
        }

        public void PlayBtnControl(bool isOnline)
        {
            IsLive = isOnline;
            if (isOnline) //直播
            {
                PlayBtn.interactable = false;
                PauseBtn.interactable = false;
                PlayPBPanel.CurrentTimeText.gameObject.SetActive(false);
                PlayPBPanel.TotalTimeText.gameObject.SetActive(false);
                PlayPBPanel.PlayPBSliderBg.raycastTarget = false;
            }
            else //点播或本地
            {
                PlayBtn.interactable = true;
                PauseBtn.interactable = true;
                PlayPBPanel.CurrentTimeText.gameObject.SetActive(true);
                PlayPBPanel.TotalTimeText.gameObject.SetActive(true);
                PlayPBPanel.PlayPBSliderBg.raycastTarget = true;
            }
        }

        public void DisableAllBtnExceptBack()
        {
            VolumePanel.Hide();
            SettingsPanel.Hide();
            VolumeBtn.isOn = false;
            SettingBtn.isOn = false;
            PlayBtnControl(true);
            SetLockBtnInteractable(false);
            SetNextBtnInteractable(false);
            SetPreviousBtnInteractable(false);
            SetFocusBtnInteractable(false);
            SetSettingBtnInteractable(false);
            SetVolumeBtnInteractable(false);
            SetLoopBtnInteractable(false);
        }

        public void ResetAllBtnExceptBack()
        {
            VolumePanel.Hide();
            SettingsPanel.Hide();
            VolumeBtn.isOn = false;
            SettingBtn.isOn = false;
            PlayBtnControl(false);
            SetLockBtnInteractable(true);
            SetNextBtnInteractable(true);
            SetPreviousBtnInteractable(true);
            SetFocusBtnInteractable(true);
            SetSettingBtnInteractable(true);
            SetVolumeBtnInteractable(true);
            SetLoopBtnInteractable(true);
        }

        public void SetPlayMode(bool isPlay)
        {
            if(isPlay)
            {
                PlayBtn.gameObject.SetActive(false);
                PauseBtn.gameObject.SetActive(true);
            }
            else
            {
                PlayBtn.gameObject.SetActive(true);
                PauseBtn.gameObject.SetActive(false);
            }
        }

        public void SetVideoName(string name)
        {
            VideoNameText.Init(name);
        }
        
        #region ButtonEvents
    
        void ClickBackBtn()
        {
            if (ClickBackBtnCallback != null)
                ClickBackBtnCallback();
        }
    
        void ClickLoopBtn()
        {
            toggleGroup.SetAllTogglesOff();
            if (ClickLoopBtnCallback != null)
            {
                ClickLoopBtnCallback.Invoke();
            }
            CheckLoopBtn();
        }
    
        void ClickPreviousBtn()
        {
            toggleGroup.SetAllTogglesOff();
        
            if (ClickPreviousBtnCallback != null)
                ClickPreviousBtnCallback();
        }

        void ClickPlayBtn()
        {
            toggleGroup.SetAllTogglesOff();
            PlayBtn.gameObject.SetActive(false);
            PauseBtn.gameObject.SetActive(true);
            if (ClickPlayBtnCallback != null)
                ClickPlayBtnCallback();
        }

        void ClickRePlayBtn()
        {
            toggleGroup.SetAllTogglesOff();
            PauseBtn.gameObject.SetActive(true);
            if (ClickRePlayBtnCallBack != null)
                ClickRePlayBtnCallBack();
        }

        void ClickPauseBtn()
        {
            toggleGroup.SetAllTogglesOff();
            PlayBtn.gameObject.SetActive(true);
            PauseBtn.gameObject.SetActive(false);
            if (ClickPauseBtnCallback != null)
                ClickPauseBtnCallback();
        }
    
        void ClickNextBtn()
        {
            toggleGroup.SetAllTogglesOff();
        
            if (ClickNextBtnCallback != null)
                ClickNextBtnCallback();
        }
    
        void ClickFocusBtn()
        {
            toggleGroup.SetAllTogglesOff();

            if (ClickFocusBtnCallback != null)
            {
                ClickFocusBtnCallback.Invoke();
            }
        }
    
        void ClickLockBtn()
        {
            toggleGroup.SetAllTogglesOff();

            if (ClickLockBtnCallback != null)
                ClickLockBtnCallback();
        }

        void DramToggleChanged(bool on)
        {
            if (on)
            {
                VideoDramaPanel.Show();
            }
            else
            {
                VideoDramaPanel.Hide();
            }
        }

        void SeatToggleChanged(bool on)
        {
            
        }

        void VolumeToggleChanged(bool on)
        {
            if (on)
            {
                VolumePanel.transform.localPosition = new Vector3(VolumeBtn.transform.localPosition.x, VolumePanel.transform.localPosition.y, VolumePanel.transform.localPosition.z);
                VolumePanel.Show();
            }
            else
            {
                VolumePanel.Hide();
            }
        }

        void SettingToggleChanged(bool on)
        {
            if (on)
            {
                SettingsPanel.transform.parent.position = new Vector3(SettingBtn.transform.position.x, SettingsPanel.transform.parent.position.y, SettingsPanel.transform.parent.position.z);
                SettingsPanel.transform.parent.localPosition = new Vector3(SettingsPanel.transform.parent.localPosition.x + 6, SettingsPanel.transform.parent.localPosition.y, SettingsPanel.transform.parent.localPosition.z);
                SettingsPanel.Show();
            }
            else
            {
                SettingsPanel.Hide();
            }
        }
        
        #endregion
        
        private void CheckLoopBtn()
        {
            switch (uiData.GetLoopType())
            {
                case LoopTypeEnum.PlayOne:
                    loopBtnLabel.UpdateText(Language.Get("MediaPlayerPanel.VideoControlPanel.LoopBtn.PlayOne.Text"));
                    loopBtnImage.sprite = loopBtnIconList[0];
                    break;
                case LoopTypeEnum.PlayOneLoop:
                    loopBtnLabel.UpdateText(Language.Get("MediaPlayerPanel.VideoControlPanel.LoopBtn.PlayOneLoop.Text"));
                    loopBtnImage.sprite = loopBtnIconList[1];
                    break;
                case LoopTypeEnum.PlayList:
                    loopBtnLabel.UpdateText(Language.Get("MediaPlayerPanel.VideoControlPanel.LoopBtn.PlayList.Text"));
                    loopBtnImage.sprite = loopBtnIconList[2];
                    break;
                case LoopTypeEnum.PlayListLoop:
                    loopBtnLabel.UpdateText(Language.Get("MediaPlayerPanel.VideoControlPanel.LoopBtn.PlayListLoop.Text"));
                    loopBtnImage.sprite = loopBtnIconList[3];
                    break;
            }
        }

        private void CheckPlayOrPaseBtnStatus()
        {
            if (IsLive) return;

            VolumePanel.Hide();
            SettingsPanel.Hide();
            VolumeBtn.isOn = false;
            SettingBtn.isOn = false;
            PlayBtn.gameObject.SetActive(false);
            PauseBtn.gameObject.SetActive(true);
        }

        void VolumeValueChanged(float value, bool IsVoluntary)
        {

            if (IsVoluntary) //by UI
            {
                if (VolumeValueChangedByUICallback != null)
                    VolumeValueChangedByUICallback(value);
            }
        }
        
        void OnPointerEnterLoopBtn(GameObject go)
        {
            loopBtnLabel.CanRoll = true;
            OnPointerEnter(go);
        }
        
        void OnPointerExitLoopBtn(GameObject go)
        {
            loopBtnLabel.CanRoll = false;
            OnPointerExit(go);
        }
        
        void OnPointerEnter(GameObject go)
        {
            if (PointerEnterUICallback != null)
                PointerEnterUICallback(true);
        }

        void OnPointerExit(GameObject go)
        {
            if (PointerEnterUICallback != null)
                PointerEnterUICallback(false);
        }

        public void SetVideoTotalTime(long time)
        {
            PlayPBPanel.SetTotalTime(time);
        }
        
        public void SetVideoCurrentTime(long time)
        {
            PlayPBPanel.SetCurrentTime(time);
        }

        public void SetLockBtnInteractable(bool isEnable)
        {
            IsEnableLockBtn = isEnable;
            LockBtn.interactable = isEnable;
        }
        
        public void SetNextBtnInteractable(bool isEnable)
        {
            NextBtn.interactable = isEnable;
        }
        
        public void SetPreviousBtnInteractable(bool isEnable)
        {
            PreviousBtn.interactable = isEnable;
        }
        
        public void SetVolumeBtnInteractable(bool isEnable)
        {
            VolumeBtn.interactable = isEnable;
        }

        public void SetSettingBtnInteractable(bool isEnable)
        {
            SettingBtn.interactable = isEnable;
        }
        
        public void SetFocusBtnInteractable(bool isEnable)
        {
            FocusBtn.interactable = isEnable;
        }
        
        public void SetLoopBtnInteractable(bool isEnable)
        {
            LoopBtn.interactable = isEnable;
        }
        
        public void Show(bool isKeep=false)
        {
            PlayPBPanel.ResetLocalPos();
            
            if (UseOrNotStereoCallback != null)
                UseOrNotStereoCallback(false);
            
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            
            if (OnUIShow != null)
                OnUIShow(true, isKeep);
        }

        public void HideSettingPanel()
        {
            SettingBtn.isOn = false;
        }

        public void Hide()
        {
            toggleGroup.SetAllTogglesOff();
            transform.localScale = Vector3.zero;
        
            if (UseOrNotStereoCallback != null)
                UseOrNotStereoCallback(true);
        
            if (OnUIShow != null)
                OnUIShow(false,false);
        }

        public void ResetUI()
        {
            toggleGroup.SetAllTogglesOff();
            
            SetVideoCurrentTime(0);
            SetPlayMode(false);
        }
    }
}

