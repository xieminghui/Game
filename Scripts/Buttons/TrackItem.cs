using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MediaPlayer
{
	public class TrackItem :  MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[SerializeField]
		private Text title;
		[SerializeField]
		private GameObject chooseMask;

		private bool _choose = false;

		private bool pointerIn = false;
	
		private Color chooseColor = new Color(0.05882f, 0.6549f, 0.984313f);
		private Color nomalColor = new Color(0.40784f, 0.42745f, 0.447058f);

		private int id = -1;
		public int ID
		{
			get { return id; }
		}


		public Action<int> OnClickTrackItem;

		public void SetTrackInfo(AudioTrackInfo.TrackInfo info)
		{
			title.text = info.GetTrackDescribe();
			id = info.id;
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
			if (OnClickTrackItem != null)
			{
				OnClickTrackItem.Invoke(id);
			}
		}
	}

}

