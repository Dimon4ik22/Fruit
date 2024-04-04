using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Sound
{
    public string ID;
    public AudioClip clip;
    [Range(0, 1f)] public float volume = 1f;
    public bool loop;
}
public class SoundSystem : Singleton<SoundSystem>
{
    [SerializeField] private List<Sound> sounds = new List<Sound>();
    private List<AudioPlayer> createdSounds = new List<AudioPlayer>();

    public void SetAudio(string Id, bool play)
    {
        if (play) PlayAudio(Id);
        else StopAudio(Id, false);
    }
    private void PlayAudio(string Id)
    {
        for (int i = 0; i < createdSounds.Count; i++)
        {
            if (string.Equals(createdSounds[i].sound.ID, Id))
            {
                StopAudio(Id, true);

                return;
            }
        }
        for (int i = 0; i < sounds.Count; i++)
        {

            if (string.Equals(sounds[i].ID, Id))
            {
                SetAudioPlayer(sounds[i]);

                int listOrder = createdSounds.Count - 1;
                createdSounds[listOrder].audioSource.Play();
                return;
            }
        }
    }

    private void StopAudio(string Id, bool playAgain)
    {
        for (int i = 0; i < createdSounds.Count; i++)
        {
            if (string.Equals(createdSounds[i].sound.ID, Id))
            {
                DOTween.Complete("VolumeDown");
                float firstVolume = createdSounds[i].audioSource.volume;
                float lerpingVolume = firstVolume;
                DOTween.To(() => lerpingVolume, x => lerpingVolume = x, 0, 10f).SetSpeedBased().SetId("VolumeDown").OnUpdate(() =>
                {
                    createdSounds[i].audioSource.volume = lerpingVolume;
                }).OnComplete(() =>
                {
                    createdSounds[i].audioSource.volume = firstVolume;
                    createdSounds[i].audioSource.Stop();
                    if(playAgain) createdSounds[i].audioSource.Play();
                });

                return;
            }
        }
    }

    private void SetAudioPlayer(Sound _sound)
    {
        GameObject go = new GameObject();
        go.name = "AudioPlayer";
        go.transform.position = Vector3.zero;
        go.transform.SetParent(transform);
        AudioPlayer audioPlayer = go.AddComponent<AudioPlayer>();

        audioPlayer.sound = _sound;

        audioPlayer.audioSource.clip = _sound.clip;
        audioPlayer.audioSource.volume = _sound.volume;
        audioPlayer.audioSource.loop = _sound.loop;

        createdSounds.Add(audioPlayer);
    }
}
