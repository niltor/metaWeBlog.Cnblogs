using System;
using SXL = System.Xml.Linq;

namespace Ater.MetaWeBlog.XmlRPC
{
    public class Base64Data : Value
    {
        public byte[] Bytes { get; private set; }

        public Base64Data(byte[] bytes)
        {
            Bytes = bytes ?? throw new ArgumentNullException("bytes");
        }

        public static string TypeString => "base64";

        protected override void AddToTypeEl(SXL.XElement parent)
        {
            parent.Add(Convert.ToBase64String(Bytes));
        }

        internal static Base64Data XmlToValue(SXL.XElement type_el)
        {
            byte[] bytes = Convert.FromBase64String(type_el.Value);
            Base64Data b = new Base64Data(bytes);
            return b;
        }

        public static implicit operator Base64Data(byte[] v)
        {
            return new Base64Data(v);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Base64Data p))
            {
                return false;
            }

            if (Bytes != p.Bytes)
            {
                if (Bytes.Length != p.Bytes.Length)
                {
                    return false;
                }

                for (int i = 0; i < Bytes.Length; i++)
                {
                    if (Bytes[i] != p.Bytes[i])
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
            return Bytes.GetHashCode();
        }
    }
}