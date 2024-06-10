using DG.DemiEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace IdleGame.Data.Numeric
{
    static class DataLimit
    {
        public const int Int = 10000;
    }

    struct IntData
    {
        List<int> data;
        string dataSize;
        bool isPositive;

        public static IntData operator+(IntData a, IntData b)
        {
            if (a.isPositive != b.isPositive)
            {
                
            }
            
            if (CalculateDataSize(a.dataSize) < CalculateDataSize(b.dataSize))
            {
                b.Add(a);
                return b;
            }
            
            a.Add(b);
            return a;
        }

        public void Add(IntData addData)
        {
            // Todo : pos + pos / pos + neg / neg + pos / neg + neg 
            
            int lastIndex = addData.data.Count;
            int carry = 0;
            
            for (int i = 0; i < lastIndex; ++i)
            {
                data[i] += carry + addData.data[i];
                carry = data[i] / DataLimit.Int;
                data[i] %= DataLimit.Int;
            }
            if (carry > 0) data.Add(carry);
        }

        public int GetDataSize()
        {
            return data.Count;
        }

        static int CalculateDataSize(string _dataSize)
        {
            // _dataSize == "ba"

            _dataSize = _dataSize.ToLower();

            int num = 0;

            for (int i = 0; i < _dataSize.Length; ++i)
            {
                num *= 26;
                num += _dataSize[i] - 'a' + 1;
                
                // Debug.Log(_dataSize[i] + " : " + num);
            }
            
            // Debug.Log("dataSize : " + num);

            return num;
        }
        
        public IntData(int _data, string _dataSize = "")
        {
            data = new List<int>();
            dataSize = _dataSize;
            if (_dataSize.Length > 0)
            {
                data = Enumerable.Repeat(0, CalculateDataSize(_dataSize)).ToList();
            }

            while (_data >= DataLimit.Int)
            {
                data.Add(_data % DataLimit.Int);
                _data /= DataLimit.Int;
            }

            if (_data < 0)
            {
                isPositive = false;
                _data *= -1;
            }
            else
            {
                isPositive = true;
            }
            data.Add(_data);
        }

        public IntData(IntData refIntData)
        {
            data = new List<int>(refIntData.data);
            dataSize = refIntData.dataSize;
            isPositive = refIntData.isPositive;
        }

        public override string ToString()
        {
            string result = "";

            int dataSize = data.Count;

            for (int i = dataSize - 1; i >= 0; --i)
            {
                string current = data[i].ToString();

                while (i < dataSize - 1 && current.Length < 4)
                {
                    current = '0' + current;
                }

                result += current + ' ';
            }

            return result;
        }
    }
    
    /* 20240609 Data Test
    interface IDataNode<T>
    {
        public T data { get; set; }

        public string GetDataSize();
    }

    [System.Serializable]
    struct IntData : IDataNode<int>
    {
        public int data { get; set; }
        public IDataNode<int> prev { get; set; }
        public IDataNode<int> next { get; set; }
        public string dataSize { get; set; }
        public string GetDataSize()
        {
            return dataSize;
        }
        
        public static string GetNextDataSize(string prevDataSize)
        {
            if (prevDataSize.IsNullOrEmpty())
            {
                return "a";
            }

            int lastPos = prevDataSize.Length - 1;
            System.Text.StringBuilder sb = new StringBuilder(prevDataSize);
            sb[lastPos]++;

            while (sb[lastPos] > 'z')
            {
                sb[lastPos--] = 'a';
                if (lastPos < 0)
                {
                    sb.Insert(0, 'a');
                }
                else
                {
                    sb[lastPos]++;
                }
            }

            return sb.ToString();
        }
        public IntData(int _data, IDataNode<int> _prev = null)
        {
            next = null;
            prev = _prev;
            while (_data >= DataLimit.Int)
            {
                prev = new IntData(_data % DataLimit.Int, prev);
                _data /= DataLimit.Int;
            }
            data = _data;
            if (prev == null)
            {
                dataSize = "";
            }
            else
            {
                dataSize = GetNextDataSize(prev.GetDataSize());
            }
        }

        public static IntData operator+(IntData A, IntData B)
        {
            
            return A;
        }

        public override string ToString()
        {
            string result = data.ToString() + dataSize;
            if (prev != null)
            {
                result += prev.ToString();
            }

            return result;
        }
    }
    */
    
    
    /* 240607 Linked List Node Struct Test
    {
    
    struct IntNode : IDataNode
    {
        public IDataNode upper;
        public IDataNode lower;
        public int data;
        public string dataSize;

        public IntNode(int _data, IDataNode _lower = null)
        {
            lower = _lower;
            while (_data >= DataLimit.Int)
            {
                lower = new IntNode(_data % DataLimit.Int, lower);
                _data /= DataLimit.Int;
            }
            
            data = _data;
            upper = null;
            
            if (lower == null)
            {
                dataSize = "";
                return;
            }

            
            if (lower.GetDataSize().IsNullOrEmpty())
            {
                dataSize = "a";
            }
            else
            {
                // 하위 단위의 마지막 문자에 따라 현재 단위문자 초기화 
                System.Text.StringBuilder sb = new System.Text.StringBuilder(lower.GetDataSize());
                if (sb[sb.Length - 1] == 'z')
                {
                    sb[sb.Length - 1] = 'a';
                    sb.Insert(0, 'a');
                }
                else
                {
                    sb[sb.Length - 1]++;
                }
            
                dataSize = sb.ToString();
            }
            lower.SetUpper(this);
        }

        static bool IsBiggerSize(string A, string B)
        {
            if (A.Length < B.Length) return false;

            if (A.Length > B.Length) return true;

            for (int i = A.Length - 1; i >= 0; --i)
            {
                if (A[i] < B[i]) return false;
            }
            
            return true;
        }

        public static int CalcDist(string a, string b)
        {
            int resultA = 0;

            for (int i = 0; i < a.Length; ++i)
            {
                resultA *= 26;
                resultA += a[i] - 'a';
            }

            int resultB = 0;

            for (int i = 0; i < b.Length; ++i)
            {
                resultB *= 26;
                resultB += b[i] - 'a';
            }

            return resultA - resultB;
        }
        
        public static IntNode operator+(IntNode a, IntNode b)
        {   
            string aDataSize = a.GetDataSize();
            string bDataSize = b.GetDataSize();

            int sizeDist = CalcDist(aDataSize, bDataSize);
            if (sizeDist < 0)
            {
                // Swap
                (a, b) = (b, a);
                sizeDist *= -1;
            }

            a.GetLower();
            
            return a;
        }

        
        public override string ToString()
        {
            string dataString = data.ToString() + dataSize;

            if (lower != null)
            {
                dataString += lower.ToString();
            }

            return dataString;
        }

        public void SetUpper(IDataNode _upper)
        {
            upper = _upper;
        }

        public void SetLower(IDataNode _lower)
        {
            lower = _lower;
        }

        public ref IDataNode GetUpper()
        {
            return ref upper;
        }

        public ref IDataNode GetLower()
        {
            return ref lower;
        }

        public string GetDataSize()
        {
            return dataSize;
        }
    }
    }
    */
    
}