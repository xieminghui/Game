
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MediaPlayer
{
	public class SubtitleItem :  MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[SerializeField]
		private Text title;
		[SerializeField]
		private GameObject chooseMask;

		private bool _choose = false;

		private bool pointerIn = false;
	
		private Color chooseColor = new Color(0.05882f, 0.6549f, 0.984313f);
		private Color nomalColor = new Color(0.40784f, 0.42745f, 0.447058f);


		public Action<string> OnClickSubtitleItem;

		public string path;

		public void SetSubtitleInfo(string _path)
		{
			path = _path;
			if (path == "")
			{
				title.text = "无";
			}
			else
			{
				title.text = Path.GetFileNameWithoutExtension(path);
			}
		}

		public bool Choose
		{
			get { return _choose; }
			set { _choose = value; UpdateState();}
		}

		private void OnDisable()
		{
			pointerIn = false;
			UpdateState();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			pointerIn = true;
			UpdateState();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			pointerIn = false;
			UpdateState();
		}

		private void UpdateState()
		{
			if (Choose)
			{
				chooseMask.SetActive(true);
				title.color = chooseColor;
			}
			else if (pointerIn)
			{
				chooseMask.SetActive(false);
				title.color = Color.white;
			}
			else
			{
				chooseMask.SetActive(false);
				title.color = nomalColor;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (OnClickSubtitleItem != null)
			{
				OnClickSubtitleItem.Invoke(path);
			}
		}
	}

}

