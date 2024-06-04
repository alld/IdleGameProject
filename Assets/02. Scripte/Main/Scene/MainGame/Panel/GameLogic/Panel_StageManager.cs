using IdleGame.Core;
using IdleGame.Data;
using IdleGame.Data.Base;
using IdleGame.Data.Common.Log;

namespace IdleGame.Main.GameLogic
{
    /// <summary>
    /// [기능] 스페이지를 구성하고 관리하는 매니저입니다.
    /// </summary>
    public class Panel_StageManager : Base_Panel
    {
        /// <summary>
        /// [상태] 현재 진행중인 데이터 정보입니다.
        /// </summary>
        Data_Stage mainStage;

        /// <summary>
        /// [상태] 진행중이던 스테이지를 잠시 중단하고 다른 스테이지가 우선 진행될때 진행상황을 저장한 정보입니다.
        /// </summary>
        Data_Stage subStage;


        /// <summary>
        /// [설정] 새로운 스테이지 정보를 설정합니다. 
        /// <br> 스테이지 정보가 들어오면 현재 진행중인 상황을 정리하고 새로운 설정에 맞쳐 준비합니다. </br>
        /// </summary>
        public void Logic_SetStage(Data_Stage m_data, bool m_isMainData = true)
        {
            if (!m_isMainData)
                Logic_ChangeStage();

            mainStage = m_data;
            mainStage.procedures = eProcedures.Initing;

            Logic_SetLevel(mainStage.currentLevel);
        }

        /// <summary>
        /// [기능] 현재 적용중인 메인 스테이지와 예비로 보관중인 서브 스테이지를 교환합니다. 
        /// </summary>
        private void Logic_ChangeStage()
        {
            Data_Stage changeData = subStage;
            subStage = mainStage;
            mainStage = changeData;

            subStage.procedures = eProcedures.Stopping;
            mainStage.procedures = eProcedures.Initing;
        }

        /// <summary>
        /// [기능] 다음 레벨을 진행시킵니다. 
        /// </summary>
        public void Logic_NextLevel()
        {
            mainStage.currentLevel++;
            if (mainStage.currentLevel > mainStage.maxLevel)
                Logic_TryNextStage();

            Logic_SetLevel(mainStage.currentLevel);
        }

        /// <summary>
        /// [기능] 현재 레벨 정보를 기준으로 UI 정보를 갱신합니다.
        /// </summary>
        public void Logic_StageUIUpdate()
        {

        }


        /// <summary>
        /// [기능] 다음 스테이지로 넘어갑니다. 
        /// </summary>
        public void Logic_TryNextStage()
        {
            mainStage.procedures = eProcedures.Exhaustion;
            if (mainStage.isMainStory)
                GameManager.Log.Logic_PutLog(new Data_Log("다음 스테이지로 진행함"));
            // todo :: 스테이지 테이블에서 새로운 데이터를 가져옵니다. 
            else
                Logic_ChangeStage();
        }

        /// <summary>
        /// [설정] 특정 레벨로 필드를 설정합니다. 
        /// </summary>
        public void Logic_SetLevel(int m_level)
        {
            Logic_StageUIUpdate();

            // TODO :: 몬스터 종류에 맞쳐서 오브젝트 풀에서 가져와 필드에 셋팅합니다. 
        }

        /// <summary>
        /// [기능] 스테이지의 진행상황을 초기화시키고 중단시킵니다.
        /// </summary>
        public void Logic_StageStop()
        {

        }

        /// <summary>
        /// [기능] 진행중인 스테이지를 그상태로 진행을 멈춥니다.
        /// </summary>
        public void Logic_StagePause()
        {

        }

        /// <summary>
        /// [기능] 일시 정지된 상태의 스테이지를 진행시킵니다.
        /// </summary>
        public void Logic_StageContinue()
        {

        }

        /// <summary>
        /// [기능] 중단된 스테이지를 현재 저장된 데이터를 기반으로 복구하여 재실행합니다. 
        /// </summary>
        public void Logic_StageStart()
        {

        }
    }
}