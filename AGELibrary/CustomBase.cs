using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AGELibrary
{
    /// <summary>
    /// 自定义基类
    /// </summary>
    public class CustomBase
    {

    }

    /// <summary>
    /// 基于String类复写 具备可嵌套、可复性的字符型对象、集合
    /// </summary>
    public sealed class NewStrObject : IList, IComparable, ICloneable, IEnumerable, IComparable<String>, IEnumerable<char>, IEquatable<String>
    {
        private static String _str = "";
        private static NewStrObject _nstr = "";

        public static String str
        {
            get
            {
                if (!_str.Equals(""))
                {
                    return _str;
                }
                else
                {
                    if (_List.Count == 1)
                    {
                        return _List[0].ToString();
                    }
                    else
                    {
                        if (!_nstr.Equals(""))
                        {
                            return _nstr;
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            set
            {
                _str = value;
            }
        }
        private static String[] strs = null;
        public String[] StringArray
        {
            get
            {
                if (strs.Length > 0)
                {
                    return strs;
                }
                else if (nstrs.Length > 0)
                {
                    nstrs.ToArray().CopyTo(strs, 0);
                }
                else if (_List.Count > 0)
                {
                    _List.Select(p => (String)p).ToList().CopyTo(strs);
                }
                return strs;
            }

        }
        private static List<object> _List = null;
        public List<object> List
        {
            get
            {
                if (_List.Count > 0)
                {
                    return _List;
                }
                else if (nstrs.Length > 0)
                {
                    foreach (NewStrObject nso in nstrs) _List.Add(nso);
                }
                else if (strs.Length > 0)
                {
                    foreach (String s in strs) _List.Add(s);
                }
                return _List;
            }

        }
        //private static NewStrObject _nstr = null;

        private static NewStrObject[] nstrs = null;
        public NewStrObject[] NewStringArray
        {
            get
            {
                if (nstrs.Length > 0)
                {
                    return nstrs;
                }
                else if (strs.Length > 0)
                {
                    strs.ToArray().CopyTo(nstrs, 0);
                }
                else if (_List.Count > 0)
                {
                    _List.Select(p => (NewStrObject)p).ToList().CopyTo(nstrs);
                }
                return nstrs;
            }

        }

        public int Length
        {
            get => _str.Length;
        }

        private static object _SyncRoot = null;

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public int Count => nstrs.Length > 0 ? nstrs.Length : 1;

        public object SyncRoot => _SyncRoot;

        public bool IsSynchronized => true;

        public object this[int index]
        {
            get => (Count > 0 && index > 0) ? this[index] : this;
            set => throw new Exception("IsReadOnly");
        }

        public bool IsSingleton
        {
            get
            {
                if (_List.Count <= 1 && _nstr != null && !_nstr.Equals(""))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsArray
        {
            get
            {
                if (_List.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
        }

        public NewStrObject()
        {

        }
        public NewStrObject(string v)
        {
            _nstr = _str = v;
        }
        public NewStrObject(NewStrObject v)
        {
            _nstr = _str = v;
        }
        public NewStrObject(string[] v)
        {
            strs = v;
        }

        public NewStrObject(NewStrObject[] v)
        {
            nstrs = v;
        }

        /// <summary>
        /// 字符串类型
        /// </summary>
        public enum NewStrObjectType
        {
            [Description("字符串")]
            String = 0,
            [Description("日期时间")]
            DateTime = 1,
            [Description("文件路径")]
            Path = 2,
            [Description("网络资源地址")]
            URI = 3
        }


        public NewStrObject(NewStrObjectType nsoType, params string[] v)
        {
            string _s = "";
            switch (nsoType)
            {
                case NewStrObjectType.String:

                    foreach (string s in v)
                    {
                        _s += s;
                    }

                    break;
                case NewStrObjectType.DateTime:
                    string sDateTime = "";
                    switch (v.Length)
                    {
                        case 2:
                            sDateTime = String.Format("{0:G} {0:G}", v[0], v[1]);

                            break;
                        case 3:
                            sDateTime = String.Format("{0:G}-{0:G}-{0:G}", v[0], v[1], v[2]);
                            break;
                        case 6:
                            sDateTime = String.Format("{0:G}-{0:G}-{0:G} {0:G}:{0:G}:{0:G}", v[0], v[1], v[2], v[3], v[4], v[5]);
                            break;
                        default:
                            break;
                    }
                    try
                    {
                        if (DateTime.TryParse(sDateTime, out DateTime dateTime))
                        {
                            _s = dateTime.ToString();
                        }
                        else
                        {
                            _s = new NewStrObject(NewStrObjectType.String, v);
                        }
                    }
                    catch
                    {
                        _s = new NewStrObject(NewStrObjectType.String, v);
                    }
                    break;
                case NewStrObjectType.Path:
                    _s = Path.Combine(v);
                    break;
                case NewStrObjectType.URI:
                    foreach (string s in v)
                    {
                        _s += s;
                    }

                    Regex reg = new Regex("//");
                    _s = reg.Replace(_s, "/").Replace("http:/", "http://").Replace("https:/", "https://");
                    break;
            }

            _nstr = _str = _s;
        }


        public override bool Equals(object obj)
        {
            return _str.Equals(obj);
        }
        public bool Equals<T>(T obj)
        {
            if (typeof(T).ToString().ToLower().Contains("string"))
            {
                return _str.Equals(obj);
            }
            else if (typeof(T).ToString().ToLower().Contains("newstringobject"))
            {
                return _nstr.Equals(obj);
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return _str.GetHashCode();
        }

        public override string ToString()
        {
            return _str;
        }

        public static implicit operator NewStrObject(string v)
        {
            str = v;
            return str;
        }

        public static implicit operator string(NewStrObject v)
        {
            _str = v;
            return _str;
        }

        //public static implicit operator NewString[] (string[] v)
        //{
        //    strs = v;
        //    foreach (string s in strs)
        //    {
        //        list.Add(s);
        //    }
        //    NewString[] nstrss = null;
        //    list.CopyTo(nstrss);
        //    return nstrs;
        //}


        public NewStrObject ToNewStrObject()
        {
            if (_nstr == null)
            {
                _nstr = _str;
            }
            if (!_List.Contains(_nstr))
            {
                _List.Add(_nstr);
            }
            return _nstr;
        }



        public NewStrObject Append(params string[] v)
        {
            string _s = "";
            foreach (string s in v)
            {
                _s += s;
            }
            _str += _s;
            _nstr = _str;
            return _nstr;
        }

        public static NewStrObject ToNewStrObject(string v)
        {
            _nstr = v;
            return _nstr;
        }

        public static NewStrObject[] ToNewStrObject(string[] v)
        {
            strs = v;
            foreach (string s in v)
            {
                _List.Add(s);
            }

            _List.Select(p => (NewStrObject)p).ToList().CopyTo(nstrs);
            return nstrs;
        }

        public NewStrObject Join(char split = ',')
        {
            NewStrObject s = "";
            foreach (NewStrObject ns in strs)
            {
                s += split + ns.ToString();
            }
            return s.Substring(1);
        }
        public NewStrObject Join(NewStrObject[] v, char split = ',')
        {
            NewStrObject s = "";
            foreach (NewStrObject ns in v)
            {
                s += split + ns.ToString();
            }
            return s.Substring(1);
        }

        public static String[] ToStringItemArray(NewStrObject[] _nstr)
        {
            foreach (string s in _nstr)
            {
                _List.Add(s);
            }

            _List.Select(p => (String)p).ToList().CopyTo(strs);
            return strs;
        }

        public NewStrObject[] Split(NewStrObject _nstr, char split = ',')
        {
            _str = _nstr;
            strs = _str.Split(split);
            foreach (string s in strs)
            {
                _List.Add(s);
            }

            _List.Select(p => (NewStrObject)p).ToList().CopyTo(nstrs);
            return nstrs;
        }
        public NewStrObject[] Split(String _str, char split = ',')
        {
            str = _str;
            strs = _str.Split(split);
            foreach (string s in strs)
            {
                _List.Add(s);
            }

            _List.Select(p => (NewStrObject)p).ToList().CopyTo(nstrs);
            return nstrs;
        }

        public int CompareTo(object obj)
        {
            int n = 0;
            if (_str.ToString() == obj.ToString())
                n++;
            if (_nstr.ToString() == obj.ToString())
                n++;

            return n;
        }

        public object Clone()
        {
            return _str.Clone();
        }

        public TypeCode GetTypeCode()
        {
            return _str.GetTypeCode();
        }

        #region override string
        public NewStrObject ToLower()
        {
            return _str.ToLower();
        }
        public NewStrObject ToUpper()
        {
            return _str.ToUpper();
        }
        public NewStrObject Trim(params char[] trimChars)
        {
            return _str.Trim(trimChars);
        }
        public NewStrObject ToUpper(params char[] trimChars)
        {
            return _str.TrimEnd(trimChars);
        }
        public NewStrObject TrimStart(params char[] trimChars)
        {
            return _str.TrimStart(trimChars);
        }
        public bool StartsWith(String value)
        {
            return NewStrObject._str.StartsWith(value);
        }
        public bool StartsWith(String value, bool ignoreCase, CultureInfo culture)
        {
            return NewStrObject._str.StartsWith(value, ignoreCase, culture);
        }
        public NewStrObject Substring(int startIndex, int length)
        {
            return _str.Substring(startIndex, length);
        }
        public NewStrObject Substring(int startIndex)
        {
            return _str.Substring(startIndex);
        }
        public char[] ToCharArray(int startIndex, int length)
        {
            return _str.ToCharArray(startIndex, length);
        }
        public char[] ToCharArray()
        {
            return _str.ToCharArray();
        }
        public NewStrObject Replace(String oldValue, String newValue)
        {
            return _str.Replace(oldValue, newValue);
        }
        public NewStrObject Replace(char oldChar, char newChar)
        {
            return _str.Replace(oldChar, newChar);
        }
        public NewStrObject Remove(int startIndex, int count)
        {
            return _str.Remove(startIndex, count);
        }
        public NewStrObject Remove(int startIndex)
        {
            return _str.Remove(startIndex);
        }
        public NewStrObject PadRight(int totalWidth, char paddingChar)
        {
            return _str.PadRight(totalWidth, paddingChar);
        }
        public NewStrObject PadRight(int totalWidth)
        {
            return _str.PadRight(totalWidth);
        }
        public NewStrObject PadLeft(int totalWidth, char paddingChar)
        {
            return _str.PadLeft(totalWidth, paddingChar);
        }
        public NewStrObject PadLeft(int totalWidth)
        {
            return _str.PadLeft(totalWidth);
        }
        public NewStrObject Normalize(NormalizationForm normalizationForm)
        {
            return _str.Normalize(normalizationForm);
        }
        public NewStrObject Normalize()
        {
            return _str.Normalize();
        }
        public NewStrObject Join(String separator, params String[] value)
        {
            return String.Join(separator, value);
        }
        public NewStrObject Join(NewStrObject separator, params String[] value)
        {
            return String.Join(separator, value);
        }
        public NewStrObject Format(String format, object arg0)
        {
            return String.Format(format, arg0);
        }
        #endregion

        #region exchange
        public bool? ToBoolean()
        {
            try
            {
                return Convert.ToBoolean(_str);
            }
            catch
            {
                return null;
            }
        }

        public char? ToChar()
        {
            try
            {
                return Convert.ToChar(_str);
            }
            catch
            {
                return null;
            }
        }

        public sbyte? ToSByte()
        {
            try
            {
                return Convert.ToSByte(_str);
            }
            catch
            {
                return null;
            }
        }

        public byte? ToByte()
        {
            try
            {
                return Convert.ToByte(_str);
            }
            catch
            {
                return null;
            }
        }

        public short? ToInt16()
        {
            try
            {
                return Convert.ToInt16(_str);
            }
            catch
            {
                return null;
            }
        }

        public ushort? ToUInt16()
        {
            try
            {
                return Convert.ToUInt16(_str);
            }
            catch
            {
                return null;
            }
        }

        public int? ToInt32()
        {
            try
            {
                return Convert.ToInt32(_str);
            }
            catch
            {
                return null;
            }
        }

        public uint? ToUInt32()
        {
            try
            {
                return Convert.ToUInt32(_str);
            }
            catch
            {
                return null;
            }
        }

        public long? ToInt64()
        {
            try
            {
                return Convert.ToInt64(_str);
            }
            catch
            {
                return null;
            }
        }

        public ulong? ToUInt64()
        {
            try
            {
                return Convert.ToUInt64(_str);
            }
            catch
            {
                return null;
            }
        }

        public float? ToSingle()
        {
            try
            {
                return Convert.ToSingle(_str);
            }
            catch
            {
                return null;
            }
        }

        public double? ToDouble()
        {
            try
            {
                return Convert.ToDouble(_str);
            }
            catch
            {
                return null;
            }
        }

        public decimal? ToDecimal()
        {
            try
            {
                return Convert.ToDecimal(_str);
            }
            catch
            {
                return null;
            }
        }

        public DateTime? ToDateTime()
        {
            try
            {
                return Convert.ToDateTime(_str);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        public string ToString(IFormatProvider provider)
        {
            return _str.ToString(provider);
        }

        public IEnumerator GetEnumerator()
        {
            return _str.GetEnumerator();
        }
        public IEnumerator GetEnumerators()
        {
            return strs.GetEnumerator();
        }
        public int CompareTo(string other)
        {
            return _str.CompareTo(other);
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            return _str.GetEnumerator();
        }

        public bool Equals(NewStrObject other)
        {
            return _str.Equals(other);
        }
        public bool Equals(string other)
        {
            return _nstr.Equals(other);
        }

        public int Add(object value)
        {
            _List.Add(value.ToString());
            _List.Select(p => (NewStrObject)p).ToList().CopyTo(nstrs);
            return 0;
        }

        public bool Contains(object value)
        {
            return NewStrObject._str.Contains(value.ToString());
        }

        public void Clear()
        {
            _List.Clear();
            _List.Select(p => (NewStrObject)p).ToList().CopyTo(nstrs);
        }

        public int IndexOf(object value)
        {
            return NewStrObject._str.IndexOf(value.ToString());
        }

        public void Insert(int index, object value)
        {
            _List.Insert(index, value.ToString());
            _List.Select(p => (NewStrObject)p).ToList().CopyTo(nstrs);
        }

        public void Remove(object value)
        {
            _List.Remove(value.ToString());
            _List.Select(p => (NewStrObject)p).ToList().CopyTo(nstrs);
        }

        public void RemoveAt(int index)
        {
            _List.RemoveAt(index);
            _List.Select(p => (NewStrObject)p).ToList().CopyTo(nstrs);
        }

        public void CopyTo(Array array, int index)
        {
            nstrs.CopyTo(array, index);
        }
    }

}
