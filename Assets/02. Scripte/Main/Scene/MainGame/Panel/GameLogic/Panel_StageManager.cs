using IdleGame.Core;
using IdleGame.Core.Unit;
using IdleGame.Data;
using IdleGame.Data.Base;
using IdleGame.Data.Common.Log;
using IdleGame.Data.DataTable;
using IdleGame.Data.Pool;
using IdleGame.Main.Scene.Main;
using IdleGame.Main.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleGame.Main.GameLogic
{
    /// <summary>
    /// [기능] 스페이지를 구성하고 관리하는 매니저입니다.
    /// </summary>
    public class Panel_StageManager : Base_ManagerPanel
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
        /// [데이터] 플레이어 시작 위치입니다. 
        /// </summary>
        [Header("Stage")]
        [SerializeField]
        private Vector3[] playerStartPos;
        /// <summary>
        /// [데이터] 플레이어 유닛들이 화면 밖에서 등장하는 위치입니다.
        /// </summary>
        [SerializeField]
        private Vector3[] playerAppearPos;
        /// <summary>
        /// [데이터] 몬스터들의 시작 위치입니다.
        /// </summary>
        [SerializeField]
        private Vector3 enemyStartPos;
        /// <summary>
        /// [데이터] 몬스터들의 화면밖 등장 위치입니다.
        /// </summary>
        [SerializeField]
        private Vector3 enemyAppearPos;

        /// <summary>
        /// [상태] 스테이지 게임이 진행중인지를 나타냅니다. 
        /// </summary>
        public static bool IsPlayingGame = false;

        /// <summary>
        /// [캐시] 현재 게임중인 플레이어 유닛입니다.
        /// </summary>
        public static Controller_PlayerUnit Unit_Player;

        /// <summary>
        /// [캐시] 스테이지에 등장한 몬스터 수입니다.
        /// </summary>
        public static List<Controller_EnemyUnit> Unit_Monsters = new List<Controller_EnemyUnit>();

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

            Logic_SetLevel(mainStage.currentWave);
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
        /// [기능] 다음 웨이브나 스테이지로 진행을 시도합니다.
        /// </summary>
        public bool Logic_TryNextLevel()
        {
            if (Unit_Monsters.Count != 0) return false;

            Logic_NextLevel();

            return true;
        }

        /// <summary>
        /// [기능] 다음 레벨을 진행시킵니다. 
        /// </summary>
        public void Logic_NextLevel()
        {
            Unit_Monsters.Clear();
            mainStage.currentWave++;
            Global_Data.PlayProgress.stage_curWave = mainStage.currentWave;

            if (mainStage.currentWave >= mainStage.wave_num)
                Logic_TryNextStage();

            Logic_SetLevel(mainStage.currentWave);
            Logic_MonsterPush();
            Logic_PlayerSetting();

            StartCoroutine(Logic_TempAppear());
        }

        /// <summary>
        /// [기능] 몬스터를 추가합니다. 
        /// </summary>
        private void Logic_MonsterPush()
        {
            for (int i = 0; i < mainStage.monster_max[mainStage.currentWave]; i++)
            {
                var monster = GameManager.Pool.Logic_GetObject(ePoolType.Enemy, (GameManager.Panel as Panel_MainGameScene).mainGamePanel.enemyGroup);
                monster.transform.localPosition = new Vector3(enemyStartPos.x + Random.Range(-1f, 1f), enemyStartPos.y + Random.Range(-1f, 1f));
                monster.gameObject.SetActive(true);
                (monster as Base_Unit).Logic_Init(new Data_UnitType(eUnitTpye.Enemy, Logic_GetRandomSelect_MonsterID()));
            }
        }

        /// <summary>
        /// [기능] 몬스터 테이블에서 확률을 적용해 적절한 몬스터를 선택합니다.
        /// </summary>
        private int Logic_GetRandomSelect_MonsterID()
        {
            float random = Random.value;
            float count = 0;

            for (int i = 0; i < mainStage.monster_id.Length; i++)
            {
                count += mainStage.monster_num[i];

                if (count >= random)
                {
                    return mainStage.monster_id[i];
                }
            }

            return 0;
        }

        /// <summary>
        /// [기능] 플레이어를 셋팅합니다. 복수의 스테이지에 별도의 캐릭터가 존재할 수 있습니다. 
        /// </summary>
        private void Logic_PlayerSetting()
        {
            if (Unit_Player != null) return;

            var player = GameManager.Pool.Logic_GetObject(ePoolType.Player, (GameManager.Panel as Panel_MainGameScene).mainGamePanel.playerGroup);
            player.transform.localPosition = playerStartPos[0];
            player.gameObject.SetActive(true);
            (player as Base_Unit).Logic_Init(new Data_UnitType(eUnitTpye.Player, Global_Data.Player.characterID));
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
            if (true) // 메인 스테이지인지를 판단함, 아닌 경우 ChangeStage를 진행해서 메인스테이지로 전환을 시도함
            {
                if (Library_DataTable.stage.ContainsKey(mainStage.next_stage))
                {
                    Global_Data.PlayProgress.stage_curWave = 0;
                    Global_Data.PlayProgress.stage_curIndex = mainStage.next_stage;

                    Logic_SetStage(Library_DataTable.stage[Global_Data.PlayProgress.stage_curIndex]);

                    GameManager.Log.Logic_PutLog(new Data_Log("다음 스테이지로 진행함"));
                    //todo::스테이지 테이블에서 새로운 데이터를 가져옵니다.
                }
                else
                {
                    Logic_NoMoreStages();
                }
            }
            else
                Logic_ChangeStage();
        }

        /// <summary>
        /// [기능] 더이상 진행할 스테이지가 없는 경우 호출됩니다.
        /// </summary>
        private void Logic_NoMoreStages()
        {
            Logic_SetStage(Library_DataTable.stage[Global_Data.PlayProgress.stage_curIndex]);
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
            // TODO 임시..
            mainStage.currentWave--;
            Logic_NextLevel();

            IsPlayingGame = true;
        }

        private IEnumerator Logic_TempAppear()
        {
            while (IsPlayingGame == false)
                yield return null;

            Unit_Player.Logic_Act_Appear();
            for (int i = 0; i < Unit_Monsters.Count; i++)
            {
                Unit_Monsters[i].Logic_Act_Appear();
            }
        }
    }
}