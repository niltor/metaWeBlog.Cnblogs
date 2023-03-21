using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SXL = System.Xml.Linq;

namespace Ater.MetaWeBlog.XmlRPC
{
    public class Struct : Value, IEnumerable<KeyValuePair<string, Value>>
    {
        private readonly Dictionary<string, Value> dic;

        public Struct()
        {
            dic = new Dictionary<string, Value>();
        }

        private bool TryGet(string name, out Value v)
        {
            return dic.TryGetValue(name, out v);
        }

        public Value TryGet(string name)
        {
            _ = dic.TryGetValue(name, out Value v);
            return v;
        }

        private void checktype<T>(Value v)
        {
            Type expected = typeof(T);
            Type actual = v.GetType();
            if (expected != actual)
            {
                string msg = string.Format("Expected type {0} instead got {1}", expected.Name, actual.Name);
                throw new XmlRPCException(msg);
            }
        }

        public T TryGet<T>(string name) where T : Value
        {
            Value v = TryGet(name);
            if (v == null)
            {
                return null;
            }

            checktype<T>(v);

            return (T)v;
        }

        public T Get<T>(string name, T defval) where T : Value
        {
            Value v = TryGet(name);
            if (v == null)
            {
                return defval;
            }

            checktype<T>(v);

            return (T)v;
        }

        public Value Get(string name)
        {
            Value v = TryGet(name);
            if (v == null)
            {
                string msg = string.Format("Struct does not contains {0}", name);
                throw new XmlRPCException(msg);
            }
            return v;
        }

        public T Get<T>(string name) where T : Value
        {
            Value v = Get(name);
            checktype<T>(v);
            return (T)v;
        }

        public Value this[string index]
        {
            get => Get(index);
            set => dic[index] = value;
        }

        public int Count => dic.Count;

        public bool ContainsKey(string name)
        {

            return dic.ContainsKey(name);
        }

        public IEnumerator<KeyValuePair<string, Value>> GetEnumerator()
        {
            return dic.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static string TypeString => "struct";

        protected override void AddToTypeEl(SXL.XElement parent)
        {
            foreach (KeyValuePair<string, Value> pair in this)
            {
                SXL.XElement member_el = new SXL.XElement("member");
                parent.Add(member_el);

                SXL.XElement name_el = new SXL.XElement("name");
                member_el.Add(name_el);
                name_el.Value = pair.Key;

                _ = pair.Value.AddXmlElement(member_el);
            }
        }

        public static Struct XmlToValue(SXL.XElement type_el)
        {
            List<SXL.XElement> member_els = type_el.Elements("member").ToList();
            Struct struct_ = new Struct();
            foreach (SXL.XElement member_el in member_els)
            {
                SXL.XElement name_el = member_el.GetElement("name");
                string name = name_el.Value;

                SXL.XElement value_el2 = member_el.GetElement("value");
                Value o = ParseXml(value_el2);

                struct_[name] = o;
            }
            return struct_;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Struct p))
            {
                return false;
            }

            if (dic != p.dic)
            {
                if (dic.Count != p.dic.Count)
                {
                    return false;
                }

                foreach (KeyValuePair<string, Value> src_pair in this)
                {

                    bool b = p.TryGet(src_pair.Key, out Value des_val);

                    if (b == false)
                    {
                        return false;
                    }

                    if (!src_pair.Value.Equals(des_val))
                    {
                        return false;
                    }
                }

                return true;
            }
            return true;
        }

        protected override string GetTypeString()
        {
            return TypeString;
        }

        public override int GetHashCode()
        {
            return dic.GetHashCode();
        }
    }
}