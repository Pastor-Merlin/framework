using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class SoundManager : Manager
    {
        private AudioSource audioSource;
        private Hashtable sounds = new Hashtable();

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// ���һ������
        /// </summary>
        void Add(string key, AudioClip value)
        {
            if (sounds[key] != null || value == null) return;
            sounds.Add(key, value);
        }

        /// <summary>
        /// ��ȡһ������
        /// </summary>
        AudioClip Get(string key)
        {
            if (sounds[key] == null) return null;
            return sounds[key] as AudioClip;
        }

        /// <summary>
        /// ����һ����Ƶ
        /// </summary>
        public AudioClip LoadAudioClip(string path, string ext)
        {
            AudioClip ac = Get(path);
            if (ac == null)
            {
                ac = ResourcesManager.Instance.DownAsset("Sound", path, ext, typeof(AudioClip), true) as AudioClip;
                Add(path, ac);
            }
            return ac;
        }

        /// <summary>
        /// ���ű�������
        /// </summary>
        /// <param name="canPlay"></param>
        public void PlayBacksound(string name, bool canPlay, string ext)
        {
            if (audioSource.clip != null)
            {
                if (name.IndexOf(audioSource.clip.name) > -1)
                {
                    if (!canPlay)
                    {
                        audioSource.Stop();
                        audioSource.clip = null;
                        Util.ClearMemory();
                    }
                    return;
                }
            }
            if (canPlay)
            {
                audioSource.loop = true;
                audioSource.clip = LoadAudioClip(name, ext);
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
                audioSource.clip = null;
                Util.ClearMemory();
            }
        }

        /// <summary>
        /// ������Ƶ����
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="position"></param>
        public void Play(AudioClip clip, Vector3 position)
        {
            if (position == Vector3.zero)
                position = Camera.main.transform.position;

            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}