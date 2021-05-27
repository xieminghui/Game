using System.Collections.Generic;
using SimpleJSON;

namespace MediaPlayer
{
    public class AudioTrackInfo
    {
        public class TrackInfo
        {
            public int id;
            public string language;
            public string title;

            public TrackInfo(int id, string language, string title)
            {
                this.id = id;
                this.language = language;
                this.title = title;
            }

            public static TrackInfo fromJsonNode(JSONNode node)
            {
                return new TrackInfo(node["id"].AsInt, node["language"].Value, node["title"].Value);
            }

            public string GetTrackDescribe()
            {
                if (!string.IsNullOrEmpty(title))
                {
                    return title;
                }
                else if (!string.IsNullOrEmpty(language))
                {
                    return language;
                }

                return "" + id;
            }
        }

        public int selectIndex = 0;
        public List<TrackInfo> trackInfos;

        public AudioTrackInfo(int selectIndex, List<TrackInfo> trackInfos)
        {
            this.selectIndex = selectIndex;
            this.trackInfos = trackInfos;
        }


        public static AudioTrackInfo fromJsonStr(string json)
        {
            JSONNode jsonNode = JSON.Parse(json);
            if (jsonNode.Count < 2)
            {
                return null;
            }

            List<TrackInfo> trackInfos= new List<TrackInfo>();
            foreach (var node in jsonNode[1].Childs)
            {
                trackInfos.Add(TrackInfo.fromJsonNode(node));
            }
        
            AudioTrackInfo info = new AudioTrackInfo(jsonNode[0].AsInt, trackInfos);
            return info;
        }
    }
    
}

