using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Bibcam.Timeline {

[TrackColor(0.1f, 0.6f, 0.5f)]
[TrackClipType(typeof(BibcamClip))]
[TrackBindingType(typeof(Camera))]
public class BibcamTrack : TrackAsset
{
    #region TrackAsset overrides

    public override void GatherProperties
      (PlayableDirector director, IPropertyCollector driver)
    {
        var camera = director.GetGenericBinding(this) as Camera;
        if (camera == null) return;
        driver.AddFromName(camera.transform, "m_LocalPosition");
        driver.AddFromName(camera.transform, "m_LocalRotation");
    }

    #endregion
}

} // namespace Bibcam.Timeline
