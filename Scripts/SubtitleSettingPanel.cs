
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class SubtitleSettingPanel : MonoBehaviour 
    {
        [SerializeField]
        private Text curSubtitleItemName;
        [SerializeField]
        private Image curSubtitleItemIcon;
        [SerializeField]
        private Image curSubtitleItemBg;
        [SerializeField]
        private GameObject moreSubtitlePanel;
        [SerializeField]
        List<SubtitleItem> SubtitleItemList = new List<SubtitleItem>();

        bool IsEnter;
        bool IsShow;

        private int videoID;
    
        private Color chooseColor = new Color(0.05882f, 0.6549f, 0.984313f);
        private Color nomalColor = new Color(0.40784f, 0.42745f, 0.447058f);

        private List<string> subtitleList = new List<string>();

        private bool showMoreSubtitle = false;
        private bool pointerInCurSubtitleItem = false;
        private bool subtitleItemEnable = false;
        private bool moreSubtitleEnable = false;

        public Action<bool> PointerEnterUICallback;
    
        public Action<string> ChangeSubtitleCallback;
        
        public void Init()
        {
            IsEnter = false;
            IsShow = false;
        
            EventTriggerListener.Get(gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(gameObject).OnPtExit += OnPointerExit;
        
            EventTriggerListener.Get(curSubtitleItemBg.gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(curSubtitleItemBg.gameObject).OnPtExit += OnPointerExit;
        
            EventTriggerListener.Get(curSubtitleItemBg.gameObject).OnPtEnter += OnPointerEnterItem;
            EventTriggerListener.Get(curSubtitleItemBg.gameObject).OnPtExit += OnPointerExitItem;
        
            EventTriggerListener.Get(curSubtitleItemBg.gameObject).OnPtClick += OnClickCurTrackItem;
        
            SubtitleItemList[0].OnClickSubtitleItem = OnChooseSubtitleItem;
            SubtitleItemList[1].OnClickSubtitleItem = OnChooseSubtitleItem;
        
            EventTriggerListener.Get(SubtitleItemList[0].gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(SubtitleItemList[0].gameObject).OnPtExit += OnPointerExit;
            EventTriggerListener.Get(SubtitleItemList[1].gameObject).OnPtEnter += OnPointerEnter;
            EventTriggerListener.Get(SubtitleItemList[1].gameObject).OnPtExit += OnPointerExit;
        }

        public void SetSubtitleInfo(int _videoID, List<string> info)
        {
            subtitleList.Clear();
            subtitleList.Add("");
            if (info != null)
            {
                subtitleList.AddRange(info);
            }

            videoID = _videoID;
        }

        public void Show()
        {
            if (IsShow)
                return;

            IsShow = true;
            gameObject.SetActive(true);

            string curSubtitle = VideoSubtitleDictionaryDetector.GetInstance().GetVideoFormatTypeByVideoId(videoID);
            Debug.LogError("1");
            if (string.IsNullOrEmpty(curSubtitle))
            {
                curSubtitleItemName.text = "无";
            }
            else
            {
                Debug.LogError(curSubtitle);
                curSubtitleItemName.text = Path.GetFileNameWithoutExtension(curSubtitle);
            }
            Debug.LogError("2");
            moreSubtitleEnable = subtitleList.Count > 1;
            curSubtitleItemIcon.gameObject.SetActive(moreSubtitleEnable);
            if (moreSubtitleEnable)
            {
                CreateSubtitleItem();
            }
            else
            {
                ClearMoreSubtitleItem();
            }
            Debug.LogError("3");
            showMoreSubtitle = false;
            pointerInCurSubtitleItem = false;
            UpdateCurSubtitleState();
            Debug.LogError("4");
            moreSubtitlePanel.gameObject.SetActive(false);
        }

        private void CreateSubtitleItem()
        {
            Debug.LogError("5");
            for (int i = SubtitleItemList.Count - 1; i >= 2; i--)
            {
                if (i >= subtitleList.Count)
                {
                    Destroy(SubtitleItemList[i].gameObject);
                    SubtitleItemList.Remove(SubtitleItemList[i]);
                }
            }
            Debug.LogError("6");
            for (int i = SubtitleItemList.Count; i < subtitleList.Count; i++)
            {
                GameObject go = Instantiate(SubtitleItemList[1].gameObject, SubtitleItemList[1].transform.parent);
                EventTriggerListener.Get(go).OnPtEnter += OnPointerEnter;
                EventTriggerListener.Get(go).OnPtExit += OnPointerExit;
                SubtitleItemList.Add(go.GetComponent<SubtitleItem>());
            }
            Debug.LogError("7");
            string curSubtitle = VideoSubtitleDictionaryDetector.GetInstance().GetVideoFormatTypeByVideoId(videoID);
            if (curSubtitle == null)
            {
                curSubtitle = "";
            }

            for (int i = 0; i < SubtitleItemList.Count; i++)
            {
                SubtitleItemList[i].SetSubtitleInfo(subtitleList[i]);
                SubtitleItemList[i].OnClickSubtitleItem = OnChooseSubtitleItem;
                if (subtitleList[i] == curSubtitle)
                {
                    SubtitleItemList[i].Choose = true;
                }
                else
                {
                    SubtitleItemList[i].Choose = false;
                }
            }
        }

        private void ClearMoreSubtitleItem()
        {
            for (int i = 2; i < SubtitleItemList.Count; i++)
            {
                Destroy(SubtitleItemList[i].gameObject);
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
            pointerInCurSubtitleItem = true;
            UpdateCurSubtitleState();
            OnPointerEnter(go);
        }

        void OnPointerExitItem(GameObject go)
        {
            pointerInCurSubtitleItem = false;
            UpdateCurSubtitleState();
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
            if (!moreSubtitleEnable)
                return;
        
            showMoreSubtitle = !showMoreSubtitle;
            moreSubtitlePanel.gameObject.SetActive(showMoreSubtitle);
            UpdateCurSubtitleState();
        }

        private void UpdateCurSubtitleState()
        {
            if (showMoreSubtitle)
            {
                curSubtitleItemName.color = chooseColor;
                curSubtitleItemIcon.color = chooseColor;
                curSubtitleItemIcon.transform.localScale = new Vector3(1,-1,1);
            }
            else
            {
                curSubtitleItemIcon.transform.localScale = Vector3.one;
                if (!moreSubtitleEnable)
                {
                    curSubtitleItemName.color = nomalColor;
                    curSubtitleItemIcon.color = nomalColor;
                }
                else if (pointerInCurSubtitleItem)
                {
                    curSubtitleItemName.color = Color.white;
                    curSubtitleItemIcon.color = Color.white;
                }
                else
                {
                    curSubtitleItemName.color = nomalColor;
                    curSubtitleItemIcon.color = nomalColor;
                }
            }
        }

        private void OnChooseSubtitleItem(string path)
        {
            string curSubtitle = VideoSubtitleDictionaryDetector.GetInstance().GetVideoFormatTypeByVideoId(videoID);
            if (curSubtitle == path)
            {
                return;
            }

            if (ChangeSubtitleCallback != null)
            {
                ChangeSubtitleCallback.Invoke(path);
            }
            
            if (path == "")
            {
                curSubtitleItemName.text = "无";
            }
            else
            {
                curSubtitleItemName.text = Path.GetFileNameWithoutExtension(path);
            }

            for (int i = 0; i < SubtitleItemList.Count; i++)
            {
                if (SubtitleItemList[i].path == path)
                {
                    SubtitleItemList[i].Choose = true;
                }
                else
                {
                    SubtitleItemList[i].Choose = false;
                }
            }
        
        }
    
}

}

