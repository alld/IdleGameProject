using DG.Tweening;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common;
using IdleGame.Data.Common.Log;
using System.Collections.Generic;
using UnityEngine;

namespace IdleGame.Core.Panel.Sound
{
    /// <summary>
    /// [기능] 실제 사운드를 실행시키고 재생, 정지, 일시정지 등 다양한 형태로 관리하는 사운드 매니저입니다. 
    /// </summary>

    public class Panel_SoundManager : Base_ManagerPanel
    {
        /// <summary>
        /// [캐시] 사운드 매니저가 사운드를 재생하기 위해서 별도로 관리되는 오디오 그룹입니다. 
        /// </summary>
        [Header("SoundManager")]
        [SerializeField]
        private Data_AudioSourceComponent _audio;


        /// <summary>
        /// [데이터] 사운드 클립들을 인스펙터에서 직접 할당받은 데이터 리스트입니다. 
        /// </summary>
        [Header("SoundLibrary")]
        [SerializeField]
        private Data_Sound[] _libraryData;

        /// <summary>
        /// [데이터] 재정리된 사운드 클립 리스트입니다.
        /// </summary>
        private Dictionary<eSoundKind, Data_Sound> _library;

        /// <summary>
        /// [상태] 현재 플레이중인 BGM 인덱스를 나타냅니다. 
        /// </summary>
        private bool _playing_BgmZeroIndex = true;

        /// <summary>
        /// [상태] BGM 플레이어의 정지상태를 나타냅니다. 
        /// </summary>
        private bool[] _checkPause;

        /// <summary>
        /// [상태] 전체 오디오가 일시정지 상태인지를 나타냅니다.
        /// </summary>
        private bool _isAudioPause;

        /// <summary>
        /// [데이터] 페이드가 진행되는 시간을 지정합니다. 
        /// </summary>
        private const float _FadeTime = 1;

        /// <summary>
        /// [상태] 현재 SFX의 오디오 플레이어의 인덱스값을 나타냅니다.
        /// </summary>
        private int _currentSFXIndex = -1;

        /// <summary>
        /// [상태] 현재 Voice의 오디오 플레이어의 인덱스값을 나타냅니다.
        /// </summary>
        private int _currentVoiceIndex = -1;

        /// <summary>
        /// [캐시] 사용가능한 오디오 플레이어 풀입니다.
        /// </summary>
        private Queue<AudioSource> _HPE_aduioPool = new Queue<AudioSource>();

        protected override void Logic_Init_Custom()
        {
            Logic_RegisterLibrary();
            Logic_RegisterAudioPool();
        }

        /// <summary>
        /// [초기화] 라이브러리 데이터의 데이터를 재정리하여 접근하기 쉬운구조로 컬렉션화합니다.
        /// </summary>
        private void Logic_RegisterLibrary()
        {
            foreach (var sound in _libraryData)
            {
                _library.Add(sound.kind, sound);
            }
        }

        private void Logic_RegisterAudioPool()
        {
            foreach (var audio in _audio.hpe)
            {
                _HPE_aduioPool.Enqueue(audio);
            }
        }

        /// <summary>
        /// [기능] 현재 클립의 길이를 계산하여 페이드를 적용할 적절한 시간을 반환합니다.
        /// </summary>
        private float Logic_GetFadeTime(AudioClip m_clip)
        {
            return m_clip.length < _FadeTime ? m_clip.length : _FadeTime;
        }

        #region BGM 관련 로직
        /// <summary>
        /// [기능] BGM을 즉시 실행시킵니다.
        /// </summary>
        private void Logic_PlayBGM(bool m_fade = false)
        {
            int index = Logic_GetBGMCurrentIndex();
            if (_audio.bgm[index].clip == null)
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log(Data_ErrorType.Warning_NullValueSoundPlay));
                return;
            }

            if (_isAudioPause)
            {
                _checkPause[index] = true;
                return;
            }

            _audio.bgm[index].DOKill();
            if (m_fade)
            {
                _audio.bgm[index].volume = 0;
                if (_checkPause[index])
                    _audio.bgm[index].UnPause();
                else
                    _audio.bgm[index].Play();
                _audio.bgm[index].DOFade(1, Logic_GetFadeTime(_audio.bgm[index].clip));
            }
            else
            {
                if (_checkPause[index])
                    _audio.bgm[index].UnPause();
                else
                    _audio.bgm[index].Play();
            }

            _checkPause[index] = true;
        }

        /// <summary>
        /// [기능] BGM을 즉시 실행시킵니다.
        /// </summary>
        public void PlayBGM(AudioClip m_clip, bool m_fade = false)
        {
            StopCurrentBGM(m_fade);

            _audio.bgm[Logic_GetBGMCurrentIndex()].clip = m_clip;

            Logic_PlayBGM(m_fade);
        }

        /// <summary>
        /// [기능] BGM을 즉시 실행시킵니다.
        /// </summary>
        public void PlayBGM(eSoundKind m_kind, bool m_fade = false)
        {
            PlayBGM(_library[m_kind].clip);
        }

        /// <summary>
        /// [기능] 현재 재생중인 BGM을 일시 정지시키고 새로운 BGM으로 교체합니다.
        /// </summary>
        public void ChangeBGM(AudioClip m_clip, bool m_fade = false)
        {
            (int, int) playerIndex = Logic_GetBGMPlayerIndex();

            Logic_StopBGM(playerIndex.Item2);
            _audio.bgm[playerIndex.Item2].clip = m_clip;
            ChangeBGM();
        }

        /// <summary>
        /// [기능] 현재 재생중인 BGM을 일시 정지시키고 새로운 BGM으로 교체합니다.
        /// </summary>
        public void ChangeBGM(eSoundKind m_kind, bool m_fade = false)
        {
            ChangeBGM(_library[m_kind].clip);
        }

        /// <summary>
        /// [기능] 현재 재생중인 BGM을 정지시키고, 기존 BGM으로 교체합니다.
        /// <br> 기존 BGM이 없는 경우 기존 재생중이던 BGM을 계속 틉니다. </br>
        /// </summary>
        public void ChangeBGM(bool m_fade = false)
        {
            (int, int) playerIndex = Logic_ChangeBGMPlayerIndex();

            Logic_StopBGM(playerIndex.Item2, m_fade, true);

            Logic_PlayBGM(m_fade);
        }

        /// <summary>
        /// [기능] BGM 플레이어 인덱스를 교환합니다. 
        /// </summary>
        private (int, int) Logic_ChangeBGMPlayerIndex()
        {
            _playing_BgmZeroIndex = !_playing_BgmZeroIndex;

            return Logic_GetBGMPlayerIndex();
        }

        /// <summary>
        /// [기능] 현재 BGM를 메인과 대기를 구분하여 번호들을 반환합니다. 
        /// </summary>
        private (int, int) Logic_GetBGMPlayerIndex()
        {
            if (_playing_BgmZeroIndex)
                return (0, 1);
            else
                return (1, 0);
        }

        /// <summary>
        /// [기능] 현재 재생중인 BGM 플레이어 인덱스를 반환합니다. 
        /// </summary>
        private int Logic_GetBGMCurrentIndex()
        {
            return _playing_BgmZeroIndex ? 0 : 1;
        }

        /// <summary>
        /// [기능] 현재 재생중인 BGM을 정지시킵니다. 
        /// </summary>
        public void StopCurrentBGM(bool m_fade = false, bool m_pause = false)
        {
            Logic_StopBGM(Logic_GetBGMCurrentIndex(), m_fade, m_pause);
        }

        /// <summary>
        /// [기능] 특정 인덱스의 BGM을 정지시킵니다. 
        /// </summary>
        public void Logic_StopBGM(int index, bool m_fade = false, bool m_pause = false)
        {
            if (_audio.bgm[index].clip == null) return;

            _audio.bgm[index].DOKill();
            if (m_fade)
            {
                _audio.bgm[index].DOFade(0, Logic_GetFadeTime(_audio.bgm[index].clip))
                    .OnComplete(
                    () =>
                    {
                        _audio.bgm[index].Stop();
                        _audio.bgm[index].clip = null;
                    });
            }
            else
            {
                if (m_pause)
                {
                    _checkPause[index] = true;
                    _audio.bgm[index].Pause();
                }
                else
                    _audio.bgm[index].Stop();
                _audio.bgm[index].clip = null;
            }
        }

        /// <summary>
        /// [기능] 모든 BGM을 정지시킵니다.
        /// </summary>
        public void StopBGM(bool m_fade = false)
        {
            StopCurrentBGM(m_fade);

            Logic_ChangeBGMPlayerIndex();

            StopCurrentBGM(m_fade);
        }
        #endregion

        #region SFX 관련 로직
        /// <summary>
        /// [기능] 효과음을 즉시 실행시킵니다.
        /// </summary>
        public float PlaySFX(eSoundKind m_kind)
        {
            return PlaySFX(_library[m_kind].clip);
        }

        /// <summary>
        /// [기능] 효과음을 즉시 실행시킵니다.
        /// </summary>
        public float PlaySFX(AudioClip m_clip)
        {
            Logic_PlaySFX(m_clip);

            if (_isAudioPause) return 0;
            return m_clip.length;
        }

        /// <summary>
        /// [기능] 효과음을 재생시킵니다. 
        /// </summary>
        /// <param name="m_clip"></param>
        private void Logic_PlaySFX(AudioClip m_clip)
        {
            if (_isAudioPause) return;

            _currentSFXIndex++;
            _currentSFXIndex %= _audio.sfx.Length;

            _audio.sfx[_currentSFXIndex].clip = m_clip;
            _audio.sfx[_currentSFXIndex].Play();
        }
        #endregion


        #region HPE 관련 로직
        /// <summary>
        /// [기능] HPE을 즉시 실행시킵니다.
        /// </summary>
        public Data_HPE PlayHPE(eSoundKind m_kind, bool m_loop = false)
        {
            return PlayHPE(_library[m_kind].clip, m_loop);
        }

        /// <summary>
        /// [기능] HPE을 즉시 실행시킵니다.
        /// </summary>
        public Data_HPE PlayHPE(AudioClip m_clip, bool m_loop = false)
        {
            if (_HPE_aduioPool.Count == 0)
                if (!Logic_RecyclingHPEPool())
                    return new Data_HPE();


            AudioSource audio = _HPE_aduioPool.Dequeue();
            audio.clip = m_clip;
            audio.loop = m_loop;

            audio.Play();
            if (_isAudioPause)
                audio.Pause();


            int index = -1;
            for (int i = 0; i < _audio.hpe.Length; i++)
            {
                if (ReferenceEquals(audio, _audio.hpe[i]))
                    index = i;
            }

            return new Data_HPE(index);
        }

        /// <summary>
        /// [기능] 현재 전부 사용된 HPE 플레이어를 찾아내서 값을 비워냅니다. 
        /// </summary>
        public bool Logic_RecyclingHPEPool()
        {
            bool check = false;

            foreach (var audio in _audio.hpe)
            {
                if (audio.isPlaying)
                {
                    _HPE_aduioPool.Enqueue(audio);
                    check = true;
                }
            }

            return check;
        }

        /// <summary>
        /// [기능] 재생중인 HPE 사운드를 정지시킵니다.
        /// </summary>
        public void StopHPE(ref Data_HPE m_hpe)
        {
            if (!m_hpe.Logic_GetUseSound() || _HPE_aduioPool.Contains(_audio.hpe[m_hpe.Logic_GetPlayIndex()]))
                return;

            m_hpe.Logic_SetUseSound();


            AudioSource audio = _audio.hpe[m_hpe.Logic_GetPlayIndex()];
            _HPE_aduioPool.Enqueue(audio);

            audio.Stop();
            audio.clip = null;
        }


        #endregion

        #region Voice 관련 로직

        /// <summary>
        /// [기능] 음성을 즉시 실행시킵니다.
        /// </summary>
        public float PlayVoice(eSoundKind m_kind)
        {
            return PlayVoice(_library[m_kind].clip);
        }

        /// <summary>
        /// [기능] 음성을 즉시 실행시킵니다.
        /// </summary>
        public float PlayVoice(AudioClip m_clip)
        {
            _currentVoiceIndex++;
            _currentVoiceIndex %= _audio.voice.Length;

            _audio.voice[_currentVoiceIndex].clip = m_clip;
            _audio.voice[_currentVoiceIndex].Play();
            if (_isAudioPause)
                _audio.voice[_currentVoiceIndex].Pause();

            return m_clip.length;
        }

        /// <summary>
        /// [기능] 모든 음성을 정지시킵니다. 
        /// </summary>
        public void StopVoice()
        {
            foreach (var audio in _audio.voice)
            {
                audio.clip = null;
                audio.Stop();
            }
        }

        #endregion

        /// <summary>
        /// [기능] 모든 사운드를 일시 정지시킵니다.
        /// </summary>
        public void AllPause()
        {
            _isAudioPause = true;

            foreach (var audio in _audio.bgm)
            {
                audio.Pause();
            }

            foreach (var audio in _audio.sfx)
            {
                audio.Stop();
            }

            foreach (var audio in _audio.hpe)
            {
                audio.Pause();
            }

            foreach (var audio in _audio.voice)
            {
                audio.Pause();
            }
        }

        /// <summary>
        /// [기능] 정지시킨 모든 사운드를 실행시킵니다.
        /// </summary>
        public void AllPlay()
        {
            _isAudioPause = false;

            foreach (var audio in _audio.bgm)
            {
                audio.UnPause();
            }

            foreach (var audio in _audio.hpe)
            {
                audio.UnPause();
            }

            foreach (var audio in _audio.voice)
            {
                audio.UnPause();
            }
        }
    }
}