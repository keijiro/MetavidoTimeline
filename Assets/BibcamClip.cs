using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Klak.Hap;

namespace Bibcam.Timeline {

[System.Serializable]
public class BibcamClip : PlayableAsset, ITimelineClipAsset
{
    #region Editable attributes

    public HapPlayer.PathMode pathMode;
    public string filePath;

    #endregion

    #region ITimelineClipAsset implementation

    public ClipCaps clipCaps => ClipCaps.ClipIn | ClipCaps.SpeedMultiplier;

    #endregion

    #region PlayableAsset overrides

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
       var playable = ScriptPlayable<BibcamPlayable>.Create(graph);
       playable.GetBehaviour().Source = CreateSourcePlayer();
       return playable;
    }

    #endregion

    HapPlayer CreateSourcePlayer()
    {
        var go = new GameObject() { hideFlags = HideFlags.DontSave };

        var hap = go.AddComponent<HapPlayer>();
        hap.Open(filePath, pathMode);
        hap.targetTexture = RenderTexture.GetTemporary(1920, 1080);
        hap.OnControlTimeStart();

        return hap;
    }
}

} // namespace Bibcam.Timeline
