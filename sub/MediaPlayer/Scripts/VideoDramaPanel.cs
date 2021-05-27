using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
    public class VideoDramaPanel : MonoBehaviour
    {
        public IMediaPlayerUIData uiData;
        
        private int totalNum; //总子集数量
        private int aviliableNum; //可用的子集数量
        private int curPlayOrderIndex; //当前播放的子集orderindex
        
        private int pageIndex; //当前选择的Page页
        private int totalPage; //总页码数量
        private int aviliblePage; //可用页码数量

        private bool dataUpdating = false;

        [SerializeField] 
        private CommonToggle[] EpisodeBtns;
        [SerializeField] 
        private List<CommonToggle> EpisodeListBtns;
        [SerializeField]
        ScrollRect ScrollRect;


        public Action<bool> PointerEnterUICallback;

        public Action<int> ChooseVideoDramaCallback;

        private bool isShow = false;

        private bool needCheckCenter = false;

        public void Init(IMediaPlayerUIData uiData)
        {
            this.uiData = uiData;
            EventTriggerListener.Get(gameObject).OnPtEnter = OnPointerEnterUI;
            EventTriggerListener.Get(gameObject).OnPtExit = OnPointerExitUI;

            for (int i = 0; i < EpisodeBtns.Length; i++) {
                EpisodeBtns[i].id = i;
                EpisodeBtns[i].group = null;
                EventTriggerListener.Get(EpisodeBtns[i].gameObject).OnPtClick += SelectTvIndexHandler;
                EventTriggerListener.Get(EpisodeBtns[i].gameObject).OnPtEnter = OnPointerEnterUI;
                EventTriggerListener.Get(EpisodeBtns[i].gameObject).OnPtExit = OnPointerExitUI;
            }
            for(int i = 0; i < EpisodeListBtns.Count; i++)
            {
                EpisodeListBtns[i].id = i;
                EpisodeListBtns[i].onValueChangedWithID.AddListener(SelectTvListHandler);
                EventTriggerListener.Get(EpisodeListBtns[i].gameObject).OnPtEnter = OnPointerEnterUI;
                EventTriggerListener.Get(EpisodeListBtns[i].gameObject).OnPtExit = OnPointerExitUI;
            }
        }

        private void InitPlayListUI()
        {
            totalNum = uiData.GetTotalDrama();
            curPlayOrderIndex = uiData.GetCurDrama();
            aviliableNum = uiData.GetAviliableDrama();

            dataUpdating = true;
            pageIndex = Mathf.CeilToInt((curPlayOrderIndex - 1) / 10);
            totalPage = (int) Math.Ceiling((decimal) totalNum / 10);
            aviliblePage = (int) Math.Ceiling((decimal) aviliableNum / 10);
            
            Debug.LogError(aviliblePage);
            
            if (EpisodeListBtns.Count >= aviliblePage)
            {
                for (int i = EpisodeListBtns.Count - 1; i > 1; i--) {
                    if (i + 1 > aviliblePage)
                    {
                        Destroy(EpisodeListBtns[i].gameObject);
                        EpisodeListBtns.RemoveAt(i);
                    }
                }
            }
            else
            {
                for (int i = EpisodeListBtns.Count - 1; i < aviliblePage - 1; i++) 
                {
                    GameObject go = Instantiate(EpisodeListBtns[0].gameObject, EpisodeListBtns[0].transform.parent);
                    EventTriggerListener.Get(go).OnPtEnter += OnPointerEnterUI;
                    EventTriggerListener.Get(go).OnPtExit += OnPointerExitUI;
                    CommonToggle com = go.GetComponent<CommonToggle>();
                    com.label.text = string.Format("{0} - {1}", (i + 1) * 10 + 1, (i + 2) * 10);
                    com.id = i + 1;
                    com.onValueChangedWithID.AddListener(SelectTvListHandler);
                    com.group = EpisodeListBtns[0].group;
                    EpisodeListBtns.Add(com);
                }
            }
            
            for (int i = 0; i < EpisodeListBtns.Count; i++)
            {
                EpisodeListBtns[i].isOn = pageIndex == i;
            }
            
            UpdateTvPlayListUI();
            needCheckCenter = true;
            dataUpdating = false;
        }
        
        private void Update()
        {
            if (needCheckCenter)
            {
                needCheckCenter = false;
                CheckCenter(false);
            }
        }

        public void UpdateTvPlayListUI()
        {
            Debug.LogError("curPlayOrderIndex " + curPlayOrderIndex);
            int onIndex = -1;
            for (int i = 0; i < EpisodeBtns.Length; i++)
            {
                CommonToggle eBtn = EpisodeBtns[i];
                int temp_oderIndex = pageIndex * 10 + 1 + i;
                eBtn.label.text = temp_oderIndex.ToString();
                eBtn.gameObject.SetActive(true);
                if (temp_oderIndex > aviliableNum)
                {
                    eBtn.gameObject.SetActive(false);
                }
                eBtn.isOn = temp_oderIndex == curPlayOrderIndex;
            }
        }

        void SelectTvIndexHandler(GameObject go)
        {
            Debug.LogError("SelectTvIndexHandler");
            CommonToggle toggle = go.GetComponent<CommonToggle>();
            
            for (int i = 0; i < EpisodeBtns.Length; i++)
            {
                EpisodeBtns[i].isOn = false;
            }
            toggle.isOn = true;
            
            int episodeOrderIndex = pageIndex * 10 + toggle.id + 1;
            if (curPlayOrderIndex != episodeOrderIndex)
            {
                curPlayOrderIndex = episodeOrderIndex;
                Debug.LogError("请求 " + curPlayOrderIndex);
                ChooseVideoDramaCallback(curPlayOrderIndex);
            }
        }
        
        void SelectTvListHandler(int id, bool isOn)
        {
            
            if (!isOn || dataUpdating)
            {
                return;
            }
            
            Debug.LogError("SelectTvListHandler" + id);
            pageIndex = id;
            CheckCenter();
            UpdateTvPlayListUI();
        }

        private void CheckCenter(bool animation = true)
        {
            float width = ScrollRect.content.rect.width - ScrollRect.GetComponent<RectTransform>().rect.width;
            Debug.LogError(width);
            if (width <= 0)
            {
                return;
            }

            CommonToggle toggle = null;
            for (int i = 0; i < EpisodeListBtns.Count; i++) {
                if (pageIndex == EpisodeListBtns[i].id)
                {
                    toggle = EpisodeListBtns[i];
                }
            }

            float x = toggle.transform.localPosition.x - 240;
            float value = x / width;
            if (value > 1)
            {
                value = 1;
            }

            if (value < 0)
            {
                value = 0;
            }

            StopAllCoroutines();
            if (animation)
            {
                StartCoroutine(Animation(ScrollRect.horizontalNormalizedPosition, value));
            }
            else
            {
                ScrollRect.horizontalNormalizedPosition = value;
            }

        }

        private IEnumerator Animation(float start, float end)
        {
            float time = 0;
            while (time <= 1)
            {
                time = time + Time.deltaTime * 8;
                if (time > 1)
                {
                    time = 1;
                }
                ScrollRect.horizontalNormalizedPosition = Mathf.Lerp(start, end, time);
                yield return 0;
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

        public void Hide()
        {
            isShow = false;
            gameObject.SetActive(false);
        }

        public void Show()
        {
            isShow = true;
            InitPlayListUI();
            gameObject.SetActive(true);
        }

    }
}

