
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MediaPlayer
{
    public class VideoSubtitleDictionaryDetector : SingletonPure<VideoSubtitleDictionaryDetector>
    {
        private static string PLAYERKEYWORD = "VIDEO_SUBTITLE_DICTIONARY";
        Dictionary<int, string> VideoSubtitleDic;

        public VideoSubtitleDictionaryDetector()
        {
            if (PlayerPrefs.HasKey(PLAYERKEYWORD) && 
                PlayerPrefs.GetString(PLAYERKEYWORD) != null && PlayerPrefs.GetString(PLAYERKEYWORD) != "")
            {
                string videoSubtitleDicString = PlayerPrefs.GetString(PLAYERKEYWORD);
                VideoSubtitleDic = StringToDictionary(videoSubtitleDicString);
            }
            else
                VideoSubtitleDic = new Dictionary<int, string>();
        }

        public void SaveVideoFormatTypeDic()
        {
            if (VideoSubtitleDic.Count == 0)
                return;

            string videoFormatDicString = DictionaryListToString(VideoSubtitleDic);

            if (videoFormatDicString != "" && videoFormatDicString != null)
                PlayerPrefs.SetString(PLAYERKEYWORD, videoFormatDicString);
        }

        public bool HasVideoFormatOrNotByVideoId(int videoId)
        {
            if (VideoSubtitleDic.ContainsKey(videoId))
                return true;
            else
                return false;
        }

        public void SetVideoFormatTypeByVideoId(int videoId, string subtitle)
        {
            if (HasVideoFormatOrNotByVideoId(videoId))
                VideoSubtitleDic[videoId] = subtitle;
            else
                VideoSubtitleDic.Add(videoId, subtitle);

            SaveVideoFormatTypeDic();
        }

        public string GetVideoFormatTypeByVideoId(int videoId)
        {
            if (VideoSubtitleDic.ContainsKey(videoId))
                return VideoSubtitleDic[videoId];
            else
                return null;
        }

        public void DeleteFormatKeyByVideoId(int videoId)
        {
            if (VideoSubtitleDic.ContainsKey(videoId))
                VideoSubtitleDic.Remove(videoId);
        }

        Dictionary<int, string> StringToDictionary(string value)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            if (value.Length < 1)
                return dic;
            string[] dicStrs = value.Split('|');
            foreach (string str in dicStrs)
            {
                string[] strs = str.Split('*');
                dic.Add(int.Parse(strs[0]), strs[1]);
            }
            return dic;
        }

        string DictionaryListToString(Dictionary<int, string> dicInfo)
        {
            if (dicInfo.Count == 0)
            {
                return "";
            }
            string str = "";

            foreach (int key in dicInfo.Keys)
            {
                str += (key + "*" + dicInfo[key]);
                str += "|";
            }
            str = str.Substring(0, str.Length - 1);

            return str;
        }
    }
}

