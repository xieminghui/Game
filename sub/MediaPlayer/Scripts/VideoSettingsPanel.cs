using UnityEngine;
using UnityEngine.UI;
using System;

namespace MediaPlayer
{
    public class VideoSettingsPanel : MonoBehaviour 
    {
        public ScreenAdjustPanel ScreenAdjustPanel;
        public StereoTypePanel StereoTypePanel;
        public SceneChangePanel SceneChangePanel;
        public VoiceSettingPanel VoiceSettingPanel;
        public DefinitionPanel DefinitionPanel;
        public SubtitleSettingPanel SubtitleSettingPanel;

        public ToggleGroup toggleGroup;
        public CommonToggle SubtitleSettingBtn;
        public CommonToggle VoiceSettingBtn;
        public CommonToggle ScreenAdjustBtn;
        public CommonToggle StereoTypeBtn;
        public CommonToggle SceneChangeBtn;
        public CommonToggle DefinitionBtn;

        bool IsEnter;
        bool IsShow;

        public Action<bool> PointerEnterUICallback;

        public void Init(IMediaPlayerUIData uiData)
        {
            ScreenAdjustPanel.Init();
            StereoTypePanel.Init(uiData);
            SceneChangePanel.Init();
            VoiceSettingPanel.Init();
            SubtitleSettingPanel.Init();
            DefinitionPanel.Init(uiData);

            IsEnter = false;
            IsShow = false;

            EventTriggerListener.Get(gameObject).OnPtEnter += OnPointerEnterUI;
            EventTriggerListener.Get(gameObject).OnPtExit += OnPointerExitUI;
            EventTriggerListener.Get(ScreenAdjustBtn.gameObject).OnPtEnter += OnPointerEnterUI;
            EventTriggerListener.Get(ScreenAdjustBtn.gameObject).OnPtExit += OnPointerExitUI;
            EventTriggerListener.Get(VoiceSettingBtn.gameObject).OnPtEnter += OnPointerEnterUI;
            EventTriggerListener.Get(VoiceSettingBtn.gameObject).OnPtExit += OnPointerExitUI;
            EventTriggerListener.Get(SubtitleSettingBtn.gameObject).OnPtEnter += OnPointerEnterUI;
            EventTriggerListener.Get(SubtitleSettingBtn.gameObject).OnPtExit += OnPointerExitUI;
            EventTriggerListener.Get(StereoTypeBtn.gameObject).OnPtEnter += OnPointerEnterUI;
            EventTriggerListener.Get(StereoTypeBtn.gameObject).OnPtExit += OnPointerExitUI;
            EventTriggerListener.Get(SceneChangeBtn.gameObject).OnPtEnter += OnPointerEnterUI;
            EventTriggerListener.Get(SceneChangeBtn.gameObject).OnPtExit += OnPointerExitUI;
            EventTriggerListener.Get(DefinitionBtn.gameObject).OnPtEnter += OnPointerEnterUI;
            EventTriggerListener.Get(DefinitionBtn.gameObject).OnPtExit += OnPointerExitUI;
            
            VoiceSettingBtn.onValueChanged.AddListener(ClickVoiceSettingToggle);
            SubtitleSettingBtn.onValueChanged.AddListener(ClickSubtitleSettingToggle);
            ScreenAdjustBtn.onValueChanged.AddListener(ClickScreenAdjustToggle);
            StereoTypeBtn.onValueChanged.AddListener(ClickStereoTypeToggle);
            SceneChangeBtn.onValueChanged.AddListener(ClickSceneChangeToggle);
            DefinitionBtn.onValueChanged.AddListener(ClickDefinitionToggle);
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

        public void ScreenAdjustBtnStatusControl(bool isInteractable)
        {
            ScreenAdjustBtn.interactable = isInteractable;
        }

        public void SceneChangeBtnStatusControl(bool isInteractable)
        {
            SceneChangeBtn.interactable = isInteractable;
        }
        
        public void DefinationBtnStatusControl(bool isInteractable)
        {
        }
        
        public void VoiceSettingBtnStatusControl(bool isInteractable)
        {
            VoiceSettingBtn.interactable = isInteractable;
        }

        #region ButtonEvents
        
        void ClickVoiceSettingToggle(bool isOn)
        {
            if (isOn)
            {
                VoiceSettingPanel.Show();
            }
            else
            {
                VoiceSettingPanel.Hide();
            }
        }
        
        void ClickSubtitleSettingToggle(bool isOn)
        {
            if (isOn)
            {
                SubtitleSettingPanel.Show();
            }
            else
            {
                SubtitleSettingPanel.Hide();
            }
        }
        
        void ClickScreenAdjustToggle(bool isOn)
        {
            if (isOn)
            {
                ScreenAdjustPanel.Show();
            }
            else
            {
                ScreenAdjustPanel.Hide();
            }
        }
        
        void ClickStereoTypeToggle(bool isOn)
        {
            if (isOn)
            {
                StereoTypePanel.Show();
            }
            else
            {
                StereoTypePanel.Hide();
            }
        }
        
        void ClickSceneChangeToggle(bool isOn)
        {
            if (isOn)
            {
                SceneChangePanel.Show();
            }
            else
            {
                SceneChangePanel.Hide();
            }
        }

        void ClickDefinitionToggle(bool isOn)
        {
            if (isOn)
            {
                DefinitionPanel.Show();
            }
            else
            {
                DefinitionPanel.Hide();
            }
        }

        #endregion

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
            ResetUI();
            gameObject.SetActive(false);
        }

        public void ResetUI()
        {
            toggleGroup.SetAllTogglesOff();
        }
    }
}


