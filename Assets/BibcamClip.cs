using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Klak.Hap;

namespace Bibcam.Timeline {

[System.Serializable]
public class BibcamClip : PlayableAsset, ITimelineClipAsset
{
    #region Editable attributes

    public string filePath = "";
    public HapPlayer.PathMode pathMode;

    #endregion

    #region ITimelineClipAsset implementation

    public ClipCaps clipCaps => ClipCaps.ClipIn | ClipCaps.SpeedMultiplier;

    #endregion

    #region PlayableAsset overrides

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
      => BibcamPlayable.CreatePlayable(graph, filePath, pathMode);

    #endregion
}

} // namespace Bibcam.Timeline
