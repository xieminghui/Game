using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace MediaPlayer
{
	public enum PlayType
	{
		None, Video, Picture
	}
	
	public enum DefinitionModel
	{
		UNKOWN,
		DEFINITION_STANDARD = 1,//标清
		DEFINITION_HIGH = 2,//高清
		DEFINITION_720P = 4,
		DEFINITION_1080P = 5,
		DEFINITION_4K =10,
		DEFINITION_LIVE_4K_N = 51,//在线4K流畅--自有视频频道
		DEFINITION_LIVE_4K_H = 52,//在线4K高清--自有视频频道
		DEFINITION_LIVE_4K_R = 53,//在线4K原画--自有视频频道
		DEFINITION_LIVE_8K_N = 61,//在线8K流畅--自有视频频道
		DEFINITION_LIVE_8K_H = 62,//在线8K高清--自有视频频道
		DEFINITION_LIVE_8K_R = 63,//在线8K原画--自有视频频道
	}
	
	public enum MediaUIButtons
	{
		LoopBtn,
		BackBtn,
		PlayBtn,
		RePlayBtn,
		PauseBtn,
		PreviousBtn,
		NextBtn,
		VolumeBtn,
		SettingBtn,
		FocusBtn,
		LockBtn,
		DramBtn,
		SeatBtn,
		
		//Settting
		VoiceSettingBtn,
		ScreenAdjustBtn,
		StereoTypeBtn,
		SceneChangeBtn,
		DefinitionBtn,
		SubtitleSettingBtn,
		Max
	}

	public interface IMediaPlayerUIData
	{
		LoopTypeEnum GetLoopType();

		bool IsInFocus();
			
		StereoType GetCurStereoType();
		
		bool GetBtnVisible(MediaUIButtons btn);

		bool GetBtnInteractable(MediaUIButtons btn);

		int GetTotalDrama();
		int GetCurDrama();
		int GetAviliableDrama();

		DefinitionModel GetCurDefinitionModel();
		List<int> GetAllDefinitionCode();
		
	}

	public class MediaPlayerUIEventBase : MonoBehaviour, IMediaPlayerUIData
	{
		public VideoControlPanel VideoControlPanel;
		public List<bool> BtnVisible = new List<bool>();
		public List<bool> BtnInteractable = new List<bool>();
		private int UIBtnVisibles = 0x0;
		protected virtual void Awake()
		{
			for (int i = 0; i < (int)MediaUIButtons.Max; i++)
			{
				BtnVisible.Add(true);
				BtnInteractable.Add(true);
			}

			VideoControlPanel.ClickBackBtnCallback += OnClickBackBtn;
			VideoControlPanel.ClickLoopBtnCallback += OnClickLoopBtn;
			VideoControlPanel.ClickPreviousBtnCallback += OnClickPreviousBtn;
			VideoControlPanel.ClickPlayBtnCallback += OnClickPlayBtn;
			VideoControlPanel.ClickRePlayBtnCallBack += OnClickRePlayBtn;
			VideoControlPanel.ClickPauseBtnCallback += OnClickPauseBtn;
			VideoControlPanel.ClickNextBtnCallback += OnClickNextBtn;
			VideoControlPanel.ClickFocusBtnCallback += OnClickFocusBtn;
			VideoControlPanel.ClickLockBtnCallback += OnClickLockBtn;
			
			VideoControlPanel.PlayPBPanel.SeekToTimeCallback += OnSeekToTime;
			VideoControlPanel.VolumePanel.ChangVolumePercentCallback += OnChangVolumePercent;

			VideoControlPanel.VideoDramaPanel.ChooseVideoDramaCallback += OnChooseVideoDrama;
				
			VideoControlPanel.SettingsPanel.ScreenAdjustPanel.ChangeScreenAdjustScaleTypeCallback += OnChangeScreenAdjustScaleType;
			VideoControlPanel.SettingsPanel.ScreenAdjustPanel.ChangeScreenAdjustSizeTypeCallback += OnChangeScreenAdjustSizeType;

			VideoControlPanel.SettingsPanel.VoiceSettingPanel.ChangeAudioTrackCallback += OnChangeAudioTrack;

			VideoControlPanel.SettingsPanel.SubtitleSettingPanel.ChangeSubtitleCallback += OnChangeSubtitle;

			VideoControlPanel.SettingsPanel.StereoTypePanel.Change3DEnableCallback += OnChange3DEnable;
			VideoControlPanel.SettingsPanel.StereoTypePanel.ChangeStereoTypeCallback += OnChangeStereoType;
			VideoControlPanel.SettingsPanel.StereoTypePanel.TensileSwitchCallback += OnTensileSwitch;

			VideoControlPanel.SettingsPanel.SceneChangePanel.ChangeSceneStyleCallback += OnChangeChangeSceneStyle;

			VideoControlPanel.SettingsPanel.DefinitionPanel.ChangeSkyVideoDefinitionCallback += OnChangeSkyVideoDefinition;
		}

		protected virtual void OnDestroy()
		{
			VideoControlPanel.ClickBackBtnCallback -= OnClickBackBtn;
			VideoControlPanel.ClickLoopBtnCallback -= OnClickLoopBtn;
			VideoControlPanel.ClickPreviousBtnCallback -= OnClickPreviousBtn;
			VideoControlPanel.ClickPlayBtnCallback -= OnClickPlayBtn;
			VideoControlPanel.ClickRePlayBtnCallBack -= OnClickRePlayBtn;
			VideoControlPanel.ClickPauseBtnCallback -= OnClickPauseBtn;
			VideoControlPanel.ClickNextBtnCallback -= OnClickNextBtn;
			VideoControlPanel.ClickFocusBtnCallback -= OnClickFocusBtn;
			VideoControlPanel.ClickLockBtnCallback -= OnClickLockBtn;
			
			VideoControlPanel.PlayPBPanel.SeekToTimeCallback -= OnSeekToTime;
			VideoControlPanel.VolumePanel.ChangVolumePercentCallback -= OnChangVolumePercent;
			
			VideoControlPanel.VideoDramaPanel.ChooseVideoDramaCallback -= OnChooseVideoDrama;
			
			VideoControlPanel.SettingsPanel.ScreenAdjustPanel.ChangeScreenAdjustScaleTypeCallback -= OnChangeScreenAdjustScaleType;
			VideoControlPanel.SettingsPanel.ScreenAdjustPanel.ChangeScreenAdjustSizeTypeCallback -= OnChangeScreenAdjustSizeType;

			VideoControlPanel.SettingsPanel.VoiceSettingPanel.ChangeAudioTrackCallback -= OnChangeAudioTrack;
			
			VideoControlPanel.SettingsPanel.SubtitleSettingPanel.ChangeSubtitleCallback -= OnChangeSubtitle;

			VideoControlPanel.SettingsPanel.StereoTypePanel.Change3DEnableCallback -= OnChange3DEnable;
			VideoControlPanel.SettingsPanel.StereoTypePanel.ChangeStereoTypeCallback -= OnChangeStereoType;
			VideoControlPanel.SettingsPanel.StereoTypePanel.TensileSwitchCallback -= OnTensileSwitch;

			VideoControlPanel.SettingsPanel.SceneChangePanel.ChangeSceneStyleCallback -= OnChangeChangeSceneStyle;
			
			VideoControlPanel.SettingsPanel.DefinitionPanel.ChangeSkyVideoDefinitionCallback -= OnChangeSkyVideoDefinition;
		}

		#region VideoControlPanel Event

		protected virtual void OnClickBackBtn()
		{
			
		}

		protected virtual void OnClickLoopBtn()
		{

		}

		protected virtual void OnClickPreviousBtn()
		{
			
		}

		protected virtual void OnClickPlayBtn()
		{
			
		}

		protected virtual void OnClickRePlayBtn()
		{
			
		}

		protected virtual void OnClickPauseBtn()
		{
			
		}

		protected virtual void OnClickNextBtn()
		{
			
		}

		protected virtual void OnClickFocusBtn()
		{

		}

		protected virtual void OnClickLockBtn()
		{
			
		}

		protected virtual void OnSeekToTime(long time)
		{
			
		}

		protected virtual void OnChangVolumePercent(float volume)
		{

		}

		#endregion

		#region ScreenAdjustPanel Event

		protected virtual void OnChangeScreenAdjustScaleType()
		{
		}

		protected virtual void OnChangeScreenAdjustSizeType()
		{
			
		}

		#endregion

		#region VoiceSettingPanel Event

		protected virtual void OnChangeAudioTrack(int id)
		{
			
		}

		protected virtual void OnChangeSubtitle(string path)
		{
			
		}


		#endregion

		#region StereoTypePanel Event

		protected virtual void OnChange3DEnable(bool enable)
		{
			
		}

		protected virtual void OnChangeStereoType(StereoType type)
		{
			
		}

		protected virtual void OnTensileSwitch(bool isOn)
		{
			
		}

		#endregion

		#region SceneChangePanel Event

		protected virtual void OnChangeChangeSceneStyle(SceneModel sceneModel)
		{
			
		}

		#endregion

		#region VideoDramaPanel Event
		protected virtual void OnChooseVideoDrama(int index)
		{
			
		}
		

		#endregion

		#region DefinitionPanel Event

		protected virtual void OnChangeSkyVideoDefinition(DefinitionModel definitionModel)
		{
			
		}

		#endregion

		#region UI Data

		public virtual LoopTypeEnum GetLoopType()
		{
			return LoopTypeEnum.PlayList;
		}

		public virtual bool IsInFocus()
		{
			return false;
		}


		public virtual StereoType GetCurStereoType()
		{
			return StereoType.ST2D;
		}

		public virtual bool LoopBtnVisible()
		{
			return false;
		}

		public virtual bool DramaBtnVisible()
		{
			return false;
		}

		public virtual bool SeatBtnVisible()
		{
			return false;
		}

		public virtual bool VoiceSettingBtnVisible()
		{
			return false;
		}

		public virtual bool DefinitionBtnVisible()
		{
			return false;
		}

		public virtual int GetTotalDrama()
		{
			return 0;
		}

		public virtual int GetCurDrama()
		{
			return 0;
		}

		public virtual int GetAviliableDrama()
		{
			return 0;
		}

		public virtual DefinitionModel GetCurDefinitionModel()
		{
			return DefinitionModel.UNKOWN;
		}

		public virtual List<int> GetAllDefinitionCode()
		{
			return null;
		}


		#endregion

		public void SetBtnVisible(MediaUIButtons btn, bool visible)
		{
			BtnVisible[(int)btn] = visible;

			switch (btn)
			{
				case MediaUIButtons.LoopBtn:
						VideoControlPanel.LoopBtn.gameObject.SetActive(visible); 
						break;
				case MediaUIButtons.BackBtn: 
						VideoControlPanel.BackBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.PlayBtn: 
						VideoControlPanel.PlayBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.RePlayBtn:
						VideoControlPanel.RePlayBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.PauseBtn: 
						VideoControlPanel.PauseBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.PreviousBtn: 
						VideoControlPanel.PreviousBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.NextBtn: 
						VideoControlPanel.NextBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.VolumeBtn: 
						VideoControlPanel.VolumeBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.SettingBtn: 
						VideoControlPanel.SettingBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.FocusBtn: 
						VideoControlPanel.FocusBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.LockBtn: 
						VideoControlPanel.LockBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.DramBtn: 
						VideoControlPanel.DramBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.SeatBtn: 
						VideoControlPanel.SeatBtn.gameObject.SetActive(visible);
						break;
		
				//Settting Panel
				case MediaUIButtons.VoiceSettingBtn:
						VideoControlPanel.SettingsPanel.VoiceSettingBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.ScreenAdjustBtn:
						VideoControlPanel.SettingsPanel.ScreenAdjustBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.StereoTypeBtn:						
						VideoControlPanel.SettingsPanel.StereoTypeBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.SceneChangeBtn:						
						VideoControlPanel.SettingsPanel.SceneChangeBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.DefinitionBtn:						
						VideoControlPanel.SettingsPanel.DefinitionBtn.gameObject.SetActive(visible);
						break;
				case MediaUIButtons.SubtitleSettingBtn:
					VideoControlPanel.SettingsPanel.SubtitleSettingBtn.gameObject.SetActive(visible);
					break;
			}
		}

		public bool GetBtnVisible(MediaUIButtons btn)
		{
			return BtnVisible[(int)btn];
		}

		public void SetBtnInteractable(MediaUIButtons btn, bool interactable)
		{
			BtnInteractable[(int)btn] = interactable;
			
			switch (btn)
			{
				case MediaUIButtons.LoopBtn:
						VideoControlPanel.LoopBtn.interactable = interactable; 
						break;
				case MediaUIButtons.BackBtn: 
						VideoControlPanel.BackBtn.interactable = interactable; 
						break;
				case MediaUIButtons.PlayBtn: 
						VideoControlPanel.PlayBtn.interactable = interactable; 
						break;
				case MediaUIButtons.PauseBtn: 
						VideoControlPanel.PauseBtn.interactable = interactable; 
						break;
				case MediaUIButtons.PreviousBtn: 
						VideoControlPanel.PreviousBtn.interactable = interactable; 
						break;
				case MediaUIButtons.NextBtn: 
						VideoControlPanel.NextBtn.interactable = interactable; 
						break;
				case MediaUIButtons.VolumeBtn: 
						VideoControlPanel.VolumeBtn.interactable = interactable; 
						break;
				case MediaUIButtons.SettingBtn: 
						VideoControlPanel.SettingBtn.interactable = interactable; 
						break;
				case MediaUIButtons.FocusBtn: 
						VideoControlPanel.FocusBtn.interactable = interactable; 
						break;
				case MediaUIButtons.LockBtn: 
						VideoControlPanel.LockBtn.interactable = interactable; 
						break;
				case MediaUIButtons.DramBtn: 
						VideoControlPanel.DramBtn.interactable = interactable; 
						break;
				case MediaUIButtons.SeatBtn: 
						VideoControlPanel.SeatBtn.interactable = interactable; 
						break;
		
				//Settting Panel
				case MediaUIButtons.VoiceSettingBtn:
						VideoControlPanel.SettingsPanel.VoiceSettingBtn.interactable = interactable; 
						break;
				case MediaUIButtons.ScreenAdjustBtn:
						VideoControlPanel.SettingsPanel.ScreenAdjustBtn.interactable = interactable; 
						break;
				case MediaUIButtons.StereoTypeBtn:						
						VideoControlPanel.SettingsPanel.StereoTypeBtn.interactable = interactable; 
						break;
				case MediaUIButtons.SceneChangeBtn:						
						VideoControlPanel.SettingsPanel.SceneChangeBtn.interactable = interactable; 
						break;
				case MediaUIButtons.DefinitionBtn:						
						VideoControlPanel.SettingsPanel.DefinitionBtn.interactable = interactable; 
						break;
				case MediaUIButtons.SubtitleSettingBtn:
					VideoControlPanel.SettingsPanel.SubtitleSettingBtn.interactable = interactable;
					break;
			}
		}

		public bool GetBtnInteractable(MediaUIButtons btn)
		{
			return BtnInteractable[(int)btn];
		}
	}
	
	public class MediaPlayerGloabVariable
	{
		#region 播放器缩放信息
		public static float NormalScreenSizeSliderMin = 100f;
		public static float NormalScreenSizeSliderMax = 140f;
		public static float IMAXTheaterScreenSizeSliderMin = 100f;
		public static float IMAXTheaterScreenSizeSliderMax = 140f;
		public static float IMAXTheaterScreenSizeRate = 140f;
		public static float NormalScreenSizeRate = 120f;
		#endregion
		
		public static string SCENE_MODEL_KEY = "SCENE_MODEL_KEY";
		public static string SCREEN_SIZE_TYPE_KEY = "SCREEN_SIZE_TYPE_KEY";
		public static string SCENE_DRIVE_MODEL_KEY = "SCENE_DRIVE_MODEL_KEY";
		
		public static string SCREEN_SCALE_TYPE_KEY = "SCREEN_SCALE_TYPE_KEY";
		public static string SCREEN_SIZE_RATE_KEY = "SCREEN_SIZE_RATE_KEY";
		
		
		public static PlayType CurPlayType = PlayType.None;
		
		public static void Set3DEnable(bool enable)
		{
			if (enable)
			{
				PlayerPrefs.SetInt("3D_Enable", 1);
			}
			else
			{
				PlayerPrefs.SetInt("3D_Enable", 0);
			}
			PlayerPrefs.Save();
			PlayerPrefs.GetInt("3D_Enable", 1);
		}
    
		public static bool Get3DEnable()
		{
			return PlayerPrefs.GetInt("3D_Enable", 1) == 1;
		}

		public static void SetSceneModel(SceneModel model)
		{
			if (CurPlayType == PlayType.Video)
				PlayerPrefs.SetInt(SCENE_MODEL_KEY, (int)model);
		}

		public static SceneModel GetSceneModel()
		{
			if (CurPlayType == PlayType.Video)
			{
				if (PlayerPrefs.HasKey(SCENE_MODEL_KEY))
					return (SceneModel)PlayerPrefs.GetInt(SCENE_MODEL_KEY);
				else
				{
					SetSceneModel(SceneModel.IMAXTheater);
					return SceneModel.IMAXTheater;
				}
			}
			else
				return SceneModel.Default;
		}

		public static void SetDriveSceneModel(DriveSceneModel model)
		{
			PlayerPrefs.SetInt(SCENE_DRIVE_MODEL_KEY, (int)model);
		}

		public static DriveSceneModel GetDriveSceneModel()
		{
			if (PlayerPrefs.HasKey(SCENE_DRIVE_MODEL_KEY))
				return (DriveSceneModel)PlayerPrefs.GetInt(SCENE_DRIVE_MODEL_KEY);
			else
			{
				SetDriveSceneModel(DriveSceneModel.Playboy);
				return DriveSceneModel.Playboy;
			}
		}

		public static void SetScreenSizeType(ScreenSizeType type)
		{
			PlayerPrefs.SetInt(SCREEN_SIZE_TYPE_KEY, (int)type);
		}

		public static ScreenSizeType GetScreenSizeType()
		{
			if (PlayerPrefs.HasKey(SCREEN_SIZE_TYPE_KEY))
				return (ScreenSizeType)PlayerPrefs.GetInt(SCREEN_SIZE_TYPE_KEY);
			else
			{
				SetScreenSizeType(ScreenSizeType.Standard);
				return ScreenSizeType.Standard;
			}
		}
		
		public static void SetScreenScaleType(ScreenScaleType type)
		{
			PlayerPrefs.SetInt(SCREEN_SCALE_TYPE_KEY, (int)type);
		}

		public static ScreenScaleType GetScreenScaleType()
		{
			if (PlayerPrefs.HasKey(SCREEN_SCALE_TYPE_KEY))
				return (ScreenScaleType)PlayerPrefs.GetInt(SCREEN_SCALE_TYPE_KEY);
			else
			{
				SetScreenScaleType(ScreenScaleType.S_Nor);
				return ScreenScaleType.S_Nor;
			}
		}
		
		public static void SetScreenSizeRate(float value)
		{
			PlayerPrefs.SetFloat(SCREEN_SIZE_RATE_KEY, value);
		}

		public static float GetScreenSizeRate()
		{
			if (PlayerPrefs.HasKey(SCREEN_SIZE_RATE_KEY))
				return PlayerPrefs.GetFloat(SCREEN_SIZE_RATE_KEY);
			else
			{
				SetScreenSizeRate(IMAXTheaterScreenSizeRate);
				return IMAXTheaterScreenSizeRate;
			}
		}
	}


}


