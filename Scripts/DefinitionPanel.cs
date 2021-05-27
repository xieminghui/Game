using System.Collections.Generic;
using UnityEngine;
using System;
namespace MediaPlayer
{
    public class DefinitionPanel : MonoBehaviour
    {
        public CommonToggle FHDBtn; //4K
        public CommonToggle AutoBtn; //自动
        public CommonToggle FluentBtn;//流畅
        public CommonToggle[] ExtendBtns;//扩展控件
        
        public IMediaPlayerUIData uiData;

        [HideInInspector]
        public DefinitionModel CurDefinitionModel; //用于UI显示当前选择的清晰度
        private DefinitionModel AutoDefinitionModel; //用于记录首次自动选择的清晰度
        bool IsEnter;
        bool IsShow;
        int FourKBenefitType = 1; //对4K清晰度享有的权益;0-CAN_PLAY(可播) 1-CAN_NOT_PLAY(不可播) 2-PREVIEW(预览)

        public Action<bool> PointerEnterUICallback;
        public Action<GameObject, bool> PointerEnterBtnCallback;
        
        public Action<DefinitionModel> ChangeSkyVideoDefinitionCallback;

        public void Init(IMediaPlayerUIData uiData)
        {
            this.uiData = uiData;
            IsEnter = false;
            IsShow = false;
            CurDefinitionModel = DefinitionModel.UNKOWN;
            AutoDefinitionModel = DefinitionModel.UNKOWN;

            EventTriggerListener.Get(gameObject).OnPtEnter = OnPointerEnterPanel;
            EventTriggerListener.Get(gameObject).OnPtExit = OnPointerExitPanel;
            EventTriggerListener.Get(FHDBtn.gameObject).OnPtEnter = OnPointerEnterBtn;
            EventTriggerListener.Get(FHDBtn.gameObject).OnPtExit = OnPointerExitBtn;
            EventTriggerListener.Get(AutoBtn.gameObject).OnPtEnter = OnPointerEnterBtn;
            EventTriggerListener.Get(AutoBtn.gameObject).OnPtExit = OnPointerExitBtn;
            EventTriggerListener.Get(FluentBtn.gameObject).OnPtEnter = OnPointerEnterBtn;
            EventTriggerListener.Get(FluentBtn.gameObject).OnPtExit = OnPointerExitBtn;

            ExtendBtns[0].id = (int)DefinitionModel.DEFINITION_LIVE_4K_N;
            ExtendBtns[1].id = (int)DefinitionModel.DEFINITION_LIVE_4K_H;
            ExtendBtns[2].id = (int)DefinitionModel.DEFINITION_LIVE_4K_R;
            ExtendBtns[3].id = (int)DefinitionModel.DEFINITION_LIVE_8K_N;
            ExtendBtns[4].id = (int)DefinitionModel.DEFINITION_LIVE_8K_H;
            ExtendBtns[5].id = (int)DefinitionModel.DEFINITION_LIVE_8K_R;
            
            foreach(CommonToggle extendBtn in ExtendBtns)
            {
                EventTriggerListener.Get(extendBtn.gameObject).OnPtEnter = OnPointerEnterBtn;
                EventTriggerListener.Get(extendBtn.gameObject).OnPtExit = OnPointerExitBtn;
                extendBtn.onValueChangedWithID.AddListener(SelectExtendBtn);
            }

            FHDBtn.onValueChanged.AddListener(SelectFHDBtn);
            AutoBtn.onValueChanged.AddListener(SelectAutoBtn);
            FluentBtn.onValueChanged.AddListener(SelectFluentBtn);
            FHDBtn.gameObject.SetActive(false);
        }

        private void SetUIState()
        {

            CurDefinitionModel = uiData.GetCurDefinitionModel();
            
#if SVR_VR9
            FluentBtn.gameObject.SetActive(false);
#else
            
            bool isAuto = true;
            List<int> definitionCodeList = uiData.GetAllDefinitionCode();
            
            if (definitionCodeList != null)
            {
                for (int i = 0; i < ExtendBtns.Length; i++)
                {
                    ExtendBtns[i].gameObject.SetActive(definitionCodeList.Contains(ExtendBtns[i].id));
                    if ((int)CurDefinitionModel == ExtendBtns[i].id)
                    {
                        ExtendBtns[i].isOn = true;
                        isAuto = false;
                    }
                }
                
                if (isAuto)
                {
                    AutoBtn.gameObject.SetActive(true);
                    AutoBtn.isOn = true;
                }
                else
                {
                    AutoBtn.gameObject.SetActive(false);
                }
            }
#endif
        }

        void SelectFluentBtn(bool isSelect)
        {

        }

        void SelectExtendBtn(int id, bool isSelect)
        {
            if ((int) CurDefinitionModel == id)
            {
                return;
            }

            CurDefinitionModel = (DefinitionModel)id;

            if (ChangeSkyVideoDefinitionCallback != null)
                ChangeSkyVideoDefinitionCallback(CurDefinitionModel);
        }

        void SelectFHDBtn(bool isSelect)
        {

        }

        void SelectAutoBtn(bool isSelect)
        {

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

        void OnPointerEnterBtn(GameObject go)
        {
            if (PointerEnterBtnCallback != null)
                PointerEnterBtnCallback(go, true);
        }

        void OnPointerExitBtn(GameObject go)
        {
            if (PointerEnterBtnCallback != null)
                PointerEnterBtnCallback(go, false);
        }

        public void Show()
        {
            if (IsShow)
                return;
            IsShow = true;
            SetUIState();

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
            FHDBtn = null;
            AutoBtn = null;
            FluentBtn = null;
            PointerEnterUICallback = null;
            PointerEnterBtnCallback = null;
            FourKBenefitType = 1;
        }
    }
}

