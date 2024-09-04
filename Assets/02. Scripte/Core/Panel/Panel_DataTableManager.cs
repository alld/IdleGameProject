using IdleGame.Core.Module.DataTable;
using IdleGame.Core.Procedure;
using IdleGame.Core.Unit;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Common.Log;
using IdleGame.Data.DataTable;
using IdleGame.Data.Numeric;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


namespace IdleGame.Core.Panel.DataTable
{
    /// <summary>
    /// [기능] 데이터 테이블을 구성합니다. 
    /// </summary>
    public class Panel_DataTableManager : Base_ManagerPanel
    {
#if DevelopeMode
        [SerializeField]
        private Module_WebTableLoader _tableLoader = new Module_WebTableLoader();
#else
        [SerializeField]
        private Module_TSVConvert _tableLoader = new Module_TSVConvert();
#endif

        protected override void Logic_RegisterEvent_Custom()
        {
            Base_Engine.Event.RegisterEvent<eDataTableType, string[], int>(eGlobalEventType.Table_OnResponseData, OnResponseData);
            Base_Engine.Event.RegisterEvent(eGlobalEventType.Option_OnChangeLanguage, Logic_LoadData_TextTable);
        }

        /// <summary>
        /// [기능] 모든 데이터 테이블을 불러옵니다. 
        /// </summary>
        private void Logic_LoadAllData()
        {
            Logic_LoadData_FristInit();
            Logic_LoadData_TextTable();
        }

        /// <summary>
        /// [기능] 게임 전반적으로 구성에 필요한 데이터 테이블을 불러옵니다. 
        /// <br> 가장 먼저 초기화 되어 추가로 초기화가 이루어질 필요가 없는 데이터 테이블들로 구성됩니다. </br>
        /// </summary>
        private void Logic_LoadData_FristInit()
        {
            for (int i = 3; i < Library_DataTable.DataTableCount; i++)
            {
                Logic_TryLoadData((eDataTableType)i);
            }
        }


        /// <summary>
        /// [기능] 텍스트 테이블로부터 새로운 텍스트리스트를 받아옵니다.
        /// <br> 텍스트 테이블은 언어 변경에 따라 반복적으로 호출이 필요할 수 있습니다. </br>
        /// </summary>
        public void Logic_LoadData_TextTable()
        {
            Logic_TryLoadData(eDataTableType.ShareText);
            Logic_TryLoadData(eDataTableType.BasicText);
        }

        /// <summary>
        /// [기능] 테이블 로더기로부터 특정 타입의 데이터를 읽어오도록 시도합니다. 
        /// </summary>
        public void Logic_TryLoadData(eDataTableType m_kind)
        {
            StartCoroutine(_tableLoader.LoadData(m_kind));
        }

        private void OnResponseData(eDataTableType m_type, string[] m_dataArray, int m_errorCode)
        {
            if (m_errorCode < 0)
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log($"데이터를 불러오는데 실패하였습니다.\n {m_dataArray[0]}", Data_ErrorType.Error_DataLoadFailed, _tag.tag + m_type.ToString()));
                return;
            }
            try
            {
                switch (m_type)
                {
                    case eDataTableType.GameInfo:
                        Convert_GameInfo(m_dataArray);
                        Logic_LoadAllData();
                        break;
                    case eDataTableType.Stage:
                        Library_DataTable.stage.Clear();
                        var data_stage = Convert_TsvKeyParse<Data_Stage>(m_dataArray);
                        for (int i = 0; i < data_stage.Count; i++)
                            Library_DataTable.stage.Add(data_stage[i].stage_id, data_stage[i]);
                        //Convert_StageTable(m_dataArray);
                        break;
                    case eDataTableType.Monster:
                        Library_DataTable.monster.Clear();
                        var data_monster = Convert_TsvKeyParse<Data_Monster>(m_dataArray);
                        for (int i = 0; i < data_monster.Count; i++)
                            Library_DataTable.monster.Add(data_monster[i].monster_id, data_monster[i]);
                        //Convert_MonsterTable(m_dataArray);
                        break;
                    case eDataTableType.Quest:
                        Library_DataTable.quest.Clear();
                        var data_quest = Convert_TsvKeyParse<Data_Quest>(m_dataArray);
                        for (int i = 0; i < data_quest.Count; i++)
                            Library_DataTable.quest.Add(data_quest[i].index, data_quest[i]);
                        //Convert_QuestTable(m_dataArray);
                        break;
                    case eDataTableType.Character:
                        Library_DataTable.character.Clear();
                        var data_char = Convert_TsvKeyParse<Data_Character>(m_dataArray);
                        for (int i = 0; i < data_char.Count; i++)
                            Library_DataTable.character.Add(data_char[i].character_id, data_char[i]);
                        //Convert_CharacterTable(m_dataArray);
                        break;
                    case eDataTableType.Item:
                        Library_DataTable.item.Clear();
                        var data_item = Convert_TsvKeyParse<Data_Item>(m_dataArray);
                        for (int i = 0; i < data_item.Count; i++)
                            Library_DataTable.item.Add(data_item[i].index, data_item[i]);
                        //Convert_ItemTable(m_dataArray);
                        break;
                    case eDataTableType.Skill:
                        Library_DataTable.skill.Clear();
                        var data_skill = Convert_TsvKeyParse<Data_Skill>(m_dataArray);
                        for (int i = 0; i < data_skill.Count; i++)
                            Library_DataTable.skill.Add(data_skill[i].index, data_skill[i]);
                        Convert_SkillTable(m_dataArray);
                        break;
                    case eDataTableType.Property:
                        Convert_ProperyTable(m_dataArray);
                        break;
                    case eDataTableType.AbilitySlot_Hp:
                    case eDataTableType.AbilitySlot_Damage:
                    case eDataTableType.AbilitySlot_HpRegen:
                    case eDataTableType.AbilitySlot_CriticalChance:
                    case eDataTableType.AbilitySlot_CriticalMultiplier:
                        Convert_AbilitySlotTable(m_dataArray, m_type);
                        break;
                    case eDataTableType.ShareText:
                        Convert_CommonTextTable(m_dataArray);
                        Global_TextData.OnChangeLanguage();
                        break;
                    case eDataTableType.BasicText:
                        Convert_BasicTextTable(m_dataArray);
                        Global_TextData.OnChangeLanguage();
                        break;
                    default:
                        Base_Engine.Log.Logic_PutLog(new Data_Log($"미 할당된 데이터 로드를 시도하였습니다.\n {m_type}", Data_ErrorType.Error_DataLoadFailed, _tag.tag));
                        break;
                }
            }
            catch (Exception e)
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log(e.ToString(), Data_ErrorType.Error_DataParsingFailed, _tag.tag));
            }
            finally
            {
                Base_Engine.Event.CallEvent(eGlobalEventType.Save_OnResponseStep);
            }

        }

        #region 데이터 파싱
        private void Convert_GameInfo(string[] m_dataArray)
        {
            Library_DataTable.Info.isDataExists = true;
            Library_DataTable.Info.version = m_dataArray[0].Split("\t")[0];

            for (int i = 1; i < m_dataArray.Length; i++)
            {
                string[] resultData = m_dataArray[i].Split("\t");
                if (string.IsNullOrEmpty(resultData[0]))
                    break;
                Library_DataTable.Info.dataTableList.Add((eDataTableType)(i), (Convert_ReplaceGid(resultData[0], resultData[Global_Data.Editor.LocalData_Grid + 2]), resultData[1]));
            }
        }

        /// <summary>
        /// [변환] 주소값에서 그리드값을 변경합니다.  
        /// </summary>
        static string Convert_ReplaceGid(string m_input, string m_newGid)
        {
            if (string.IsNullOrEmpty(m_newGid))
                return m_input;

            int startIndex = m_input.IndexOf("gid=") + 4;
            int endIndex = m_input.IndexOf("&", startIndex);

            if (endIndex == -1)
            {
                endIndex = m_input.Length;
            }

            string replaced = m_input.Substring(0, startIndex) + m_newGid + m_input.Substring(endIndex);
            return replaced;
        }

        /// <summary>
        /// [변환] 키값을 기준으로 파싱합니다.
        /// </summary>
        public List<T> Convert_TsvKeyParse<T>(string[] m_dataArray) where T : new()
        {
            var dataList = new List<T>();
            var fields = typeof(T).GetFields();

            var headers = m_dataArray[0].Split('\t');

            for (int j = 1; j < m_dataArray.Length; j++)
            {
                var values = m_dataArray[j].Split('\t');

                var obj = new T();

                for (int i = 0; i < headers.Length; i++)
                {
                    var header = headers[i].Trim();
                    var value = values[i].Trim().AsSpan();

                    var field = fields.FirstOrDefault(f => f.Name.Equals(header, StringComparison.OrdinalIgnoreCase));
                    try
                    {
                        if (field != null)
                        {
                            if (field.FieldType == typeof(ExactInt))
                            {
                                field.SetValue(obj, new ExactInt(new string(value)));
                            }
                            else if (field.FieldType.IsEnum)
                            {
                                var numberString = new string(value).Split('.')[0]; // 첫 번째 부분을 가져옴
                                if (int.TryParse(numberString, out int enumValue))
                                {
                                    if (Enum.IsDefined(field.FieldType, enumValue))
                                        field.SetValue(obj, Enum.ToObject(field.FieldType, enumValue));
                                    else
                                        throw new InvalidOperationException($"Unsupported enum type: {enumValue}");
                                }
                            }
                            else
                                switch (Type.GetTypeCode(field.FieldType))
                                {
                                    case TypeCode.Int32:
                                        field.SetValue(obj, int.Parse(new string(value)));
                                        break;
                                    case TypeCode.Int64:
                                        field.SetValue(obj, long.Parse(new string(value)));
                                        break;
                                    case TypeCode.Boolean:
                                        field.SetValue(obj, bool.Parse(new string(value)));
                                        break;
                                    case TypeCode.Single:
                                        field.SetValue(obj, float.Parse(new string(value)));
                                        break;
                                    case TypeCode.Double:
                                        field.SetValue(obj, double.Parse(new string(value)));
                                        break;
                                    case TypeCode.String:
                                        field.SetValue(obj, new string(value));
                                        break;
                                    case TypeCode.Object when field.FieldType.IsArray: // 배열 처리
                                        var arrayValues = new string(value).Split(','); // 콤마로 나누기
                                        var arrayType = field.FieldType.GetElementType(); // 배열의 요소 타입 가져오기

                                        Array array = Array.CreateInstance(arrayType, arrayValues.Length); // 배열 생성

                                        for (int k = 0; k < arrayValues.Length; k++)
                                        {
                                            switch (Type.GetTypeCode(arrayType))
                                            {
                                                case TypeCode.Int32:
                                                    array.SetValue(int.Parse(arrayValues[k]), k);
                                                    break;
                                                case TypeCode.Single: // float 처리
                                                    array.SetValue(float.Parse(arrayValues[k]), k);
                                                    break;
                                                case TypeCode.Int64:
                                                    array.SetValue(long.Parse(arrayValues[k]), k);
                                                    break;
                                                case TypeCode.Boolean:
                                                    array.SetValue(bool.Parse(arrayValues[k]), k);
                                                    break;
                                                case TypeCode.Double:
                                                    array.SetValue(double.Parse(arrayValues[k]), k);
                                                    break;
                                                case TypeCode.String:
                                                    array.SetValue(arrayValues[k], k);
                                                    break;
                                                // 다른 타입에 대한 처리도 추가 가능
                                                default:
                                                    throw new InvalidOperationException($"Unsupported array element type: {arrayType}");
                                            }
                                        }

                                        field.SetValue(obj, array);
                                        break;

                                    // 필요한 경우 다른 타입을 추가할 수 있습니다.
                                    default:
                                        throw new InvalidOperationException($"Unsupported field type: {field.FieldType}");
                                }
                        }
                    }
                    catch (Exception e)
                    {
                        Base_Engine.Log.Logic_PutLog(new Data_Log(Data_ErrorType.Error_DataParsingFailed, $"name : {field.Name}\ntype : {field.FieldType}, data : {new string(value)}\n {e.ToString()}", _tag));
                        continue;
                    }
                }

                dataList.Add(obj);

            }

            return dataList;
        }

        /// <summary>
        /// [변환] 데이터 리스트에서, 스테이지에 대한 데이터를 파싱합니다. 
        /// </summary>
        private void Convert_StageTable(string[] m_dataArray)
        {
            Library_DataTable.stage.Clear();

            for (int i = 0; i < m_dataArray.Length; i++)
            {
                int index = 0;
                Data_Stage parsingData = new Data_Stage();
                string[] dataSegment = m_dataArray[i].Split("\t");

                Convert_ParsingData(ref parsingData.index, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.stage_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.next_stage, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.wave_num, dataSegment[index++]);
                //Convert_ParsingData(ref parsingData.stage_effect, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.story, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.story_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.background_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.monster_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.monster_num, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.monster_max, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.boss_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.boss_battletime, dataSegment[index++]);

                Library_DataTable.stage.Add(parsingData.stage_id, parsingData);
            }
        }

        /// <summary>
        /// [변환] 데이터 리스트에서, 몬스터 테이블에 대한 데이터를 파싱합니다. 
        /// </summary>
        private void Convert_MonsterTable(string[] m_dataArray)
        {
            Library_DataTable.monster.Clear();

            for (int i = 0; i < m_dataArray.Length; i++)
            {
                int index = 0;
                Data_Monster parsingData = new Data_Monster();
                string[] dataSegment = m_dataArray[i].Split("\t");

                Convert_ParsingData(ref parsingData.idx, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.monster_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.monster_name, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.monster_type, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.mon_shape_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.level, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.defense, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.speed, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.mon_max_hp, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.attack_range, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.attack_speed, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.mon_attack, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.attack_skill, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.experience_reward, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.gold_reward, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.colleague_reward, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.proprty, dataSegment[index++]);

                Library_DataTable.monster.Add(parsingData.monster_id, parsingData);
            }
        }

        /// <summary>
        /// [변환] 데이터 리스트에서, 퀘스트에 대한 데이터를 파싱합니다. 
        /// </summary>
        private void Convert_QuestTable(string[] m_dataArray)
        {
            Library_DataTable.quest.Clear();

            for (int i = 0; i < m_dataArray.Length; i++)
            {
                int index = 0;
                Data_Quest parsingData = new Data_Quest();
                string[] dataSegment = m_dataArray[i].Split("\t");

                Convert_ParsingData(ref parsingData.index, dataSegment[index++]);

                Library_DataTable.quest.Add(parsingData.index, parsingData);
            }
        }

        /// <summary>
        /// [변환] 데이터 리스트에서, 캐릭터에 대한 데이터를 파싱합니다. 
        /// </summary>
        private void Convert_CharacterTable(string[] m_dataArray)
        {
            Library_DataTable.character.Clear();

            for (int i = 0; i < m_dataArray.Length; i++)
            {
                int index = 0;
                Data_Character parsingData = new Data_Character();
                string[] dataSegment = m_dataArray[i].Split("\t");

                Convert_ParsingData(ref parsingData.idx, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.player_value, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.defens, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.character_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.shape_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.stage_id, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.damage, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.hp, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.speed, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.critical_chance, dataSegment[index++]);
                parsingData.critical_chance /= 100;
                Convert_ParsingData(ref parsingData.critical_strike_rate, dataSegment[index++]);
                parsingData.critical_strike_rate /= 100;
                Convert_ParsingData(ref parsingData.attack_speed, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.attack_range, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.skill, dataSegment[index++]);
                Convert_ParsingData(ref parsingData.effect, dataSegment[index++]);

                Library_DataTable.character.Add(parsingData.character_id, parsingData);
            }
        }

        /// <summary>
        /// [변환] 데이터 리스트에서, 아이템에 대한 데이터를 파싱합니다. 
        /// </summary>
        private void Convert_ItemTable(string[] m_dataArray)
        {
            Library_DataTable.item.Clear();

            for (int i = 0; i < m_dataArray.Length; i++)
            {
                int index = 0;
                Data_Item parsingData = new Data_Item();
                string[] dataSegment = m_dataArray[i].Split("\t");

                Convert_ParsingData(ref parsingData.index, dataSegment[index++]);

                Library_DataTable.item.Add(parsingData.index, parsingData);
            }
        }

        /// <summary>
        /// [변환] 데이터 리스트에서, 스킬에 대한 데이터를 파싱합니다. 
        /// </summary>
        private void Convert_SkillTable(string[] m_dataArray)
        {
            Library_DataTable.skill.Clear();

            for (int i = 0; i < m_dataArray.Length; i++)
            {
                int index = 0;
                Data_Skill parsingData = new Data_Skill();
                string[] dataSegment = m_dataArray[i].Split("\t");

                Convert_ParsingData(ref parsingData.index, dataSegment[index++]);

                Library_DataTable.skill.Add(parsingData.index, parsingData);
            }
        }

        /// <summary>
        /// [변환] 데이터 리스트에서, 속성에 대한 데이터를 파싱합니다. 
        /// </summary>
        private void Convert_ProperyTable(string[] m_dataArray)
        {
            Library_DataTable.property.Clear();

            for (int i = 0; i < m_dataArray.Length; i++)
            {
                string[] dataSegment = m_dataArray[i].Split("\t");

                for (int j = 0; j < dataSegment.Length; j++)
                {
                    Library_DataTable.property.Add(((eUnitProperty)i, (eUnitProperty)j), int.Parse(dataSegment[j]));
                }

            }
        }

        /// <summary>
        /// [변환] 데이터 리스트에서, 능력 상승 테이블에 대한 데이터를 파싱합니다. 
        /// </summary>
        private void Convert_AbilitySlotTable(string[] m_dataArray, eDataTableType m_type)
        {
            eAbilityType dataType = eAbilityType.None;
            switch (m_type)
            {
                case eDataTableType.AbilitySlot_Hp:
                    dataType = eAbilityType.Hp;
                    break;
                case eDataTableType.AbilitySlot_Damage:
                    dataType = eAbilityType.Damage;
                    break;
                case eDataTableType.AbilitySlot_HpRegen:
                    dataType = eAbilityType.HpRegen;
                    break;
                case eDataTableType.AbilitySlot_CriticalChance:
                    dataType = eAbilityType.CriticalChance;
                    break;
                case eDataTableType.AbilitySlot_CriticalMultiplier:
                    dataType = eAbilityType.CriticalMultiplier;
                    break;
            }

            if (Library_DataTable.abilitySlot.ContainsKey(dataType))
                Library_DataTable.abilitySlot.Remove(dataType);

            Data_AbilitySlotInfo[] parsingData = new Data_AbilitySlotInfo[m_dataArray.Length];
            for (int i = 0; i < m_dataArray.Length; i++)
            {
                int index = 0;
                string[] dataSegment = m_dataArray[i].Split("\t");

                Convert_ParsingData(ref parsingData[i].idx, dataSegment[index++]);
                Convert_ParsingData(ref parsingData[i].level, dataSegment[index++]);
                switch (dataType)
                {
                    case eAbilityType.Hp:
                    case eAbilityType.Damage:
                    case eAbilityType.HpRegen:
                    case eAbilityType.CriticalMultiplier:
                        Convert_ParsingData(ref parsingData[i].value_exact, dataSegment[index++]);
                        break;
                    case eAbilityType.CriticalChance:
                        Convert_ParsingData(ref parsingData[i].value_float, dataSegment[index++]);
                        parsingData[i].value_float /= 100;
                        break;
                }
                Convert_ParsingData(ref parsingData[i].price, dataSegment[index++]);
            }
            Library_DataTable.abilitySlot.Add(dataType, parsingData);
        }

        /// <summary>
        /// [변환] 커스텀 숫자를 적절하게 파싱해줍니다.
        /// </summary>
        private void Convert_ParsingData(ref ExactInt m_parsingData, string m_dataSegment)
        {
            if (string.IsNullOrEmpty(m_dataSegment)) return;

            m_parsingData = ExactInt.Parse(m_dataSegment);
        }

        /// <summary>
        /// [변환] 값을 적절하게 파싱해줍니다.
        /// </summary>
        private void Convert_ParsingData<T>(ref T m_parsingData, string m_dataSegment)
        {
            if (string.IsNullOrEmpty(m_dataSegment)) return;

            m_parsingData = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(m_dataSegment);
        }

        /// <summary>
        /// [변환] 1차 배열의 값을 적절하게 파싱해줍니다.
        /// </summary>
        private void Convert_ParsingData<T>(ref T[] m_parsingData, string m_dataSegment)
        {
            if (string.IsNullOrEmpty(m_dataSegment)) return;

            string[] arrayData = m_dataSegment.Split(",");
            m_parsingData = new T[arrayData.Length];
            for (int i = 0; i < arrayData.Length; i++)
            {
                m_parsingData[i] = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(arrayData[i]);
            }
        }

        /// <summary>
        /// [변환] 2차 배열의 값을 적절하게 파싱해줍니다.
        /// </summary>
        private void Convert_ParsingData<T>(ref T[][] m_parsingData, string m_dataSegment)
        {
            if (string.IsNullOrEmpty(m_dataSegment)) return;

            string[] arrayData = m_dataSegment.Split("//");
            m_parsingData = new T[arrayData.Length][];
            for (int i = 0; i < arrayData.Length; i++)
            {
                string[] arrayData_N = arrayData[i].Split(",");
                m_parsingData[i] = new T[arrayData_N.Length];
                for (int j = 0; j < arrayData_N.Length; j++)
                {
                    m_parsingData[i][j] = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(arrayData_N[j]);
                }
            }
        }

        private void Convert_CommonTextTable(string[] m_dataArray)
        {
            Global_TextData.SetCommonText(m_dataArray);
        }

        private void Convert_BasicTextTable(string[] m_dataArray)
        {
            Global_TextData.SetBasicText(m_dataArray);
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