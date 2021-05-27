/*using UnityEngine;
using System;
using SVR.LocalPanel;

namespace MediaPlayer
{
    public delegate long Long_VoidDelegate();

    public class MediaPlayerPanel : VRInputMessageBase
    {
        public VideoControlPanel VideoCtrlPanel;
        public ImageControlPanel ImageCtrlPanel;
    
        public BendQuadScreenRaycastImage BendQuadScreenRaycastImage;
        public GameObject NoloLeftController;
        public GameObject NoloRightController;
        public GameObject GvrControllerPointer;
        public MeshRenderer SvrReticleRayLine;
        public GameObject LoadingObjectl;

        JVideoDescriptionInfo CurJVideoDscpInfo;
        JImageDescriptionInfo CurJImageDscpInfo;
    
        bool IsKeepShowUI;
        bool IsShowUIForAutoMode;//对于自动模式时，是否在显示
        bool OldIsMonoShow;
        bool UIIsShow; //UI是否显示

        bool TPAndVCPUIIsShow;
        bool IsEnterPlayer; //true时才能手势缩放播放器
        bool IsEnterUI; //true时不能切换图片
        bool IsShowLoading; //true时不能切换图片
        bool IsChangeSize; //true时不能切换图片
        float OldScreenSize;

        public Action<StereoType> ChangeStereoTypeCallback;
        public Action<bool> Change3DEnableCallBack;
        public Action<bool> PointerEnterUICallback;
        public Long_VoidDelegate GetVideoCurrentTimePtr;

        //Picture action for change previous/next picture
        public Action UIRecenterCallback;
        public Action ChangeToPreviousPictureCallback;
        public Action ChangeToNextPictureCallback;
        public Action<bool> StretchingPictureCallback;
        public Action ShowUICallback;
        private Vector2 CurrentScreenScale = Vector2.one;

        public static event Action<bool> OnEnableChangeEvent; 

        public void ResetVariable()
        {
            CurJVideoDscpInfo = null;
            CurJImageDscpInfo = null;
            IsKeepShowUI = false;
            IsShowUIForAutoMode = true;
            OldIsMonoShow = true;
            OldScreenSize = 0;
            IsEnterPlayer = false;
            IsShowLoading = false;
            TPAndVCPUIIsShow = true;
            IsChangeSize = false;
            UIIsShow = false;

            ShowControllerRayLine();
            StopAutoHideUI();
            VideoCtrlPanel.Hide();
            ImageCtrlPanel.Hide();
        }

        public void Init()
        {
            ResetVariable();
            VideoCtrlPanel.Init();
            ImageCtrlPanel.Init();

            VideoCtrlPanel.SettingsPanel.StereoTypePanel.ChangeStereoTypeCallback += ChangeStereoType;
            ImageCtrlPanel.SettingsPanel.StereoTypePanel.ChangeStereoTypeCallback += ChangeStereoType;
            VideoCtrlPanel.SettingsPanel.StereoTypePanel.Change3DEnableCallback += Change3DEnable;
            ImageCtrlPanel.SettingsPanel.StereoTypePanel.Change3DEnableCallback += Change3DEnable;
            VideoCtrlPanel.ClickBackBtnCallback += BackToLocalList;
            VideoCtrlPanel.VolumePanel.PointerEnterUICallback += PointerEnterUI;
            VideoCtrlPanel.SettingsPanel.PointerEnterUICallback += PointerEnterUI;
            ImageCtrlPanel.SettingsPanel.PointerEnterUICallback += PointerEnterUI;
            VideoCtrlPanel.SettingsPanel.VoiceSettingPanel.PointerEnterUICallback += PointerEnterUI;
            VideoCtrlPanel.SettingsPanel.StereoTypePanel.PointerEnterUICallback += PointerEnterUI;
            ImageCtrlPanel.SettingsPanel.StereoTypePanel.PointerEnterUICallback += PointerEnterUI;
            VideoCtrlPanel.SettingsPanel.SceneChangePanel.PointerEnterUICallback += PointerEnterUI;
            VideoCtrlPanel.SettingsPanel.SceneChangePanel.PointerEnterUICallback += PointerEnterUI;
            VideoCtrlPanel.PointerEnterUICallback += PointerEnterUI;
            ImageCtrlPanel.PointerEnterUICallback += PointerEnterUI;

            BendQuadScreenRaycastImage.PointerEnterUICallback += PointerEnterPlayer;
            CurrentScreenScale = GetNormalPlayerUIScale();
        }

        void ChangeAdjustScreenSize()
        {
            float value_rate = MediaPlayerGloabVariable.GetScreenSizeRate();
            Debug.Log("调试=>>尺寸变动前,长度为:" + CurrentScreenScale.x + " 高度为:" + CurrentScreenScale.y + "...当前变化率为:" + value_rate / 100f);
            Vector2 changedScreenScale = CurrentScreenScale * (value_rate / 100f);
            Debug.Log("调试=>>尺寸变动后,长度为:" + changedScreenScale.x + " 高度为:" + changedScreenScale.y);
            PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = new Vector3(changedScreenScale.x, changedScreenScale.y, 1);
        }

        void ChangeAdjustScreenScale()
        {
            ScreenScaleType scaleType = MediaPlayerGloabVariable.GetScreenScaleType();
            Vector2 creenScale = Vector2.one;
            switch (scaleType)
            {
                case ScreenScaleType.S_Nor:
                    creenScale = GetNormalPlayerUIScale();
                    break;
                case ScreenScaleType.S_16_9:
                    creenScale = new Vector2(16, 9);
                    break;
                case ScreenScaleType.S_4_3:
                    creenScale = new Vector2(4, 3);
                    break;
                case ScreenScaleType.S_3_2:
                    creenScale = new Vector2(3, 2);
                    break;
                case ScreenScaleType.S_2_1:
                    creenScale = new Vector2(2, 1);
                    break;
                case ScreenScaleType.S_2351:
                    creenScale = new Vector2(2.35f, 1f);
                    break;
            }
            Vector2 defaultScale = GetNormalPlayerUIScale();
            float defaultRadius = Mathf.Sqrt(Mathf.Pow(defaultScale.x, 2f) + Mathf.Pow(defaultScale.y, 2f));

            float scaleRadius = Mathf.Sqrt(Mathf.Pow(creenScale.x, 2f) + Mathf.Pow(creenScale.y, 2f));
            float scaleRate = defaultRadius / scaleRadius;

            Vector2 currentScreenScale = creenScale * scaleRate;

            LogTool.Log("调试=>>defaultScale=" + defaultScale.ToString());
            LogTool.Log("调试=>>调整比例后当前播放器的长为:" + currentScreenScale.x + " 高为:" + currentScreenScale.y + " currentScreenScale=" + currentScreenScale);
            CurrentScreenScale = new Vector3(currentScreenScale.x, currentScreenScale.y, 1);
            PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CurrentScreenScale;
            VideoCtrlPanel.SettingsPanel.ScreenAdjustPanel.UpdateScreenAdjustPanelUI();
        }

        void ChangeStereoType(StereoType stereoType)
        {
            if (MediaPlayerGloabVariable.CurPlayType == PlayType.Video)
            {
                if (CurJVideoDscpInfo == null)
                    return;

                CurJVideoDscpInfo.stereoType = (int)stereoType;
            }
            else if (MediaPlayerGloabVariable.CurPlayType == PlayType.Picture)
            {
                if (CurJImageDscpInfo == null)
                    return;

                CurJImageDscpInfo.stereoType = (int)stereoType;
            }

            if (ChangeStereoTypeCallback != null)
                ChangeStereoTypeCallback(stereoType);
        }

        void Change3DEnable(bool enable)
        {
            if (Change3DEnableCallBack != null)
            {
                Change3DEnableCallBack.Invoke(enable);
            }
        }

        void BackToLocalList()
        {
            PointerEnterUI(false);
            ShowControllerRayLine();
            GvrControllerPointer.transform.localScale = Vector3.one;
        }

        public void PointerEnterUI(bool isEnter)
        {
            IsEnterUI = isEnter;

            if (PointerEnterUICallback != null)
                PointerEnterUICallback(isEnter);
        }

        void PointerEnterPlayer(bool isEnter)
        {
            IsEnterPlayer = isEnter;
            if (!TPAndVCPUIIsShow)
                return;

            if (PointerEnterUICallback != null)
                PointerEnterUICallback(isEnter);
        }
        
        private void OnEnable()
        {
            if (OnEnableChangeEvent != null) OnEnableChangeEvent(true);
        }
        private void OnDisable()
        {
            if (OnEnableChangeEvent != null) OnEnableChangeEvent(false);
        }
        private void OnDestroy()
        {

        }
        public void Show()
        {
            gameObject.SetActive(true);
            if (MediaPlayerGloabVariable.CurPlayType == PlayType.Video)
            {
                UIIsShow = true;
                VideoCtrlPanel.Show();
                ImageCtrlPanel.Hide();
            }
            else if (MediaPlayerGloabVariable.CurPlayType == PlayType.Picture)
            {
                UIIsShow = false;
                ImageCtrlPanel.Show();
                VideoCtrlPanel.Hide();
            }

#if UNITY_ANDROID
            AutoHideUI();
#endif
        }

        public void Hide()
        {
            ResetUI();
            RestUIControl();
            gameObject.SetActive(false);
        }

        public void SetVideoPlayerState(JVideoDescriptionInfo jVdi)
        {
            CurJVideoDscpInfo = jVdi;

            VideoCtrlPanel.SetVideoName(jVdi.name);
            VideoCtrlPanel.SetVideoCurrentTime(0);
            VideoCtrlPanel.SetPlayMode(true);
        }

        public void SetImagePlayerState(JImageDescriptionInfo jImg)
        {
            CurJImageDscpInfo = jImg;

            PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().Normal_QuadScreenStandardScale;

            VideoCtrlPanel.SetVideoName(jImg.name);
        }

        public void PlayVideo(bool isManual = false)
        {
            if (!isManual)
                VideoCtrlPanel.SetPlayMode(true);//播放按钮变成播放模式
        }

        void ResetUI()
        {
            if (MediaPlayerGloabVariable.CurPlayType == PlayType.Video)
                VideoCtrlPanel.ResetUI();
            else if (MediaPlayerGloabVariable.CurPlayType == PlayType.Picture)
                ImageCtrlPanel.SettingsPanel.Hide();
        }

        public void KeepShowUI()
        {
            IsKeepShowUI = true;
            IsShowUIForAutoMode = true;
            StopAutoHideUI();

            if (MediaPlayerGloabVariable.CurPlayType == PlayType.Video)
            {
                UIIsShow = true;
                VideoCtrlPanel.Show(true);
            }
            else if (MediaPlayerGloabVariable.CurPlayType == PlayType.Picture)
                ImageCtrlPanel.Show();

            TPAndVCPUIIsShow = true;
            ShowControllerRayLine();
        }

        public void ResetControlValue()
        {
            IsKeepShowUI = false;

            if (!PlayerGameobjectControl.Instance.QuadScreen.gameObject.activeInHierarchy)
                IsEnterPlayer = false;

            Cinema.IsPointerEnterVideoPlayerUI = IsEnterUI;
        }
    
        public void ChangePlayerUI()
        {
            SceneModel curSceneModel = MediaPlayerGloabVariable.GetSceneModel();
            if (curSceneModel == SceneModel.Default || curSceneModel == SceneModel.StarringNight)
            {
                PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().ImaxQuadScreenScale*MediaPlayerGloabVariable.NormalScreenSizeRate/100f;
                PlayerGameobjectControl.Instance.QuadScreen.transform.localPosition = CinemaSettings.GetInstance().ImaxQuadScreenPosition;
            }
            else if (curSceneModel == SceneModel.IMAXTheater)
            {
                PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().ImaxQuadScreenScale * MediaPlayerGloabVariable.IMAXTheaterScreenSizeRate / 100f;
                PlayerGameobjectControl.Instance.QuadScreen.transform.localPosition = CinemaSettings.GetInstance().ImaxQuadScreenPosition;
                CinemaMaterialSetting.GetInstance().ImaxPurple.transform.localRotation = Quaternion.Euler(PlayerGameobjectControl.Instance.QuadScreen.transform.eulerAngles + CinemaSettings.GetInstance().ImaxQuadScreenEulerDelta);
            }
            else if (curSceneModel == SceneModel.Drive)
            {
                CinemaSettings.GetInstance().DriveModelQuadScreenTrans();
            }
            else if (curSceneModel == SceneModel.HomeTheater)
            {
                PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().HomeTheaterScreenScale;
                PlayerGameobjectControl.Instance.QuadScreen.transform.localPosition = CinemaSettings.GetInstance().HomeTheaterScreenPosition;
            }
        }
	
        /// <summary>
        /// 获取默认播放器大小
        /// </summary>
        public Vector2 GetNormalPlayerUIScale()
        {
            SceneModel sceneModel = MediaPlayerGloabVariable.GetSceneModel();
            Vector2 normal_scale = Vector2.one;
            if (sceneModel == SceneModel.IMAXTheater)
            {
                normal_scale = CinemaSettings.GetInstance().ImaxQuadScreenScale;
            }
            else
            if (sceneModel == SceneModel.Default || sceneModel == SceneModel.StarringNight)
            {
                normal_scale = CinemaSettings.GetInstance().ImaxQuadScreenScale;
            }
            else
            if (sceneModel == SceneModel.HomeTheater || sceneModel == SceneModel.Drive)
            {
                normal_scale = CinemaSettings.GetInstance().HomeTheaterScreenScale;
            }
            return normal_scale;
        }

        /// <summary>
        /// 更新播放器大小
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="curr"></param>
        public void UpdatePlayerUI(SceneModel prev, SceneModel curr)
        {
            if (prev == SceneModel.Default || prev == SceneModel.StarringNight)
            {
                if (curr == SceneModel.Default || curr == SceneModel.StarringNight)
                {
                }
                if (curr == SceneModel.IMAXTheater)
                {
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().ImaxQuadScreenScale*MediaPlayerGloabVariable.IMAXTheaterScreenSizeRate/100f;
                    MediaPlayerGloabVariable.SetScreenSizeRate(MediaPlayerGloabVariable.IMAXTheaterScreenSizeRate);
                    MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_Nor);
                    MediaPlayerGloabVariable.SetSceneModel(curr);
                    ChangeAdjustScreenScale();
                    ChangeAdjustScreenSize();
                }
                if (curr == SceneModel.Drive || curr == SceneModel.HomeTheater)
                {
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().HomeTheaterScreenScale;
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localPosition = CinemaSettings.GetInstance().HomeTheaterScreenPosition;
                }
            }

            if (prev == SceneModel.IMAXTheater)
            {
                if (curr == SceneModel.Default || curr == SceneModel.StarringNight)
                {
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().ImaxQuadScreenScale*MediaPlayerGloabVariable.NormalScreenSizeRate/100f; 
                    MediaPlayerGloabVariable.SetScreenSizeRate(MediaPlayerGloabVariable.NormalScreenSizeRate);
                    MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_Nor);
                    MediaPlayerGloabVariable.SetSceneModel(curr);
                    ChangeAdjustScreenScale();
                    ChangeAdjustScreenSize();
                }
                if (curr == SceneModel.Drive || curr == SceneModel.HomeTheater)
                {
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().HomeTheaterScreenScale;
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localPosition = CinemaSettings.GetInstance().HomeTheaterScreenPosition;
                }
            }

            if (prev == SceneModel.Drive || prev == SceneModel.HomeTheater)
            {
                if (curr == SceneModel.Drive || curr == SceneModel.HomeTheater)
                {
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().HomeTheaterScreenScale;
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localPosition = CinemaSettings.GetInstance().HomeTheaterScreenPosition;
                }

                if (curr == SceneModel.Default || curr == SceneModel.StarringNight)
                {
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().ImaxQuadScreenScale*MediaPlayerGloabVariable.NormalScreenSizeRate/100f;
                    MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_Nor);
                    MediaPlayerGloabVariable.SetScreenSizeRate(MediaPlayerGloabVariable.NormalScreenSizeRate);
                    MediaPlayerGloabVariable.SetSceneModel(curr);
                    ChangeAdjustScreenScale();
                    ChangeAdjustScreenSize();
                }
                if (curr == SceneModel.IMAXTheater)
                {
                    PlayerGameobjectControl.Instance.QuadScreen.transform.localScale = CinemaSettings.GetInstance().ImaxQuadScreenScale*MediaPlayerGloabVariable.IMAXTheaterScreenSizeRate/100f;
                    MediaPlayerGloabVariable.SetScreenScaleType(ScreenScaleType.S_Nor);
                    MediaPlayerGloabVariable.SetScreenSizeRate(MediaPlayerGloabVariable.IMAXTheaterScreenSizeRate);
                    MediaPlayerGloabVariable.SetSceneModel(curr);
                    ChangeAdjustScreenScale();
                    ChangeAdjustScreenSize();
                }
            }
        }

        public void SwitchUIVision()
        {
            if (IsKeepShowUI)
                return;
            if (!IsShowUIForAutoMode)
                ShowUI();
            else
                HideUI();
        }

        public void ShowUI()
        {
            if (!Cinema.IsPlayMode) return;
            IsKeepShowUI = false;
            IsShowUIForAutoMode = true;

            if (MediaPlayerGloabVariable.CurPlayType == PlayType.Video)
            {
                UIIsShow = true;
                VideoCtrlPanel.Show();
                //场景出现
                if (Cinema.IsInLockAngle)
                {
                    if(ShowUICallback != null)
                        ShowUICallback();
                    HeadRecenter.GetInstance().LockCamera(false);
                    Cinema.IsInLockAngle = false;
                }
            }
            else if (MediaPlayerGloabVariable.CurPlayType == PlayType.Picture)
                ImageCtrlPanel.Show();

            TPAndVCPUIIsShow = true;
            ShowControllerRayLine();
            AutoHideUI();
        }

        public void HideUI()
        {
            LogTool.Log("隐藏VPUI");
            if (!Cinema.IsPlayMode)
                return;

            IsKeepShowUI = false;
            IsShowUIForAutoMode = false;
            UIIsShow = false;

            if (MediaPlayerGloabVariable.CurPlayType == PlayType.Video)
                VideoCtrlPanel.Hide();
            else if (MediaPlayerGloabVariable.CurPlayType == PlayType.Picture)
                ImageCtrlPanel.Hide();

            TPAndVCPUIIsShow = false;

            if ((GvrControllerInput.SvrState & (SvrControllerState.NoloLeftContoller | SvrControllerState.NoloRightContoller)) != 0)
            {
                if ((GvrControllerInput.SvrState & SvrControllerState.NoloLeftContoller) != 0 && NoloLeftController != null)
                    NoloLeftController.transform.localScale = Vector3.zero;

                if ((GvrControllerInput.SvrState & SvrControllerState.NoloRightContoller) != 0 && NoloRightController != null)
                    NoloRightController.transform.localScale = Vector3.zero;
            }
            else if (GvrControllerInput.SvrState == SvrControllerState.None && SvrReticleRayLine != null)
            {
                if (Svr.SvrSetting.IsVR9Device)
                    SVR.AtwAPI.ShowDualSurface(false);
                else
                    SvrReticleRayLine.enabled = false;
            }
            else if (GvrControllerInput.SvrState == SvrControllerState.GvrController && GvrControllerPointer != null)
                GvrControllerPointer.transform.localScale = Vector3.zero;
        }

        void StopAutoHideUI()
        {
            if (IsInvoking("HideUI"))
                CancelInvoke("HideUI");
        }

        public void AutoHideUI()
        {
            if (IsEnterPlayer && TPAndVCPUIIsShow)
                return;

            LogTool.Log("开始自动隐藏VPUI");
            IsKeepShowUI = false;
            StopAutoHideUI();
            Invoke("HideUI",5);
        }

        public void SetLoadingObjActive(bool isShow)
        {
            IsShowLoading = isShow;
            LoadingObjectl.SetActive(IsShowLoading);
        }

        void RestUIControl()
        {
            LogTool.Log("重置VPUI控制");
            IsKeepShowUI = false;
            IsShowUIForAutoMode = true;
            StopAutoHideUI();

            if (MediaPlayerGloabVariable.CurPlayType == PlayType.Video)
            {
                UIIsShow = true;
                VideoCtrlPanel.Show();
            }
            else if (MediaPlayerGloabVariable.CurPlayType == PlayType.Picture)
                ImageCtrlPanel.Show();

            TPAndVCPUIIsShow = true;
            ShowControllerRayLine();
        }

        public void SetVideoFormatTypeWhenPlayLoop(int stereoType)
        {
            if (MediaPlayerGloabVariable.CurPlayType == PlayType.Video)
                VideoFormatDictionaryDetector.GetInstance().SetVideoFormatTypeByVideoId(CurJVideoDscpInfo.id.ToString(), (int)stereoType);
            else if (MediaPlayerGloabVariable.CurPlayType == PlayType.Picture)
                ImageFormatDictionaryDetector.GetInstance().SetImageFormatTypeByImageId(CurJImageDscpInfo.id, (int)stereoType);
        }

        public override void TouchPadSlipUnifiedDirEnd(GestureDirection gestureDir)
        {
            if (MediaPlayerGloabVariable.CurPlayType != PlayType.Picture)
                return;



            if (!Cinema.IsPlayMode)
                return;

            if (IsEnterUI || IsShowLoading)
                return;

            if (IsChangeSize)
            {
                IsChangeSize = false;
                return;
            }

            if (gestureDir != GestureDirection.Left && gestureDir != GestureDirection.Right)
                return;

            if (gestureDir == GestureDirection.Left)
            {
                if (ChangeToPreviousPictureCallback != null)
                    ChangeToPreviousPictureCallback();
            }
            else if (gestureDir == GestureDirection.Right)
            {
                if (ChangeToNextPictureCallback != null)
                    ChangeToNextPictureCallback();
            }


            //重置UI
            if (UIRecenterCallback != null)
                UIRecenterCallback();

        }

        private void ShowControllerRayLine()
        {
            if ((GvrControllerInput.SvrState & (SvrControllerState.NoloLeftContoller | SvrControllerState.NoloRightContoller)) != 0)
            {
                if ((GvrControllerInput.SvrState & SvrControllerState.NoloLeftContoller) != 0 && NoloLeftController != null)
                    NoloLeftController.transform.localScale = Vector3.one;

                if ((GvrControllerInput.SvrState & SvrControllerState.NoloRightContoller) != 0 && NoloRightController != null)
                    NoloRightController.transform.localScale = Vector3.one;
            }
            else if (GvrControllerInput.SvrState == SvrControllerState.None && SvrReticleRayLine != null)
            {
                if (Svr.SvrSetting.IsVR9Device)
                    SVR.AtwAPI.ShowDualSurface(true);
                else
                    SvrReticleRayLine.enabled = true;
            }
            else if (GvrControllerInput.SvrState == SvrControllerState.GvrController && GvrControllerPointer != null)
                GvrControllerPointer.transform.localScale = Vector3.one;
        }

        public bool IsUIShowing()
        {
            return UIIsShow;
        }
    }
}


*/