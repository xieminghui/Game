/*
 * Author:李传礼
 * DateTime:2017.12.21
 * Description:立体类型面板
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Remoting.Channels;

namespace MediaPlayer
{
    public class StereoTypePanel : VRInputMessageBase
    {
        public CommonToggle[] stereoTypeBtns;
        public CommonToggle[] projectionTypeBtns;
        public CommonToggle tensileBtn;

        public ColorToggle enable3DToggle;

        int STBIndex;//立体类型按钮索引
        int PTBIndex;//投影类型按钮索引
        bool IsShow;
        
        public Action<StereoType> ChangeStereoTypeCallback;
        public Action<bool> PointerEnterUICallback;
        public Action<bool> TensileSwitchCallback;
    
        public Action<bool> Change3DEnableCallback;

        private IMediaPlayerUIData uiData;
        public void Init(IMediaPlayerUIData _uiData)
        {
            uiData = _uiData;
                
            STBIndex = 0;
            PTBIndex = 0;
            IsShow = false;
            for (int i = 0; i < stereoTypeBtns.Length; i++)
            {
                stereoTypeBtns[i].id = i;
                stereoTypeBtns[i].onValueChangedWithID.AddListener(SelectSTB);
                EventTriggerListener.Get(stereoTypeBtns[i].gameObject).OnPtEnter += OnPointerEnter;
                EventTriggerListener.Get(stereoTypeBtns[i].gameObject).OnPtExit += OnPointerExit;
            }

            for (int i = 0; i < projectionTypeBtns.Length; i++)
            {
                projectionTypeBtns[i].id = i;
                projectionTypeBtns[i].onValueChangedWithID.AddListener(SelectPTB);
                EventTriggerListener.Get(projectionTypeBtns[i].gameObject).OnPtEnter += OnPointerEnter;
                EventTriggerListener.Get(projectionTypeBtns[i].gameObject).OnPtExit += OnPointerExit;
            }

            tensileBtn.onValueChanged.AddListener(ClickTensileBtn);
            
            enable3DToggle.onValueChanged.AddListener(OnToggle3DBtn);

            EventTriggerListener.Get(gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(gameObject).OnPtExit += OnPointerExit;
        
            EventTriggerListener.Get(enable3DToggle.gameObject).OnPtEnter = OnPointerEnter;
            EventTriggerListener.Get(enable3DToggle.gameObject).OnPtExit = OnPointerExit;

            
            EventTriggerListener.Get(tensileBtn.gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(tensileBtn.gameObject).OnPtExit += OnPointerExit;
        }

        #region Button Event
        void SelectSTB(int id, bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (STBIndex != id)
            {
                STBIndex = id;
                Set3DToggleVisible(STBIndex == 1 || STBIndex == 2);
                StereoType st = GetStereoType(STBIndex, PTBIndex);
                if (ChangeStereoTypeCallback != null)
                    ChangeStereoTypeCallback(st);
            }
        }
        
        void SelectPTB(int id, bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            if (id != PTBIndex)
            {
                PTBIndex = id;
                StereoType st = GetStereoType(STBIndex, PTBIndex);
                if (ChangeStereoTypeCallback != null)
                    ChangeStereoTypeCallback(st);
            }
        }
        #endregion

        StereoType GetStereoType(int stbIndex, int ptbIndex)
        {
            if(stbIndex == 0)
            {
                if (ptbIndex == 0)
                    return StereoType.ST2D;
                else if (ptbIndex == 1)
                    return StereoType.ST180_2D;
                else if (ptbIndex == 2)
                    return StereoType.ST360_2D;
                else
                    return StereoType.STFISH_2D;
            }
            else if( stbIndex == 1)
            {
                if (ptbIndex == 0)
                    return StereoType.ST3D_LR;
                else if (ptbIndex == 1)
                    return StereoType.ST180_LR;
                else if (ptbIndex == 2)
                    return StereoType.ST360_LR;
                else 
                    return StereoType.STFISH_LR;
            }
            else
            {
                if (ptbIndex == 0)
                    return StereoType.ST3D_TB;
                else if (ptbIndex == 1)
                    return StereoType.ST180_TB;
                else if (ptbIndex == 2)
                    return StereoType.ST360_TB;
                else 
                    return StereoType.STFISH_TB;
            }
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

        void ClickTensileBtn(bool isOn)
        {
            if (TensileSwitchCallback != null)
                TensileSwitchCallback(tensileBtn.isOn);
        }

        void TensileBtnSelectControl()
        {
            tensileBtn.isOn = MediaStretchPlayerPrefsDetector.GetInstance().GetMediaStretchKey();
            Debug.LogError("TensileBtnSelectControl " + tensileBtn.isOn);
        }

        public void TensileBtnControl(StereoType stereoType)
        {
            switch (stereoType)
            {
                case StereoType.ST3D_LR:
                case StereoType.ST3D_TB:
                    tensileBtn.interactable = true;
                    break;
                default:
                    tensileBtn.interactable = false;
                    break;
            }
            Debug.LogError("TensileBtnControl " + tensileBtn.interactable);
        }

        public void OnToggle3DBtn(bool on)
        {
            bool enable = MediaPlayerGloabVariable.Get3DEnable();
            if (on != enable && Change3DEnableCallback != null)
            {
                Change3DEnableCallback.Invoke(on);
            }
        }
        
        public void Show()
        {
            if (IsShow)
                return;
            IsShow = true;
            gameObject.SetActive(true);
            
            stereoTypeBtns[0].group.SetAllTogglesOff();
            projectionTypeBtns[0].group.SetAllTogglesOff();
            TensileBtnSelectControl();
            StereoType stereoType = uiData.GetCurStereoType();
            
            enable3DToggle.isOn = MediaPlayerGloabVariable.Get3DEnable();
            switch (stereoType)
            {
                case StereoType.ST2D:
                    STBIndex = 0;
                    PTBIndex = 0;
                    break;
                case StereoType.ST180_2D:
                    STBIndex = 0;
                    PTBIndex = 1;
                    break;
                case StereoType.ST360_2D:
                    STBIndex = 0;
                    PTBIndex = 2;
                    break;
                case StereoType.STFISH_2D:
                    STBIndex = 0;
                    PTBIndex = 3;
                    break;
                case StereoType.ST3D_LR:
                    STBIndex = 1;
                    PTBIndex = 0;
                    break;
                case StereoType.ST180_LR:
                    STBIndex = 1;
                    PTBIndex = 1;
                    break;
                case StereoType.ST360_LR:
                    STBIndex = 1;
                    PTBIndex = 2;
                    break;
                case StereoType.STFISH_LR:
                    STBIndex = 1;
                    PTBIndex = 3;
                    break;
                case StereoType.ST3D_TB:
                    STBIndex = 2;
                    PTBIndex = 0;
                    break;
                case StereoType.ST180_TB:
                    STBIndex = 2;
                    PTBIndex = 1;
                    break;
                case StereoType.ST360_TB:
                    STBIndex = 2;
                    PTBIndex = 2;
                    break;
                case StereoType.STFISH_TB:
                    STBIndex = 2;
                    PTBIndex = 3;
                    break;
            }
            Set3DToggleVisible(STBIndex == 1 || STBIndex == 2);
            
            for (int i = 0; i < stereoTypeBtns.Length; i++)
            {
                stereoTypeBtns[i].isOn = STBIndex == i;
            }

            for (int i = 0; i < projectionTypeBtns.Length; i++)
            {
                projectionTypeBtns[i].isOn = i == PTBIndex;
            }

            TensileBtnControl(stereoType);
        }

        private void Set3DToggleVisible(bool visible)
        {
            enable3DToggle.gameObject.SetActive(visible);
        }

        public void Hide()
        {
            if (!IsShow)
                return;

            IsShow = false;
            gameObject.SetActive(false);
        }
    }

}

