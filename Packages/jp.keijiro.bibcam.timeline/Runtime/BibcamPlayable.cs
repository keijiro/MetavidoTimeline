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
    (Vector3 p, Quaternion r) _sample1, _sample2;
    float _hash, _param;

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
        // Scene objects
        var camera = playerData as Camera;
        var decoder = camera.GetComponent<BibcamMetadataDecoder>();
        var demuxer = camera.GetComponent<BibcamTextureDemuxer>();
        if (camera == null || decoder == null || demuxer == null) return;
        var xform = camera.transform;

        // Bibcam decoding
        decoder.Decode(_source.targetTexture);
        demuxer.Demux(_source.targetTexture, decoder.Metadata);

        // Metadata validity check
        var meta = decoder.Metadata;
        if (!meta.IsValid) return;

        // Keyframe detection
        if (_hash != meta.Hash)
        {
            // Interpolation parameter reset
            // Disable interpolation when:
            // 1) it's the initial frame,
            // 2) or the timeline is not playing.
            _param = _hash != 0 && playable.GetGraph().IsPlaying() ? 0 : 1;

            // Sample update
            _sample1.p = xform.localPosition;
            _sample1.r = xform.localRotation;
            _sample2.p = meta.CameraPosition;
            _sample2.r = meta.CameraRotation;
            _hash = meta.Hash;

            // Projection matrix update (no interpolation)
            camera.ResetProjectionMatrix();
            camera.projectionMatrix =
                meta.ReconstructProjectionMatrix(camera.projectionMatrix);
        }

        // Frame duration
        var dur = (float)(_source.streamDuration / _source.frameCount);
        dur /= info.effectiveSpeed;

        // Parameter increment
        _param = Mathf.Clamp01(_param + info.deltaTime / dur);

        // Position/rotation update with interpolation
        xform.localPosition = Vector3.Lerp(_sample1.p, _sample2.p, _param);
        xform.localRotation = Quaternion.Slerp(_sample1.r, _sample2.r, _param);
    }

    public override void OnPlayableDestroy(Playable playable)
      => PlayerFactory.Destroy(_source);

    #endregion
}

} // namespace Bibcam.Timeline
