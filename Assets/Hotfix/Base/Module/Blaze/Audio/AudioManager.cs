//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/03/18 | Initialize core skeleton |
| 1.1 | Eric   | 2020/03/19 | Extend sound manager     |
| 2.0 | ALYR   | 2021/02/20 | 简化声音管理，减少不不必要的对象开销     |
*/

using System;
using System.Collections.Generic;
using Blaze.Common;
using Blaze.Core;
using Blaze.Manage.Csv.Poco;
using Blaze.Resource;
using Blaze.Resource.Asset;
using Blaze.Resource.Common;
using Blaze.Utility;
using DG.Tweening;
using ETModel;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Hotfix.Base.Module.Blaze.Audio
{
    public sealed class AudioManager : Singeton<AudioManager>
    {
        private GameObject _audioNode;

        // 是否开启音乐
        private bool _musicIsOn = true;
        private float _musicVol = 1.0f;

        // 是否开启音效
        private bool _soundIsOn = true;
        private float _soundVol = 0.5f;

        // 声音片段缓存
        private readonly Dictionary<string, AudioClip> _audioMap = new Dictionary<string, AudioClip>();

        // Audio sources which are playing.
        private readonly List<AudioSource> _audioList = new List<AudioSource>();

        // Pool of sound source.
        private readonly Queue<AudioSource> _audioPool = new Queue<AudioSource>();

        private AudioSource _musicSource; //背景音乐
        private AudioSource _deputyMusicSource; //副背景音乐
        private AudioSource _dubbSource; //配音
        private string _curPlayMusicName;
        private string _curPlayDeputyMusicName;

        private AudioMixer soundMixer;
        private Dictionary<string, AudioMixerGroup> _mixerGroups = new Dictionary<string, AudioMixerGroup>();

        enum MixerMod
        {
            Dubbing,
            Music,
            Sound,
            DeputyMusic
        }

        private void SnapSwitch(bool toDubb = false)
        {
            if (toDubb)
                soundMixer?.FindSnapshot("snap_dubbing")?.TransitionTo(1f);
            else
                soundMixer?.FindSnapshot("snap_default")?.TransitionTo(1f);
        }

        public void Initialize()
        {
            Object.DontDestroyOnLoad(_audioNode = new GameObject("SoundSystem"));

            var mixObj = Res.InstantiateSync("Assets/Projects/Audio/Mixer/SoundMixer.prefab").Target;
            soundMixer = mixObj.GetComponent<SoundMixer>()?.audioMixer;
            // 切换到默认模式播放通道
            SnapSwitch();

            // 放入混音组件
            _mixerGroups.Clear();
            foreach (var mixerGroup in soundMixer.FindMatchingGroups(""))
            {
                _mixerGroups.Add(mixerGroup.name, mixerGroup);
            }


            var listener = GameObject.FindObjectOfType<AudioListener>();
            if (listener == null) _audioNode.AddComponent<AudioListener>();

            // _dubbSource = _audioNode.AddComponent<AudioSource>();
            _dubbSource = GetSoundSource();
            _dubbSource.loop = false;
            var AudioSound = PlayerPrefs.GetInt("AudioSound", 1);
            var AudioMusic = PlayerPrefs.GetInt("AudioMusic", 1);
            SetMusicIsOn(AudioSound == 1);
            SetSoundIsOn(AudioMusic == 1);

            // 钩子勾上底层发来的点击音效事件
            HookAudioComponet();
        }

        public void HookAudioComponet()
        {
            // AudioComponent._.Click += delegate(Button button)
            // {
            //     // 可根据 button 位置自行判断是否需要播放音效
            //     Play("click", false, true);
            // };
            //
            // AudioComponent._.ButtonOverAudio += delegate(string s)
            // {
            //     if (string.IsNullOrEmpty(s)) return;
            //     Play(s, false, true);
            // };
        }


        #region 配音

        private System.IDisposable _dubbTimer;
        private Action _dubbFinishCb;

        /// <summary>
        /// 暂停配音
        /// </summary>
        public void PauseDubb()
        {
            _dubbSource.Pause();
            _dubbTimer?.Dispose();
            // _musicSource.volume = 1;
        }

        /// <summary>
        /// 停止配音播放
        /// </summary>
        public void StopDubb()
        {
            _dubbSource.Stop();
            _dubbTimer?.Dispose();
            _dubbFinishCb = null;
            SnapSwitch();
            // _musicSource.volume = 1;
        }

        /// <summary>
        /// 获得配音的长度
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public float GetAudioLength(string name)
        {
            if (string.IsNullOrEmpty(name)) return 0;
            if (_audioMap.ContainsKey(name))
                return _audioMap[name].length;
            var paths = AssetIndex._.FindsAtPath("Assets/Projects/Audio/Dubbing", name);
            foreach (var path in paths)
            {
                Debug.Log("加载声音" + path);
                var clip = Res.LoadAssetSync(path, typeof(AudioClip));
                return clip.Get<AudioClip>().length;
            }

            return 0;
        }

        /// <summary>
        /// 播放配音
        /// </summary>
        /// <param name="name"></param>
        public void PlayDubb(string name, Action dubbFinishCb)
        {
            _dubbFinishCb = dubbFinishCb;

            _dubbSource.outputAudioMixerGroup = _mixerGroups[MixerMod.Dubbing.ToString()];
            if (_dubbSource.clip != null && _dubbSource.clip.name == name)
            {
                _dubbSource.Play();
                // _musicSource.volume = 0.8f;
                SnapSwitch(true);
                _dubbTimer = Observable.Timer(TimeSpan.FromSeconds(_dubbSource.clip.length - _dubbSource.time))
                    .Subscribe(_ => { _dubbFinishCb?.Invoke(); });
                return;
            }

            try
            {
                // 加载声音
                AssetIndex._.FindsAtPath("Assets/Projects/Audio/Dubbing", name).ForEach(delegate(string path)
                {
                    Debug.Log("加载声音" + path);
                    Res.LoadAsset(path, delegate(IUAsset asset)
                    {
                        _dubbSource.Stop();
                        var ch = asset.Get<AudioClip>();
                        // _audioMap[name] = ch;
                        _dubbSource.clip = ch;
                        _dubbSource.Play();
                        SnapSwitch(true);
                        // _musicSource.volume = 0.8f;
                        _dubbTimer = Observable
                            .Timer(TimeSpan.FromSeconds(_dubbSource.clip.length - _dubbSource.time))
                            .Subscribe(_ => { _dubbFinishCb?.Invoke(); });
                    }, typeof(AudioClip));
                });
            }
            catch (Exception e)
            {
                if (DefaultDebug.AllowVerboseInfo)
                    Tuner.Log(e.StackTrace);
            }
        }

        #endregion

        #region 背景，音效

        public void Play(int id, bool isMusic, bool focusPlay = false, bool isdeputy = false)
        {
            var row = CsvHelper.GetMusicCsv().GetRowByUnid(id);
            Play(row.Name,isMusic,focusPlay,isdeputy);
        }
        
        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="isMusic">是否为音乐</param>
        public void Play(string name, bool isMusic, bool focusPlay = false, bool isdeputy = false)
        {
            //没有开启音乐
            if (isMusic && !_musicIsOn) return;

            // 没有开启音效
            if (!isMusic && !_soundIsOn) return;

            // 正在播放要求的背景音乐，并且不要求重头播放时，跳过
            if (isMusic && name == _curPlayMusicName && !focusPlay) return;
            if (isMusic && name == _curPlayDeputyMusicName && !focusPlay) return;

            if (isMusic && !isdeputy)
                _curPlayMusicName = name;
            if (isMusic && isdeputy)
                _curPlayDeputyMusicName = name;
            PlaySource(name, isMusic, isdeputy);
        }

        /// <summary>
        /// 停止副音乐
        /// </summary>
        public void StopDeputyMusic(float time=0.5f)
        {
            if (_deputyMusicSource != null)
                RecycleSource(_deputyMusicSource,time);
            _deputyMusicSource = null;
            _curPlayDeputyMusicName = "";
        }

        /// <summary>
        /// 停止音乐
        /// </summary>
        public void StopMusic(float time=0.5f)
        {
            if (_musicSource!=null)
                RecycleSource(_musicSource,time);
            _musicSource = null;
            _curPlayMusicName = "";
        }

        /// <summary>
        /// 根据名字播放
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isMusic"></param>
        private void PlaySource(string name, bool isMusic, bool isdeputy)
        {
            try
            {
                if (_audioMap.ContainsKey(name))
                {
                    PlaySource(_audioMap[name], isMusic, isdeputy);
                    return;
                }

                // 加载声音
                AssetIndex._.FindsAtPath("Assets/Projects/Audio/", name).ForEach(delegate(string path)
                {
                    Debug.Log("加载声音" + path);
                    Res.LoadAsset(path, delegate(IUAsset asset)
                    {
                        var ch = asset.Get<AudioClip>();
                        // 只缓存点击音效，不缓存背景音乐
                        if (!isMusic)
                            _audioMap[name] = ch;
                        PlaySource(ch, isMusic, isdeputy);
                    }, typeof(AudioClip));
                });
            }
            catch (Exception e)
            {
                if (DefaultDebug.AllowVerboseInfo)
                    Tuner.Log(e.StackTrace);
            }
        }

        /// <summary>
        /// 根据片段播放
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="isMusic"></param>
        private void PlaySource(AudioClip clip, bool isMusic, bool isdeputy)
        {
            AudioSource source = GetSoundSource();
            if (isMusic)
            {
                var mSource = isdeputy ? _deputyMusicSource : _musicSource;
                if (mSource != null)
                {
                    var preAs = mSource;
                    RecycleSource(preAs,0.5f);
                }

                source.volume = 0;
                DOTween.To(value => source.volume = value, 0, _musicVol, 0.5f);
                if (isdeputy)
                    _deputyMusicSource = source;
                else
                    _musicSource = source;
                SetMusicIsOn(GetMusicIsOn());
            }
            else
            {
                // 音效音量
                source.volume = _soundVol;
            }

            source.clip = clip;
            source.loop = isMusic;
            if (isMusic && !isdeputy)
                source.outputAudioMixerGroup = _mixerGroups[MixerMod.Music.ToString()];
            else if (isMusic && isdeputy)
                source.outputAudioMixerGroup = _mixerGroups[MixerMod.DeputyMusic.ToString()];
            else
                source.outputAudioMixerGroup = _mixerGroups[MixerMod.Sound.ToString()];

            source.Play();
            // 音效播放完毕后回收声音源
            if (!isMusic)
                Observable.Timer(TimeSpan.FromSeconds(clip.length)).Subscribe(value => RecycleSource(source,0));
        }

        /// <summary>
        /// 回收audiosource
        /// </summary>
        private void RecycleSource(AudioSource source,float time)
        {
            if (time==0)
                Stop(source);
            else
                DOTween.To(x => source.volume = x, source.volume, 0, time)
                    .SetEase(Ease.Linear)
                    .onComplete = () => Stop(source);

            void Stop(AudioSource mSource)
            {
                mSource.Stop();
                mSource.clip = null;
                _audioList.Remove(mSource);
                mSource.mute = false;
                _audioPool.Enqueue(mSource);    
            }
            
        }

        /// <summary>
        /// Get a audio source.
        /// </summary>
        private AudioSource GetSoundSource()
        {
            AudioSource source = _audioPool.Count > 0
                ? _audioPool.Dequeue()
                : _audioNode.AddComponent<AudioSource>();

            _audioList.Add(source);
            // source.volume = _soundVol;
            return source;
        }

        #endregion

        #region Set

        /// <summary>
        /// Set all audio source volumn
        /// </summary>
        /// <param name="value"></param>
        public void SetAllVolumn(float value)
        {
            SetSoundVolume(value);
            SetMusicVolume(value);
        }

        /// <summary>
        /// Set volume value, range 0 ~ 100.
        /// </summary>
        public void SetSoundVolume(int value)
        {
            var volume = (float) value / 100f;
            SetSoundVolume(volume);
        }

        /// <summary>
        /// Set volume value, range 0f ~ 1f.
        /// </summary>
        public void SetSoundVolume(float value)
        {
            foreach (var audioSource in _audioList)
                audioSource.volume = value;
            _soundVol = value;
        }

        /// <summary>
        /// Set volume value, range 0 ~ 100.
        /// </summary>
        public void SetMusicVolume(int value)
        {
            var volume = (float) value / 100f;
            SetMusicVolume(volume);
        }

        /// <summary>
        /// Set volume value, range 0f ~ 1f.
        /// </summary>
        public void SetMusicVolume(float value)
        {
            _musicSource.volume = value;
            _musicVol = value;
        }


        /// <summary>
        /// 是否打开音乐
        /// </summary>
        /// <param name="status">true 有声  false 静音</param>
        public void SetMusicIsOn(bool status)
        {
            if (status)
                PlayerPrefs.SetInt("AudioMusic", 1);
            else
                PlayerPrefs.SetInt("AudioMusic", 0);
            if (_musicSource != null)
                _musicSource.mute = !status;
            _musicIsOn = status;
        }


        /// <summary>
        /// 是否打开音乐
        /// </summary>
        /// <param name="status">true 有声  false 静音</param>
        public bool GetMusicIsOn()
        {
            return _musicIsOn;
        }

        /// <summary>
        /// Volume mute on-off.
        /// </summary>
        public void SetSoundIsOn(bool status)
        {
            if (status)
                PlayerPrefs.SetInt("AudioSound", 1);
            else
                PlayerPrefs.SetInt("AudioSound", 0);


            foreach (var audioSource in _audioList)
            {
                if (audioSource != _musicSource)
                    audioSource.mute = !status;
            }

            _soundIsOn = status;
        }


        /// <summary>
        /// Volume mute on-off.
        /// </summary>
        public bool GetSoundIsOn()
        {
            return _soundIsOn;
        }


        /// <summary>
        /// 暂停音乐
        /// </summary>
        /// <param name="id"></param>
        public void PauseMusic() => _musicSource.Pause();

        /// <summary>
        /// 继续播放
        /// </summary>
        /// <param name="id"></param>
        public void UnPauseMusic() => _musicSource.UnPause();

        #endregion
    }
}