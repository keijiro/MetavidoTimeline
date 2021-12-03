using UnityEngine;
using UnityEngine.Playables;
using Klak.Hap;
using Bibcam.Decoder;

namespace Bibcam.Timeline {

public class BibcamPlayable : PlayableBehaviour
{
    #region Public members

    public static Playable CreatePlayable
      (PlayableGraph graph, string filePath, HapPlayer.PathMode pathMode)
    {
        var playable = ScriptPlayable<BibcamPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour._source = PlayerFactory.Create(filePath, pathMode);
        return playable;
    }

    #endregion

    #region Private members

    HapPlayer _source;

    #endregion

    #region PlayableBehaviour overrides

    public override void OnBehaviourPause(Playable playable, FrameData info)
      => _source.gameObject.SetActive(false);

    public override void OnBehaviourPlay(Playable playable, FrameData info)
      => _source.gameObject.SetActive(true);

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        _source.SetTime(playable.GetTime());
        _source.UpdateNow();
    }

    public override void ProcessFrame
      (Playable playable, FrameData info, object playerData)
    {
        var camera = playerData as Camera;
        var decoder = camera.GetComponent<BibcamMetadataDecoder>();
        var demuxer = camera.GetComponent<BibcamTextureDemuxer>();
        if (camera == null || decoder == null || demuxer == null) return;

        decoder.Decode(_source.targetTexture);
        demuxer.Demux(_source.targetTexture, decoder.Metadata);

        var meta = decoder.Metadata;
        if (!meta.IsValid) return;

        camera.transform.localPosition = meta.CameraPosition;
        camera.transform.localRotation = meta.CameraRotation;
        camera.projectionMatrix =
          meta.ReconstructProjectionMatrix(camera.projectionMatrix);
    }

    public override void OnPlayableDestroy(Playable playable)
      => PlayerFactory.Destroy(_source);

    #endregion
}

} // namespace Bibcam.Timeline
