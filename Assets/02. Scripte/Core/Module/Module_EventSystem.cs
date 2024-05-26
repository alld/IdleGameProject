using IdleGame.Core.UnSafe;
using IdleGame.Data.Base;
using System;
using System.Collections.Generic;

namespace IdleGame.Core.Module.EventSystem
{
    /// <summary>
    /// [모듈] 코드간의 종속성을 제한하기위해서 마련된 이벤트 시스템입니다. 
    /// <br> 이벤트 단위로 게임의 흐름을 통제합니다. </br>
    /// </summary>
    public class Module_EventSystem<eEventType> : Base_Module where eEventType : unmanaged, Enum
    {
        /// <summary>
        /// [기능] 이벤트 타입별로 구성되는 이벤트 리스트입니다. 
        /// </summary>
        private struct EventList
        {
            /// <summary>
            /// [기능] 이벤트에 담겨있는 함수리스트입니다.
            /// </summary>
            private List<Delegate> _EventList;

            /// <summary>
            /// [기능] 이벤트에 해당하는 함수 리스트입니다.
            /// </summary>
            /// <returns></returns>
            public List<Delegate> GetEventList() => _EventList;

            /// <summary>
            /// [기능] 이벤트리스트에 함수를 추가합니다.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="func"></param>
            public void AddFUnction<T>(T func)
            {
                if (null == _EventList)
                {
                    _EventList = new List<Delegate>();
                }

                _EventList.Add(func as Delegate);
            }

            /// <summary>
            /// [기능] 이벤트리스트에서 특정 함수를 제거합니다. 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="func"></param>
            public void RemoveFunction<T>(T func)
            {
                if (null == _EventList)
                {
                    return;
                }
                _EventList.Remove(func as Delegate);
            }

            /// <summary>
            /// [검사] 이벤트가 비어있는지를 확인합니다. 
            /// </summary>
            /// <returns>true : 비어있음<br>false : 함수가 있음</br></returns>
            public bool IsEmpty()
            {
                if (null == _EventList)
                {
                    return true;
                }

                if (_EventList.Count <= 0) { return true; }

                return false;
            }
        }

        /// <summary>
        /// [데이터] 모든 이벤트가 담겨있는 이벤트 테이블입니다. 
        /// </summary>
        private Dictionary<int, EventList> _eventTable = null;

        /// <summary>
        /// [기능] 담겨있는 모든 이벤트를 지웁니다. 
        /// </summary>
        public void ClearEvent()
        {
            if (null != _eventTable)
            {
                _eventTable.Clear();
            }
        }

        /// <summary>
        /// [검사] 이벤트 테이블에 이벤트가 존재하는지를 확인합니다. 
        /// </summary>
        /// <returns>true : 비어있음<br>false : 이벤트가 존재함</br></returns>
        private bool IsEventTableNull()
        {

            if (null == _eventTable)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// [기능] 이벤트 타입에 알맞는 이벤트를 실행시킵니다.
        /// </summary>
        /// <param name="eventType"></param>
        public void CallEvent(eEventType eventType)
        {
            if (IsEventTableNull()) return;


            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (_eventTable.ContainsKey(parseType))
            {
                List<Delegate> eventList = _eventTable[parseType].GetEventList();

                foreach (Dele_EventFunc func in eventList)
                {
                    if (func.Target is UnityEngine.Object && func.Target.Equals(null)) continue;
                    func();
                }
            }

        }

        /// <summary>
        /// [기능] 이벤트 타입에 알맞는 이벤트를 실행시킵니다.
        /// </summary>
        public void CallEvent<T>(eEventType eventType, T param)
        {
            if (IsEventTableNull()) return;

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (_eventTable.ContainsKey(parseType))
            {
                List<Delegate> eventList = _eventTable[parseType].GetEventList();

                foreach (Dele_EventFunc<T> func in eventList)
                {
                    if (func.Target is UnityEngine.Object && func.Target.Equals(null)) continue;
                    func(param);
                }
            }

        }

        /// <summary>
        /// [기능] 이벤트 타입에 알맞는 이벤트를 실행시킵니다.
        /// </summary>
        public void CallEvent<T1, T2>(eEventType eventType, T1 param1, T2 param2)
        {
            if (IsEventTableNull()) return;

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (_eventTable.ContainsKey(parseType))
            {
                List<Delegate> eventList = _eventTable[parseType].GetEventList();

                foreach (Dele_EventFunc<T1, T2> func in eventList)
                {
                    if (func.Target is UnityEngine.Object && func.Target.Equals(null)) continue;
                    func(param1, param2);
                }
            }

        }

        /// <summary>
        /// [기능] 이벤트 타입에 알맞는 이벤트를 실행시킵니다.
        /// </summary>
        public void CallEvent<T1, T2, T3>(eEventType eventType, T1 param1, T2 param2, T3 param3)
        {
            if (IsEventTableNull()) return;

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (_eventTable.ContainsKey(parseType))
            {
                List<Delegate> eventList = _eventTable[parseType].GetEventList();

                foreach (Dele_EventFunc<T1, T2, T3> func in eventList)
                {
                    if (func.Target is UnityEngine.Object && func.Target.Equals(null)) continue;
                    func(param1, param2, param3);
                }
            }

        }

        /// <summary>
        /// [기능] 이벤트 테이블에 이벤트를 추가합니다. 
        /// </summary>
        public void RegisterEvent(eEventType eventType, Dele_EventFunc func)
        {
            if (null == _eventTable)
            {
                _eventTable = new Dictionary<int, EventList>();
            }

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (false == _eventTable.ContainsKey(parseType))
            {
                EventList list = new EventList();
                list.AddFUnction(func);

                _eventTable.Add(parseType, list);
            }
            else
            {
                _eventTable[parseType].AddFUnction(func);
            }
        }

        /// <summary>
        /// [기능] 이벤트 테이블에 이벤트를 추가합니다. 
        /// </summary>
        public void RegisterEvent<T>(eEventType eventType, Dele_EventFunc<T> func)
        {
            if (null == _eventTable)
            {
                _eventTable = new Dictionary<int, EventList>();
            }

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (false == _eventTable.ContainsKey(parseType))
            {
                EventList list = new EventList();
                list.AddFUnction(func);

                _eventTable.Add(parseType, list);
            }
            else
            {
                _eventTable[parseType].AddFUnction(func);
            }
        }

        /// <summary>
        /// [기능] 이벤트 테이블에 이벤트를 추가합니다. 
        /// </summary>
        public void RegisterEvent<T1, T2>(eEventType eventType, Dele_EventFunc<T1, T2> func)
        {
            if (null == _eventTable)
            {
                _eventTable = new Dictionary<int, EventList>();
            }

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (false == _eventTable.ContainsKey(parseType))
            {
                EventList list = new EventList();
                list.AddFUnction(func);

                _eventTable.Add(parseType, list);
            }
            else
            {
                _eventTable[parseType].AddFUnction(func);
            }
        }

        /// <summary>
        /// [기능] 이벤트 테이블에 이벤트를 추가합니다. 
        /// </summary>
        public void RegisterEvent<T1, T2, T3>(eEventType eventType, Dele_EventFunc<T1, T2, T3> func)
        {
            if (null == _eventTable)
            {
                _eventTable = new Dictionary<int, EventList>();
            }

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (false == _eventTable.ContainsKey(parseType))
            {
                EventList list = new EventList();
                list.AddFUnction(func);

                _eventTable.Add(parseType, list);
            }
            else
            {
                _eventTable[parseType].AddFUnction(func);
            }
        }

        /// <summary>
        /// [기능] 이벤트 테이블에서 특정 이벤트를 제거합니다. 
        /// </summary>
        public void RemoveEvent(eEventType eventType, Dele_EventFunc func)
        {
            if (null == _eventTable) { return; }

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (true == _eventTable.ContainsKey(parseType))
            {
                _eventTable[parseType].RemoveFunction(func);
            }
        }

        /// <summary>
        /// [기능] 이벤트 테이블에서 특정 이벤트를 제거합니다. 
        /// </summary>
        public void RemoveEvent<T>(eEventType eventType, Dele_EventFunc<T> func)
        {
            if (null == _eventTable) { return; }

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (true == _eventTable.ContainsKey(parseType))
            {
                _eventTable[parseType].RemoveFunction(func);
            }
        }

        /// <summary>
        /// [기능] 이벤트 테이블에서 특정 이벤트를 제거합니다. 
        /// </summary>
        public void RemoveEvent<T1, T2>(eEventType eventType, Dele_EventFunc<T1, T2> func)
        {
            if (null == _eventTable) { return; }

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (true == _eventTable.ContainsKey(parseType))
            {
                _eventTable[parseType].RemoveFunction(func);
            }
        }

        /// <summary>
        /// [기능] 이벤트 테이블에서 특정 이벤트를 제거합니다. 
        /// </summary>
        public void RemoveEvent<T1, T2, T3>(eEventType eventType, Dele_EventFunc<T1, T2, T3> func)
        {
            if (null == _eventTable) { return; }

            int parseType = Utility_EnumConvert.Enum32ToInt(eventType);

            if (true == _eventTable.ContainsKey(parseType))
            {
                _eventTable[parseType].RemoveFunction(func);
            }
        }
    }
}