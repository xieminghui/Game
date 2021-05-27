using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class VoiceSettingPanel : MonoBehaviour 
    {
        [SerializeField]
        private Text curTrackItemName;
        [SerializeField]
        private Image curTrackItemIcon;
        [SerializeField]
        private Image curTrackItemBg;
        [SerializeField]
        private GameObject moreTrackPanel;
        [SerializeField]
        List<TrackItem> TrackItemList = new List<TrackItem>();

        bool IsEnter;
        bool IsShow;
    
        private Color chooseColor = new Color(0.05882f, 0.6549f, 0.984313f);
        private Color nomalColor = new Color(0.40784f, 0.42745f, 0.447058f);

        private AudioTrackInfo audioInfo;

        private bool showMoreTrack = false;
        private bool pointerInCurTrackItem = false;
        private bool trackItemEnable = false;
        private bool moreTrackEnable = false;

        public Action<bool> PointerEnterUICallback;
    
        public Action<int> ChangeAudioTrackCallback;
        public void Init()
        {
            IsEnter = false;
            IsShow = false;
        
            EventTriggerListener.Get(gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(gameObject).OnPtExit += OnPointerExit;
        
            EventTriggerListener.Get(curTrackItemBg.gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(curTrackItemBg.gameObject).OnPtExit += OnPointerExit;
        
            EventTriggerListener.Get(curTrackItemBg.gameObject).OnPtEnter += OnPointerEnterItem;
            EventTriggerListener.Get(curTrackItemBg.gameObject).OnPtExit += OnPointerExitItem;
        
            EventTriggerListener.Get(curTrackItemBg.gameObject).OnPtClick += OnClickCurTrackItem;
        
            TrackItemList[0].OnClickTrackItem = OnChooseTrackItem;
            TrackItemList[1].OnClickTrackItem = OnChooseTrackItem;
        
            EventTriggerListener.Get(TrackItemList[0].gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(TrackItemList[0].gameObject).OnPtExit += OnPointerExit;
            EventTriggerListener.Get(TrackItemList[1].gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(TrackItemList[1].gameObject).OnPtExit += OnPointerExit;
        }

        public void SetAudioTrackInfo(AudioTrackInfo info)
        {
            audioInfo = info;
        }

        public void Show()
        {
            if (IsShow)
                return;

            IsShow = true;
            gameObject.SetActive(true);


            if (audioInfo == null || audioInfo.trackInfos == null)
            {
                curTrackItemName.text = "----";
                curTrackItemIcon.gameObject.SetActive(false);
                moreTrackEnable = false;
                if (audioInfo == null)
                {
                    LogTool.Log("audioInfo == null");
                }
                else if (audioInfo.trackInfos == null)
                {
                    LogTool.Log("audioInfo.trackInfos == null");
                }
            }
            else
            {
                for (int i = 0; i < audioInfo.trackInfos.Count; i++)
                {
                    if (audioInfo.selectIndex == audioInfo.trackInfos[i].id)
                    {
                        curTrackItemName.text = audioInfo.trackInfos[i].GetTrackDescribe();
                    }
                }
            
                moreTrackEnable = audioInfo.trackInfos.Count > 1;
                curTrackItemIcon.gameObject.SetActive(moreTrackEnable);
                if (moreTrackEnable)
                {
                    CreateTrackItem();
                }
                else
                {
                    ClearMoreTrackItem();
                }
            }
        
            showMoreTrack = false;
            pointerInCurTrackItem = false;
            UpdateCurTrackState();
        
            moreTrackPanel.gameObject.SetActive(false);
        }

        private void CreateTrackItem()
        {
            for (int i = TrackItemList.Count - 1; i >= 2; i--)
            {
                if (i >= audioInfo.trackInfos.Count)
                {
                    Destroy(TrackItemList[i].gameObject);
                    TrackItemList.Remove(TrackItemList[i]);
                }
            }

            for (int i = TrackItemList.Count; i < audioInfo.trackInfos.Count; i++)
            {
                GameObject go = Instantiate(TrackItemList[1].gameObject, TrackItemList[1].transform.parent);
                EventTriggerListener.Get(go).OnPtEnter += OnPointerEnter;
                EventTriggerListener.Get(go).OnPtExit += OnPointerExit;
                TrackItemList.Add(go.GetComponent<TrackItem>());
            }

            for (int i = 0; i < TrackItemList.Count; i++)
            {
                TrackItemList[i].SetTrackInfo(audioInfo.trackInfos[i]);
                TrackItemList[i].OnClickTrackItem = OnChooseTrackItem;
                if (audioInfo.trackInfos[i].id == audioInfo.selectIndex)
                {
                    TrackItemList[i].Choose = true;
                }
                else
                {
                    TrackItemList[i].Choose = false;
                }
            
            }
        }

        private void ClearMoreTrackItem()
        {
            for (int i = 2; i < TrackItemList.Count; i++)
            {
                Destroy(TrackItemList[i].gameObject);
            }
        }

        public void Hide()
        {
            if (!IsShow)
                return;

            IsShow = false;
            gameObject.SetActive(false);
        }
    
        void OnPointerEnterItem(GameObject go)
        {
            pointerInCurTrackItem = true;
            UpdateCurTrackState();
            OnPointerEnter(go);
        }

        void OnPointerExitItem(GameObject go)
        {
            pointerInCurTrackItem = false;
            UpdateCurTrackState();
            OnPointerExit(go);
        } 
    
        void OnPointerEnter(GameObject go)
        {
            IsEnter = true;

            if (PointerEnterUICallback != null)
                PointerEnterUICallback(true);
        }

        void OnPointerExit(GameObject go)
        {
            if (!IsEnter)
                return;

            IsEnter = false;

            if (PointerEnterUICallback != null)
                PointerEnterUICallback(false);
        }

        public void OnClickCurTrackItem(GameObject go)
        {
            if (!moreTrackEnable)
                return;
        
            showMoreTrack = !showMoreTrack;
            moreTrackPanel.gameObject.SetActive(showMoreTrack);
            UpdateCurTrackState();
        }

        private void UpdateCurTrackState()
        {
            if (showMoreTrack)
            {
                curTrackItemName.color = chooseColor;
                curTrackItemIcon.color = chooseColor;
                curTrackItemIcon.transform.localScale = new Vector3(1,-1,1);
            }
            else
            {
                curTrackItemIcon.transform.localScale = Vector3.one;
                if (!moreTrackEnable)
                {
                    curTrackItemName.color = nomalColor;
                    curTrackItemIcon.color = nomalColor;
                }
                else if (pointerInCurTrackItem)
                {
                    curTrackItemName.color = Color.white;
                    curTrackItemIcon.color = Color.white;
                }
                else
                {
                    curTrackItemName.color = nomalColor;
                    curTrackItemIcon.color = nomalColor;
                }
            }
        }

        private void OnChooseTrackItem(int id)
        {
            if (id == audioInfo.selectIndex)
            {
                return;
            }

            if (ChangeAudioTrackCallback != null)
            {
                ChangeAudioTrackCallback.Invoke(id);
            }

            audioInfo.selectIndex = id;
        
        
            for (int i = 0; i < TrackItemList.Count; i++)
            {
                if (TrackItemList[i].ID == audioInfo.selectIndex)
                {
                    TrackItemList[i].Choose = true;
                    curTrackItemName.text = audioInfo.trackInfos[i].GetTrackDescribe();
                }
                else
                {
                    TrackItemList[i].Choose = false;
                }
            }
        
        }
    
}

}

