using SXL = System.Xml.Linq;

namespace Ater.MetaWeBlog.XmlRPC
{
    public class IntegerValue : Value
    {
        public readonly int Integer;

        public IntegerValue(int i)
        {
            Integer = i;
        }

        public static string TypeString => "int";

        protected override void AddToTypeEl(SXL.XElement parent)
        {
            parent.Value = Integer.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static IntegerValue XmlToValue(SXL.XElement parent)
        {
            IntegerValue bv = new IntegerValue(int.Parse(parent.Value));
            return bv;
        }

        public static string AlternateTypeString => "i4";

        public static implicit operator IntegerValue(int v)
        {
            return new IntegerValue(v);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is IntegerValue p))
            {
                return false;
            }

            return Integer == p.Integer;
        }

        public override int GetHashCode()
        {
            return Integer.GetHashCode();
        }

        protected override string GetTypeString()
        {
            return TypeString;
        }

    }
}