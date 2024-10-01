using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] Sounds2D;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        foreach (Sound sound in Sounds2D)
            CreateSound(sound);

        PlaySound("cctv");
        PlaySound("Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSound(Sound sound)
    {
        AudioSource source = sound.parent.gameObject.AddComponent<AudioSource>();
        source.clip = sound.clip;
        source.volume = sound.volume;
        sound.source = source;
        source.loop = sound.loop;
        source.spatialBlend = sound.spatialBlend;
        if (source.spatialBlend > 0)
        {
            source.minDistance = sound.minDistance;
            source.maxDistance = sound.maxDistance;
        }

    }

    public void PlaySound(string name)
    {
        foreach(Sound sound in Sounds2D)
        {
            if (sound.name == name)
                sound.source.Play();
        }
    }

    public void StopSoundAll()
    {
        foreach (Sound sound in Sounds2D)
            sound.source.Stop();
    }

    public AudioSource GetSource(string name)
    {
        foreach (Sound sound in Sounds2D)
        {
            if (sound.name == name)
                return sound.source;
        }

        return null;
    }

    public void StopSound(string name)
    {

        foreach (Sound sound in Sounds2D)
        {
            if (sound.name == name)
                sound.source.Stop();
        }
    }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public Transform parent;
        public float volume;
        public bool loop;
        public AudioClip clip;
        public AudioSource source;
        public float spatialBlend;
        public float minDistance;
        public float maxDistance;
    }
}
