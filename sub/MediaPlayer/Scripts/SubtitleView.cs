using System;
using System.Collections;
using System.Collections.Generic;
using Svr.Keyboard;
using UnityEngine;
using UnityEngine.UI;

namespace MediaPlayer
{
	public class SubtitleView : MonoBehaviour {

		[SerializeField]
		private SvrVideoPlayer mSvrVideoPlayer;
		private SubtitleApi mSubtitleApi;
		private GLTexture mOriginTexture;

		private void Awake()
		{
			mSubtitleApi = new SubtitleApi();
			mSubtitleApi.OnTextureCreated = OnTextureCreated;
		}

		private void Update()
		{

		}

		public void ShowSubtitleView(string uri, int width, int height)
		{
			if (mSubtitleApi == null)
			{
				Debug.LogError("mSubtitleApi == null");
				mSubtitleApi = new SubtitleApi();
				mSubtitleApi.OnTextureCreated = OnTextureCreated;
			}

			mSubtitleApi.Show(uri, width, height);
		}

		private void OnTextureCreated(GLTexture glTexture)
		{
			Debug.LogError("OnTextureCreated " + glTexture.Width + "   " + glTexture.Hight);
			Texture2D texture = Texture2D.CreateExternalTexture(glTexture.Width, glTexture.Hight, TextureFormat.ARGB32, false, false, new IntPtr(glTexture.TextureID));
			mOriginTexture = glTexture;
		}
	}
}

