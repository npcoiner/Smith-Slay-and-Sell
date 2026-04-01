using UnityEngine;
public static class SFX
{
    public static void Play(AudioClip clip, Vector3 pos, float pitch = 1f, float vol = 1f)
    {
        GameObject temp = new GameObject("TempSFX");
        temp.transform.position = pos;
        AudioSource a = temp.AddComponent<AudioSource>();
        a.clip = clip;
        a.pitch = pitch;
        a.volume = vol;
        a.Play();
        Object.Destroy(temp, clip.length);
    }
}
