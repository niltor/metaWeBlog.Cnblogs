using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SXL = System.Xml.Linq;

namespace Ater.MetaWeBlog.XmlRPC
{
    public class Array : Value, IEnumerable<Value>
    {
        private readonly List<Value> items;

        public Array()
        {
            items = new List<Value>();
        }

        public Array(int capacity)
        {
            items = new List<Value>(capacity);
        }

        public void Add(Value v)
        {
            items.Add(v);
        }

        public void Add(int v)
        {
            items.Add(new IntegerValue(v));
        }

        public void Add(double v)
        {
            items.Add(new DoubleValue(v));
        }

        public void Add(bool v)
        {
            items.Add(new BooleanValue(v));
        }

        public void Add(System.DateTime v)
        {
            items.Add(new DateTimeValue(v));
        }

        public void AddRange(IEnumerable<Value> items)
        {
            foreach (Value item in items)
            {
                this.items.Add(item);
            }
        }

        public Value this[int index] => items[index];

        public IEnumerator<Value> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static string TypeString => "array";

        public int Count => items.Count;

        protected override void AddToTypeEl(SXL.XElement parent)
        {
            SXL.XElement data_el = new SXL.XElement("data");
            parent.Add(data_el);
            foreach (Value item in this)
            {
                _ = item.AddXmlElement(data_el);
            }
        }

        internal static Array XmlToValue(SXL.XElement type_el)
        {
            SXL.XElement data_el = type_el.GetElement("data");

            List<SXL.XElement> value_els = data_el.Elements("value").ToList();
            Array list = new Array();
            foreach (SXL.XElement value_el2 in value_els)
            {
                Value o = ParseXml(value_el2);
                list.Add(o);
            }
            return list;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Array p))
            {
                return false;
            }

            if (items != p.items)
            {
                if (items.Count != p.items.Count)
                {
                    return false;
                }

                for (int i = 0; i < items.Count; i++)
                {
                    if (!items[i].Equals(p[i]))
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
            return items.GetHashCode();
        }
    }
}