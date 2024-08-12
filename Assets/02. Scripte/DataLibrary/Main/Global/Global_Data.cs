using IdleGame.Core.Unit;
using IdleGame.Data.DataTable;
using IdleGame.Data.EditorSetting;
using IdleGame.Data.NSave;
using IdleGame.Data.Numeric;
using IdleGame.Data.Option;
using System;
using System.Collections.Generic;

namespace IdleGame.Data
{
    /// <summary>
    /// [기능] 게임이 진행되면서 발생하는 모든 동적 데이터를 관리하고 접근가능하게 합니다. 
    /// </summary>
    public static class Global_Data
    {
        /// <summary>
        /// [데이터] Global_Data에서 취급되는 데이터 종류의 수입니다. 
        /// </summary>
        public static int DataCount = -1;

        /// <summary>
        /// [데이터] 세이브데이터의 가장 기본적인 데이터 구성들에대한 정보들을 담고 있습니다.
        /// </summary>
        public static Data_MainSave Main = new Data_MainSave();

        /// <summary>
        /// [데이터] 플레이어에 관련된 기본적인 정보들을 담고 있습니다. 대표적인것이 재화들이 있습니다.
        /// </summary>
        public static Data_Player Player = new Data_Player();

        /// <summary>
        /// [데이터] 플레이어가 지정한 게임에 관한 전반적인 설정입니다. 
        /// </summary>
        public static Data_Option Option = new Data_Option();

        /// <summary>
        /// [데이터] 플레이어가 게임을 진행하면서 이루어진 진행상황이나 상태 값 들을 담습니다. 
        /// </summary>
        public static Data_PlayProgress PlayProgress = new Data_PlayProgress();

        /// <summary>
        /// [데이터] 개발 과정에서 사용되어지는 에디터 설정입니다.
        /// </summary>
        public static Data_EditorSetting Editor = new Data_EditorSetting();

        /// <summary>
        /// [기능] 세이브 엔진에서 사용되어지는 기능입니다. 세이브 형태로 데이터를 변환하여 반환합니다.
        /// <br> 특정 데이터 타입이 추가 될경우 GetSaveDatas 와 SetSaveDatas 에 대응되는 데이터를 추가해주세요.</br>
        /// </summary>
        public static List<Interface_SaveData> GetSaveDatas()
        {
            List<Interface_SaveData> dataList = new List<Interface_SaveData>
            {
                Main,
                Player,
                Option,
                PlayProgress
            };

            DataCount = dataList.Count;

            return dataList;
        }

        /// <summary>
        /// [기능] Global_Data에서 저장가능한 형태로 취급하는 데이터들을 리스트로 변환하여 담습니다. 
        /// </summary>
        public static List<Type> GetTypeList()
        {
            List<Type> types = new List<Type>();
            foreach (var data in GetSaveDatas())
            {
                types.Add(data.GetType());
            }

            return types;
        }

        /// <summary>
        /// [기능] 세이브 엔진에서 사용되어지는 기능입니다. 로드한 데이터를 파싱합니다.
        /// <br> 특정 데이터 타입이 추가 될경우 GetSaveDatas 와 SetSaveDatas 에 대응되는 데이터를 추가해주세요.</br>
        /// </summary>
        public static void SetSaveDatas(List<Interface_SaveData> m_datas)
        {
            Main = m_datas[0] as Data_MainSave;
            Player = m_datas[1] as Data_Player;
            Option = m_datas[2] as Data_Option;
            PlayProgress = m_datas[3] as Data_PlayProgress;
        }

        /// <summary>
        /// [초기화] 처음 데이터가 활용된 경우 해당 기능을 통해서 초기 데이터를 할당해줍니다. 
        /// <br> 라이브러리가 초기화된 직후에 호출되어야합니다. </br>
        /// </summary>
        public static void FirstSetting()
        {
            Player.unit_Ability = (Data_UnitAbility)Library_DataTable.character[1000];
            foreach (eAbilityType type in Enum.GetValues(typeof(eAbilityType)))
            {
                if (type == eAbilityType.None)
                    continue;
                if (Player.slot_Ability.ContainsKey(type))
                    continue;

                Player.slot_Ability.Add(type, new Data_AbilitySlot()
                {
                    level = 1,
                    price = new ExactInt(0),
                    value = new ExactInt(0)
                });
            }
        }
    }
}