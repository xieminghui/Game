using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Svr.Keyboard;

namespace MediaPlayer
{
    public class SubtitleApi
    {
        private static AndroidJavaObject mJavaObject;

        private GLTexture mSubTitleTexture;
        private Queue<Action> mMainThreadQueue = new Queue<Action>();
        public Action<GLTexture> OnTextureCreated;
        private OpenGLRenderApi mOpenGLRenderApi;
        private AndroidJavaObject mApplication;

        private SubtitleListener mSubtitleListener;
        private BindResult mBindResult;
        
        private int mSubtitleWidth;
        private int mSubtitleHight;
        public SubtitleApi()
        {
            mSubTitleTexture = new GLTexture();
            mOpenGLRenderApi = new OpenGLRenderApi();
            
            OpenGLRenderApi.CreatTextureAction += OpenGLRenderApi_CreatTextureAction;
            OpenGLRenderApi.UpdateTextureAction += OpenGLRenderApi_UpdateTextureAction;
            OpenGLRenderApi.ReleaseTextureAction += OpenGLRenderApi_ReleaseTextureAction;
            
            mJavaObject = new AndroidJavaObject("com.ssnwt.vr.playermanager.SubtitleApi");
            AndroidJavaClass unitypalyer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unitypalyer.GetStatic<AndroidJavaObject>("currentActivity");
            mApplication = activity.Call<AndroidJavaObject>("getApplication");

            mSubtitleListener = new SubtitleListener(this);
            mBindResult = new BindResult(this);
            
            mJavaObject.Call("setSubtitleListener", mSubtitleListener);
            Connect();
        }

        private void OpenGLRenderApi_ReleaseTextureAction(int id, int textureid)
        {
            if (id != mSubTitleTexture.ID) return;
            mSubTitleTexture.glRelease();
            Debug.LogError("OpenGLRenderApi_ReleaseTextureAction");
        }
        private void OpenGLRenderApi_UpdateTextureAction(int id, int textureid)
        {
            if (id != mSubTitleTexture.ID) return;
            mSubTitleTexture.glUpdateTexImage();
        }

        private void OpenGLRenderApi_CreatTextureAction(int id, int textureid)
        {
            if (id != mSubTitleTexture.ID) return;
            mSubTitleTexture.glCreateSurface(mSubtitleWidth, mSubtitleHight);
            mJavaObject.Call("setSurface", mSubTitleTexture.getSurface(),mSubtitleWidth, mSubtitleHight);
            mMainThreadQueue.Enqueue(() =>
            {
                if (OnTextureCreated != null) OnTextureCreated(mSubTitleTexture);
            });
        }
        private void UpdateMainThreadMessage()
        {
            if (mMainThreadQueue.Count > 0)
            {
                mMainThreadQueue.Dequeue()();
            }
    
        }
        
        public void Connect()
        {
            if (!IsConnected())
                mJavaObject.Call("connect", mApplication, false, mBindResult);
        }

        public void Update()
        {
            UpdateMainThreadMessage();
            if (mSubTitleTexture != null && mSubTitleTexture.TextureID != 0)
                OpenGLRenderApi.IssuePluginEvent(mSubTitleTexture.ID, (int)OpenGLRenderApi.OpenGLOption.UpdateTexture, 0);
        }
        
        public void Disconnect()
        {
            mJavaObject.Call("disconnect", mApplication);
        }
        
        public bool IsConnected()
        {
            return mJavaObject.Call<bool>("isConnected");
        }
        
        public void Show(string uri, int subtitleWidth, int subtitleHight)
        {
            Debug.LogError("ShowSubtitle " + uri);
            mSubtitleWidth = subtitleWidth;
            mSubtitleHight = subtitleHight;
            
            mJavaObject.Call("show", uri);
        }
        
        public void Hide()
        {
            Debug.LogError("Hide ");
            mJavaObject.Call("hide");
            if (mSubTitleTexture != null)
            {
                Debug.LogError("ReleaseTexture ");
                OpenGLRenderApi.IssuePluginEvent(mSubTitleTexture.ID, (int) OpenGLRenderApi.OpenGLOption.ReleaseTexture,
                    0);
            }
        }

        public void Release()
        {
            Disconnect();
            if (mSubTitleTexture.TextureID != 0)
                OpenGLRenderApi.IssuePluginEvent(mSubTitleTexture.ID, (int)OpenGLRenderApi.OpenGLOption.ReleaseTexture, 0);
    
            OpenGLRenderApi.CreatTextureAction -= OpenGLRenderApi_CreatTextureAction;
            OpenGLRenderApi.UpdateTextureAction -= OpenGLRenderApi_UpdateTextureAction;
            OpenGLRenderApi.ReleaseTextureAction -= OpenGLRenderApi_ReleaseTextureAction;
        }
        
        private sealed class SubtitleListener : AndroidJavaProxy
        {
            private SubtitleApi SubtitleApi;
            public SubtitleListener(SubtitleApi subtitleApi) : base("com.ssnwt.vr.playermanager.SubtitleApi$SubtitleListener")
            {
                SubtitleApi = subtitleApi;
            }
            
            public void onHideSubtitle()
            {
                SubtitleApi.mMainThreadQueue.Enqueue(() =>
                {
                    
                });
            }
            
            public void onSubtitleReady(String uri)
            {
                SubtitleApi.mMainThreadQueue.Enqueue(() =>
                {
                    OpenGLRenderApi.IssuePluginEvent(SubtitleApi.mSubTitleTexture.ID, (int)OpenGLRenderApi.OpenGLOption.CreatTexture, 0);
                });
            }
            
        }
        
        private sealed class BindResult : AndroidJavaProxy
        {
            private SubtitleApi SubtitleApi;
            public BindResult(SubtitleApi subtitleApi) : base("com.ssnwt.vr.playermanager.SubtitleApi$BindResult")
            {
                SubtitleApi = subtitleApi;
            }
            public void onConnected()
            {
                SubtitleApi.mMainThreadQueue.Enqueue(() =>
                {

                });
            }
            public void onDisconnected()
            {
                SubtitleApi.mMainThreadQueue.Enqueue(() =>
                {

                });
            }
            public void onError()
            {
                SubtitleApi.mMainThreadQueue.Enqueue(() =>
                {

                });
            }
        }
    }
}
