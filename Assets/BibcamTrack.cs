using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Bibcam.Decoder;

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

        var decoder = camera.GetComponent<BibcamMetadataDecoder>();
        var demuxer = camera.GetComponent<BibcamTextureDemuxer>();
        if (decoder == null || demuxer == null)
            Debug.LogWarning("Bibcam Track requires Metadata Decoder and" +
                             " Texture Demuxer attached to the camera.");
    }

    #endregion
}

} // namespace Bibcam.Timeline
