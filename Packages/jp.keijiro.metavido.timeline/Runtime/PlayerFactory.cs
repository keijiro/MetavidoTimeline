using UnityEngine;
using Klak.Hap;

namespace Metavido.Timeline {

public static class PlayerFactory
{
    public static HapPlayer Create
      (string filePath, HapPlayer.PathMode pathMode)
    {
        var go = new GameObject("[Not Saved] HAP Player for Metavido");
        go.hideFlags = HideFlags.DontSave;

        var hap = go.AddComponent<HapPlayer>();
        hap.Open(filePath, pathMode);
        hap.targetTexture = RenderTexture.GetTemporary(1920, 1080);
        hap.OnControlTimeStart();

        return hap;
    }

    public static void Destroy(HapPlayer instance)
    {
        if (instance == null) return;

        RenderTexture.ReleaseTemporary(instance.targetTexture);

        if (Application.isPlaying)
            Object.Destroy(instance.gameObject);
        else
            Object.DestroyImmediate(instance.gameObject);
    }
}

} // namespace Metavido.Timeline
