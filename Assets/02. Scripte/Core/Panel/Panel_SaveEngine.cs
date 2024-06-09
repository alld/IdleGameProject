using IdleGame.Core.Procedure;
using IdleGame.Data.Base;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Common.Log;
using UnityEngine;

namespace IdleGame.Core.Panel.SaveEngine
{
    /// <summary>
    /// [기능] 세이브와 로드를 전반적으로 통제하는 패널입니다.
    /// </summary>
    public class Panel_SaveEngine : Base_Panel
    {
        /// <summary>
        /// [기능] 세이브의 암호화 로직을 담당합니다.
        /// </summary>
        private Module_Encryption _encryption = new Module_Encryption();

        /// <summary>
        /// [캐시] 실질적인 세이브를 기능을 담당하는 로직입니다. 
        /// </summary>
        [SerializeField]
        private Logic_Save _logic = null;

        /// <summary>
        /// [상태] 현재 세이브가 진행중인지를 확인합니다. 
        /// </summary>
        public bool isSaveing => _tag.procedure == eProcedures.Running;

        /// <summary>
        /// [상태] 필수적인 값이 변동되어 세이브가 필요한 경우를 판단합니다.
        /// </summary>
        private bool _isTrigger = false;

        /// <summary>
        /// [상태] 로드가 가능한 상태인지를 확인합니다. 
        /// </summary>
        private bool _usedLoadData = true;

        /// <summary>
        /// [상태] 로드가 가능한 상태인지를 확인합니다. 
        /// </summary>
        public bool usedLoadData => _usedLoadData;

        /// <summary>
        /// [기능] 세이브가 이루어져야하는 필수적인 값의 변동이 있을때 호출합니다. 
        /// </summary>
        public void SetTrigger() => _isTrigger = true;

        protected override void Logic_Init_Custom()
        {
            Logic_CheckLoadData();
        }

        protected override void Logic_RegisterEvent_Custom()
        {
            Base_Engine.Event.RegisterEvent(eGlobalEventType.Save_OnResponseSave, OnResponseSave);
            Base_Engine.Event.RegisterEvent(eGlobalEventType.Save_OnResponseLoad, OnResponseLoad);
        }

        /// <summary>
        /// [기능] 로드할 데이터가 있는지 확인합니다. 
        /// </summary>
        public void Logic_CheckLoadData() => _usedLoadData = _logic.Check_UsedLoadData();

        /// <summary>
        /// [기능] 동기적 세이브를 진행합니다.
        /// <br> 해당 세이브는 특수한 경우에서만 사용해주세요. </br>
        /// </summary>
        public void Logic_SyncSave()
        {
            if (!Check_TrySaveInit()) return;

            _logic.SyncSave();

            Logic_CompleteSave();
        }

        /// <summary>
        /// [기능] 세이브 파일을 삭제합니다. 
        /// </summary>
        public void Editor_DeleteSave()
        {
            _logic.DeleteSaveFile();
        }

        /// <summary>
        /// [기능] 에디터에서 파일을 저장합니다. 
        /// <br> 해당 기능은 인게임에서만 실행이 가능합니다. </br>
        /// </summary>
        public void Editor_Save()
        {
            Logic_SyncSave();
        }

        /// <summary>
        /// [기능] 에디터에서 파일을 불러옵니다. 
        /// </summary>
        public void Editor_Load()
        {
            if (!Check_TrySaveInit()) return;

            _logic.Load();
        }

        /// <summary>
        /// [기능] 비동기적 세이브를 진행합니다.
        /// </summary>
        public void Logic_Save(bool m_isForced = false)
        {
            if (!m_isForced && !_isTrigger) return;

            if (!Check_TrySaveInit()) return;

            _logic.Save();
        }

        /// <summary>
        /// [기능] 비동기적 로드를 진행합니다. 
        /// </summary>
        public void Logic_Load()
        {
            if (!_usedLoadData) return;

            if (!Check_TrySaveInit()) return;

            _logic.Load();
        }

        /// <summary>
        /// [기능] 세이브 로직에대한 초기화를 시도합니다. 현재 세이브가 진행중이라면 False를 반환합니다. 
        /// </summary>
        private bool Check_TrySaveInit()
        {
            if (isSaveing)
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log(Data_ErrorType.Warning_IsSaveing, _tag.tag));
                return false;
            }
            _tag.procedure = eProcedures.Running;
            _isTrigger = false;

            _logic.init();

            return true;
        }

        /// <summary>
        /// [기능] 성공적으로 세이브나 로드가 완료된 경우 호출하여 설정값을 변경합니다. 
        /// </summary>
        private void Logic_CompleteSave()
        {
            _tag.procedure = eProcedures.Stay;
        }

        #region 콜백 함수
        /// <summary>
        /// [콜백 함수] 성공적으로 세이브가 완료된 경우 호출됩니다.
        /// </summary>
        private void OnResponseSave()
        {
            Logic_CompleteSave();
        }

        /// <summary>
        /// [콜백 함수] 성공적으로 로드가 완료된 경우 호출됩니다. 
        /// </summary>
        private void OnResponseLoad()
        {
            _usedLoadData = false;

            Logic_CompleteSave();
        }
        #endregion
    }
}