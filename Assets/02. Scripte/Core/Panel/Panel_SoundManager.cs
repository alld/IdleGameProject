using IdleGame.Data.Common;
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

        protected override void Logic_Init_Custom()
        {
            Logic_RegisterLibrary();
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


        /// <summary>
        /// [기능] BGM을 즉시 실행시킵니다.
        /// </summary>
        public void PlayBGM(eSoundKind m_kind, bool m_fade = false)
        {

        }

        /// <summary>
        /// [기능] 현재 재생중인 BGM을 일시 정지시키고 새로운 BGM으로 교체합니다.
        /// </summary>
        public void ChangeBGM(eSoundKind m_kind, bool m_fade = false)
        {

        }

        /// <summary>
        /// [기능] 현재 재생중인 BGM을 정지시키고, 기존 BGM으로 교체합니다.
        /// <br> 기존 BGM이 없는 경우 기존 재생중이던 BGM을 계속 틉니다. </br>
        /// </summary>
        public void ChangeBGM(bool m_fade = false)
        {

        }

        /// <summary>
        /// [기능] 효과음을 즉시 실행시킵니다.
        /// </summary>
        public float PlaySFX(eSoundKind m_kind)
        {
            return _library[m_kind].clip.length;
        }

        /// <summary>
        /// [기능] HPE을 즉시 실행시킵니다.
        /// </summary>
        public Data_HPE PlayHPE(eSoundKind m_kind)
        {
            Data_HPE hpe = new Data_HPE(5);

            return hpe;
        }

        /// <summary>
        /// [기능] 음성을 즉시 실행시킵니다.
        /// </summary>
        public float PlayVoice(eSoundKind m_kind)
        {
            return _library[m_kind].clip.length;
        }

        /// <summary>
        /// [기능] 현재 재생중인 BGM을 정지시킵니다. 
        /// </summary>
        public void StopCurrentBGM(bool m_fade = false)
        {

        }

        /// <summary>
        /// [기능] 모든 BGM을 정지시킵니다.
        /// </summary>
        public void StopBGM(bool m_fade = false)
        {

        }

        /// <summary>
        /// [기능] 재생중인 HPE 사운드를 정지시킵니다.
        /// </summary>
        public void StopHPE(ref Data_HPE m_hpe)
        {
            m_hpe.Logic_SetUseSound();
        }

        /// <summary>
        /// [기능] 모든 음성을 정지시킵니다. 
        /// </summary>
        public void StopVoice()
        {

        }

        /// <summary>
        /// [기능] 모든 사운드를 일시 정지시킵니다.
        /// </summary>
        public void AllPause()
        {

        }

        /// <summary>
        /// [기능] 정지시킨 모든 사운드를 실행시킵니다.
        /// </summary>
        public void AllPlay()
        {

        }
    }
}