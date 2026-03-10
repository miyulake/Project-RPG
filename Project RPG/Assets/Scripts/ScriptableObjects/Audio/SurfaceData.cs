using UnityEngine;

[CreateAssetMenu(fileName = "SurfaceData", menuName = "Miyu/Audio/SurfaceData")]
public class SurfaceData : ScriptableObject
{
    [SerializeField] private Surface[] surfaces;

    public AudioClip GetRandomHit(SurfaceType surface)
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            if (surfaces[i].surface == surface && surfaces[i].hitAudio.Length > 0)
                return surfaces[i].hitAudio[Random.Range(0, surfaces[i].hitAudio.Length)];
        }
        return null;
    }

    public AudioClip GetRandomStep(SurfaceType surface)
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            if (surfaces[i].surface == surface && surfaces[i].stepAudio.Length > 0)
                return surfaces[i].stepAudio[Random.Range(0, surfaces[i].stepAudio.Length)];
        }
        return null;
    }
}