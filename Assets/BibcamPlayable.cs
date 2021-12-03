using UnityEngine;
using UnityEngine.Playables;
using Bibcam.Decoder;
using Klak.Hap;

namespace Bibcam.Timeline {

public class BibcamPlayable : PlayableBehaviour
{
    #region Public members

    public HapPlayer Source { get; set; }

    #endregion

    #region PlayableBehaviour overrides

    public override void PrepareFrame
      (Playable playable, FrameData info)
    {
        Source.SetTime(playable.GetTime());
        Source.UpdateNow();
    }

    public override void ProcessFrame
      (Playable playable, FrameData info, object playerData)
    {
        var camera = playerData as Camera;
        if (camera == null) return;

        var decoder = camera.GetComponent<BibcamMetadataDecoder>();
        var demuxer = camera.GetComponent<BibcamTextureDemuxer>();

        decoder.Decode(Source.targetTexture);
        demuxer.Demux(Source.targetTexture, decoder.Metadata);

        var meta = decoder.Metadata;
        if (meta.IsValid)
        {
            camera.transform.localPosition = meta.CameraPosition;
            camera.transform.localRotation = meta.CameraRotation;
            camera.projectionMatrix =
              meta.ReconstructProjectionMatrix(camera.projectionMatrix);
        }
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        if (Source == null) return;

        RenderTexture.ReleaseTemporary(Source.targetTexture);

        if (Application.isPlaying)
            Object.Destroy(Source.gameObject);
        else
            Object.DestroyImmediate(Source.gameObject);
    }

    #endregion
}

} // namespace Bibcam.Timeline
