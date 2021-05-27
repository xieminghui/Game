using UnityEngine;
using System;

namespace MediaPlayer
{
    public enum SceneModel
    {
        Default, StarringNight, IMAXTheater,
        //汽车影院(默认Drive_Playboy)
        Drive,
        HomeTheater
    }

    public enum DriveSceneModel { Karting, King, Playboy, Rattletrap }

    public class SceneChangePanel : MonoBehaviour
    {
        public CommonToggle DefaultBtn;
        public CommonToggle ImaxTheaterBtn;
        public CommonToggle StarrySkyBtn;
        public CommonToggle DriveBtn;
        public CommonToggle HomeTheaterBtn;
        bool IsShow;

        public Action<bool> PointerEnterUICallback;
        public Action<SceneModel> ChangeSceneStyleCallback;

        public TransitionButton preBtn;
        public TransitionButton nextBtn;

        public RectTransform contentTransform;

        public void Init()
        {
            IsShow = false;

            DefaultBtn.onValueChanged.AddListener(SelectDefaultScene);
            StarrySkyBtn.onValueChanged.AddListener(SelectStarrySkyScene);
            ImaxTheaterBtn.onValueChanged.AddListener(SelectImaxScene);
            DriveBtn.onValueChanged.AddListener(SelectDriveScene);
            HomeTheaterBtn.onValueChanged.AddListener(SelectHomeTheaterScene);

            EventTriggerListener.Get(gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(DefaultBtn.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(DefaultBtn.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(StarrySkyBtn.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(StarrySkyBtn.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(ImaxTheaterBtn.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(ImaxTheaterBtn.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(DriveBtn.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(DriveBtn.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(HomeTheaterBtn.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(HomeTheaterBtn.gameObject).OnPtExit = OnPointerExitUI;
        
            EventTriggerListener.Get(preBtn.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(preBtn.gameObject).OnPtExit = OnPointerExitUI;
            EventTriggerListener.Get(nextBtn.gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(nextBtn.gameObject).OnPtExit = OnPointerExitUI;
        
            preBtn.onClick.AddListener(OnClickPreBtn);
            nextBtn.onClick.AddListener(OnClickNextBtn);
        
            preBtn.interactable = false;
        }

        #region Button Event
        void SelectDefaultScene(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (ChangeSceneStyleCallback != null)
                ChangeSceneStyleCallback(SceneModel.Default);
        }
    
        void SelectStarrySkyScene(bool isOn)
        {
            if (!isOn)
            {
                return;
            }
            
            if (ChangeSceneStyleCallback != null)
                ChangeSceneStyleCallback(SceneModel.StarringNight);
        }
    
        void SelectImaxScene(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (ChangeSceneStyleCallback != null)
                ChangeSceneStyleCallback(SceneModel.IMAXTheater);
        }

        void SelectDriveScene(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (ChangeSceneStyleCallback != null)
                ChangeSceneStyleCallback(SceneModel.Drive);
        }

        void SelectHomeTheaterScene(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (ChangeSceneStyleCallback != null)
                ChangeSceneStyleCallback(SceneModel.HomeTheater);
        }

        private void OnClickNextBtn()
        {
            contentTransform.anchoredPosition = new Vector2(-462, contentTransform.anchoredPosition.y);
            preBtn.interactable = true;
            nextBtn.interactable = false;
        }
    
        private void OnClickPreBtn()
        {
            contentTransform.anchoredPosition = new Vector2(0, contentTransform.anchoredPosition.y);
            preBtn.interactable = false;
            nextBtn.interactable = true;
        }
    
        #endregion
        
        private void SetBtnStatus()
        {
            SceneModel sceneModel = MediaPlayerGloabVariable.GetSceneModel();
            if (sceneModel == SceneModel.Default)
            {
                DefaultBtn.isOn = true;
            }
            else if (sceneModel == SceneModel.StarringNight)
            {
                StarrySkyBtn.isOn = true;

            }
            else if (sceneModel == SceneModel.IMAXTheater)
            {
                ImaxTheaterBtn.isOn = true;
            }
            else if (sceneModel == SceneModel.Drive)
            {
                DriveBtn.isOn = true;
            }
            else if (sceneModel == SceneModel.HomeTheater)
            {
                HomeTheaterBtn.isOn = true;
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
            SetBtnStatus();

            gameObject.SetActive(true);
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
            DefaultBtn = null;
            ImaxTheaterBtn = null;
            DriveBtn = null;
            StarrySkyBtn = null;
            HomeTheaterBtn = null;
            PointerEnterUICallback = null;
            ChangeSceneStyleCallback = null;
        }
    }
}

