using IdleGame.Core.Module.DataTable;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Event;
using IdleGame.Data.DataTable;
using System.Text.RegularExpressions;
using UnityEngine;


namespace IdleGame.Core.Panel.DataTable
{
    /// <summary>
    /// [기능] 데이터 테이블을 구성합니다. 
    /// </summary>
    public class Panel_DataTableManager : Base_Panel
    {
        /// <summary>
        /// 데이터 테이블의 전반적인 정보를 담고 있습니다. 파싱하는 과정에서 필요한 데이터입니다. 
        /// </summary>
        private Data_DataTableInfo _dataTableInfo = new Data_DataTableInfo();

#if DevelopeMode
        [SerializeField]
        private Module_WebTableLoader _tableLoader;
#else
        [SerializeField]
        private Module_TSVConvert _tableLoader;
#endif

        protected override void Logic_Init_Custom()
        {
            transform.GetChild(0).TryGetComponent(out _tableLoader);
            if (_tableLoader != null) TryLoadData(eDataTableType.GameInfo);
        }

        protected override void Logic_RegisterEvent_Custom()
        {
            Base_Engine.Event.RegisterEvent<eDataTableType, string[], int>(eGlobalEventType.OnResponseData_Table, OnResponseData);
            //Base_Engine.Event.RegisterEvent(eGlobalEventType.SetLanguage, LoadData_TextTable);
        }

        /// <summary>
        /// [기능] 모든 데이터 테이블을 가져와 파싱을 시도합니다.
        /// </summary>
        public void LoadData()
        {
            LoadData_StageTable();
            LoadData_MonsterTable();
            LoadData_WeaponTable();
            LoadData_CharacterTable();
            LoadData_TextTable();
        }

        /// <summary>
        /// [기능] 스테이지 테이블을 불러와 Global_Data를 갱신합니다.
        /// </summary>
        public void LoadData_StageTable()
        {
            TryLoadData(eDataTableType.Stage);
        }

        /// <summary>
        /// [기능] 몬스터 테이블을 불러와 Global_Data를 갱신합니다.
        /// </summary>
        public void LoadData_MonsterTable()
        {
            TryLoadData(eDataTableType.Monsters);
        }

        /// <summary>
        /// [기능] 무기 테이블을 불러와 Global_Data를 갱신합니다.
        /// </summary>
        public void LoadData_WeaponTable()
        {
            TryLoadData(eDataTableType.Weapon);
        }

        /// <summary>
        /// [기능] 무기 테이블을 불러와 Global_Data를 갱신합니다.
        /// </summary>
        public void LoadData_CharacterTable()
        {
            TryLoadData(eDataTableType.Character);
        }

        /// <summary>
        /// [기능] 무기 특성 테이블을 불러와 Global_Data를 갱신합니다.
        /// </summary>
        public void LoadData_WeaponPropertyTable()
        {
            TryLoadData(eDataTableType.WeaponProperty);
        }

        /// <summary>
        /// [기능] 몬스터 테이블을 불러와 Global_Data를 갱신합니다.
        /// </summary>
        public void LoadData_TextTable()
        {
            TryLoadData(eDataTableType.CommonText);
            TryLoadData(eDataTableType.BasicText);
        }


        public void TryLoadData(eDataTableType m_kind)
        {
            StartCoroutine(_tableLoader.LoadData(m_kind));
        }

        private void OnResponseData(eDataTableType m_type, string[] m_dataArray, int m_errorCode)
        {
            if (m_errorCode < 0)
            {
                // TODO :: 에러메시지 출력 (데이터를 불러오는 과정에서 에러가 발생하였습니다.)
                // m_dataArray[0]에 메시지 타입이 담겨있음.
                return;
            }

            switch (m_type)
            {
                case eDataTableType.GameInfo:
                    _tableLoader.LoaderSetting(Convert_GameInfo(m_dataArray));

                    LoadData();
                    break;
                case eDataTableType.Stage:
                    Convert_StageTable(m_dataArray);
                    break;
                case eDataTableType.Monsters:
                    Convert_MonsterTable(m_dataArray);
                    break;
                case eDataTableType.Weapon:
                    Convert_WeaponTable(m_dataArray);
                    break;
                case eDataTableType.Character:
                    Convert_CharacterTable(m_dataArray);
                    break;
                case eDataTableType.WeaponProperty:
                    Convert_WeaponPropertyTable(m_dataArray);
                    break;
                case eDataTableType.CommonText:
                    Convert_CommonTextTable(m_dataArray);
                    //Logic_TextData.OnChangeLanguage();
                    break;
                case eDataTableType.BasicText:
                    Convert_BasicTextTable(m_dataArray);
                    //Logic_TextData.OnChangeLanguage();
                    break;
                default:
                    // TODO :: 에러메시지 출력 (설정되지않은 데이터 타입이 지정되었습니다.)
                    break;
            }
        }

        #region 데이터 파싱
        private Data_DataTableInfo Convert_GameInfo(string[] m_dataArray)
        {
            string[] resultData = m_dataArray[0].Split("\t");

            _dataTableInfo.version = resultData[0];

            for (int i = 1; i < resultData.Length - 4; i += 2)
            {
                _dataTableInfo.dataTableList.Add((eDataTableType)((i + 1) / 2), (resultData[i], resultData[i + 1]));
            }

            GetCountData(resultData[17], out _dataTableInfo.commonTextTableCount);
            _dataTableInfo.commonTextTableURL = resultData[18];
            GetCountData(resultData[19], out _dataTableInfo.basicTextTableCount);
            _dataTableInfo.basicTextTableURL = resultData[20];
            return _dataTableInfo;
        }


        private void Convert_StageTable(string[] m_dataArray)
        {
            //Global_Data.stageTable.Clear();

            //for (int i = 0; i < m_dataArray.Length; i++)
            //{
            //    Data_Stage parsingData = new Data_Stage();
            //    string[] dataSegment = m_dataArray[i].Split("\t");

            //    parsingData.index = int.Parse(dataSegment[0]);
            //    parsingData.tileKind = (eMapTileKind)Enum.Parse(typeof(eMapTileKind), dataSegment[1]);

            //    string[] tempStringData = dataSegment[2].Split(",");
            //    parsingData.monsters = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.monsters[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[3].Split(",");
            //    parsingData.nameds = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.nameds[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[4].Split(",");
            //    parsingData.bosses = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.bosses[j] = int.Parse(tempStringData[j]);
            //    }

            //    parsingData.clearTime = int.Parse(dataSegment[5]);

            //    tempStringData = dataSegment[6].Split(",");
            //    parsingData.rewards = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.rewards[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[7].Split(",");
            //    parsingData.mainEventTimes = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.mainEventTimes[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[8].Split(",");
            //    parsingData.mainEventBossMobs = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.mainEventBossMobs[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[9].Split(",");
            //    parsingData.subEventTimes = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.subEventTimes[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[10].Split(",");
            //    parsingData.subEventNamedMobs = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.subEventNamedMobs[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[11].Split(",");
            //    parsingData.specialEventTimes = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.specialEventTimes[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[12].Split(",");
            //    parsingData.specialEventTypes = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.specialEventTypes[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[13].Split(",");
            //    parsingData.mobChangeTimes = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.mobChangeTimes[j] = int.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[14].Split(",");
            //    parsingData.mobChanges = new int[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.mobChanges[j] = int.Parse(tempStringData[j]);
            //    }

            //    parsingData.branchDelay = float.Parse(dataSegment[15]);

            //    tempStringData = dataSegment[16].Split(",");
            //    parsingData.startEndSpawnDelay = new float[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.startEndSpawnDelay[j] = float.Parse(tempStringData[j]);
            //    }

            //    parsingData.spawnDelayInterval = float.Parse(dataSegment[17]);

            //    Global_Data.stageTable.Add(parsingData.index, parsingData);
            //}
        }

        private void Convert_MonsterTable(string[] m_dataArray)
        {
            //Global_Data.mosnterTable.Clear();

            //for (int i = 0; i < m_dataArray.Length; i++)
            //{
            //    Data_Monster parsingData = new Data_Monster();
            //    string[] dataSegment = m_dataArray[i].Split("\t");

            //    parsingData.ID = int.Parse(dataSegment[0]);
            //    parsingData.kind = (eMonsterKind)parsingData.ID;
            //    parsingData.name = dataSegment[1];
            //    parsingData.type = (eUnitType)Enum.Parse(typeof(eUnitType), dataSegment[2]);
            //    parsingData.hp = float.Parse(dataSegment[3]);
            //    parsingData.damage = float.Parse(dataSegment[4]);
            //    parsingData.moveSpeed = float.Parse(dataSegment[5]);
            //    parsingData.armor = float.Parse(dataSegment[6]);

            //    parsingData.namedHp = float.Parse(dataSegment[7]);
            //    parsingData.namedDamage = float.Parse(dataSegment[8]);
            //    parsingData.namedArmor = float.Parse(dataSegment[9]);

            //    parsingData.bossHp = float.Parse(dataSegment[10]);
            //    parsingData.bossDamage = float.Parse(dataSegment[11]);
            //    parsingData.bossArmor = float.Parse(dataSegment[12]);

            //    Global_Data.mosnterTable.Add(parsingData.kind, parsingData);
            //}
        }

        private void Convert_WeaponTable(string[] m_dataArray)
        {
            //Global_Data.weaponTable.Clear();

            //for (int i = 0; i < m_dataArray.Length; i++)
            //{
            //    Data_Weapon_Stats parsingData = new Data_Weapon_Stats();
            //    string[] dataSegment = m_dataArray[i].Split("\t");

            //    parsingData.ID = int.Parse(dataSegment[0]);
            //    parsingData.name = dataSegment[1];
            //    parsingData.damage = float.Parse(dataSegment[2]);
            //    parsingData.speed = float.Parse(dataSegment[3]);
            //    parsingData.delay = float.Parse(dataSegment[4]);
            //    parsingData.collectType = (eCollectionType)Enum.Parse(typeof(eCollectionType), dataSegment[5]);
            //    parsingData.duration = float.Parse(dataSegment[6]);
            //    parsingData.throwCount = float.Parse(dataSegment[7]);
            //    parsingData.combineWeaponID = int.Parse(dataSegment[8]);
            //    parsingData.PassCount = int.Parse(dataSegment[9]);
            //    parsingData.maxLevel = int.Parse(dataSegment[10]);

            //    string[] tempStringData = dataSegment[11].Split(",");
            //    parsingData.addtionalDamages = new float[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.addtionalDamages[j] = float.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[12].Split(",");
            //    parsingData.addtionalMoveSpeeds = new float[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.addtionalMoveSpeeds[j] = float.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[13].Split(",");
            //    parsingData.addtionalDurations = new float[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.addtionalDurations[j] = float.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[14].Split(",");
            //    parsingData.reducionDelays = new float[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.reducionDelays[j] = float.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[14].Split(",");
            //    parsingData.addtionalThrowCount = new float[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.addtionalThrowCount[j] = float.Parse(tempStringData[j]);
            //    }

            //    tempStringData = dataSegment[15].Split(",");
            //    parsingData.addtionalPassCount = new float[tempStringData.Length];
            //    for (int j = 0; j < tempStringData.Length; j++)
            //    {
            //        parsingData.addtionalPassCount[j] = float.Parse(tempStringData[j]);
            //    }

            //    Global_Data.weaponTable.Add(parsingData.ID, parsingData);
            //}
        }

        private void Convert_CharacterTable(string[] m_dataArray)
        {
            //Global_Data.characterTable.Clear();

            //for (int i = 0; i < m_dataArray.Length; i++)
            //{
            //    Data_Character parsingData = new Data_Character();
            //    string[] dataSegment = m_dataArray[i].Split("\t");

            //    parsingData.ID = int.Parse(dataSegment[0]);
            //    parsingData.name = dataSegment[1];
            //    parsingData.weapon = (eWeaponType)int.Parse(dataSegment[2]);
            //    parsingData.hp = float.Parse(dataSegment[3]);
            //    parsingData.damage = float.Parse(dataSegment[4]);
            //    parsingData.moveSpeed = float.Parse(dataSegment[5]);
            //    parsingData.armor = float.Parse(dataSegment[6]);
            //    parsingData.recoverHp = float.Parse(dataSegment[7]);
            //    parsingData.lucky = float.Parse(dataSegment[8]);
            //    parsingData.throwCount = float.Parse(dataSegment[9]);
            //    parsingData.throwSpeed = float.Parse(dataSegment[10]);
            //    parsingData.attackRange = float.Parse(dataSegment[11]);
            //    parsingData.attackDelay = float.Parse(dataSegment[12]);
            //    parsingData.attackDuration = float.Parse(dataSegment[13]);
            //    parsingData.bonusExp = float.Parse(dataSegment[14]);
            //    parsingData.bonusGold = float.Parse(dataSegment[15]);
            //    parsingData.avoidRate = float.Parse(dataSegment[16]);

            //    Global_Data.characterTable.Add(parsingData.ID, parsingData);
            //}
        }

        private void Convert_WeaponPropertyTable(string[] m_dataArray)
        {
            //Global_Data.weaponPropertyTable.Clear();

            //for (int i = 0; i < m_dataArray.Length; i++)
            //{
            //    Data_Weapon_Properties parsingData = new Data_Weapon_Properties();
            //    string[] dataSegment = m_dataArray[i].Split("\t");

            //    parsingData.ID = int.Parse(dataSegment[0]);
            //    parsingData.isTargeting = dataSegment[1] == "1" ? true : false;
            //    parsingData.isContinuous = dataSegment[2] == "1" ? true : false;
            //    parsingData.hasPostProcess = dataSegment[3] == "1" ? true : false;
            //    parsingData.isCollisionMonster = dataSegment[4] == "1" ? true : false;
            //    parsingData.isCollisionScreen = dataSegment[5] == "1" ? true : false;
            //    parsingData.hasReflection = dataSegment[6] == "1" ? true : false;
            //    parsingData.isNeedDir = dataSegment[7] == "1" ? true : false;
            //    parsingData.isControllableDir = dataSegment[8] == "1" ? true : false;
            //    parsingData.hasFlexiblePath = dataSegment[9] == "1" ? true : false;
            //    parsingData.hasSpecificStartPos = dataSegment[10] == "1" ? true : false;
            //    parsingData.isRotate = dataSegment[11] == "1" ? true : false;
            //    parsingData.isFalling = dataSegment[12] == "1" ? true : false;
            //    parsingData.isTargeting = dataSegment[13] == "1" ? true : false;

            //    Global_Data.weaponPropertyTable.Add(parsingData.ID, parsingData);
            //}
        }

        private void Convert_CommonTextTable(string[] m_dataArray)
        {
            //Logic_TextData.SetCommonText(m_dataArray);
        }

        private void Convert_BasicTextTable(string[] m_dataArray)
        {
            //Logic_TextData.SetBasicText(m_dataArray);
        }

        /// <summary>
        /// 범위 데이터를 사용하여 데이터의 총 크기를 구합니다.
        /// <br> 입력되는 데이터 양식은 A2:B10 같은 형태여야합니다. </br>
        /// </summary>
        private int GetCountData(string m_data, out int[] m_count)
        {
            string[] tempData = m_data.Split(":");
            m_count = new int[2];

            // 설명 :: A2:B10 데이터 A2부분의 숫자만을 추출하여 totalCount에 담습니다.
            m_count[0] = int.Parse(Regex.Replace(tempData[0], @"\D", ""));
            // 설명 :: A2:B10 데이터 중 B10 부분의 숫자만을 추출하여 기존 길이값을 빼서 총 길이값을 구합니다.
            m_count[1] = int.Parse(Regex.Replace(tempData[1], @"\D", ""));

            return m_count[1] - m_count[0];
        }
        #endregion
    }
}