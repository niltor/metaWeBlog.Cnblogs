using SXL = System.Xml.Linq;

namespace Ater.MetaWeBlog.XmlRPC
{
    public class StringValue : Value
    {
        public string String;

        public StringValue(string s)
        {
            String = s;
        }

        public static string TypeString => "string";

        protected override void AddToTypeEl(SXL.XElement parent)
        {
            parent.Value = String;
        }

        public static StringValue XmlToValue(SXL.XElement parent)
        {
            StringValue bv = new StringValue(parent.Value);
            return bv;
        }

        public static implicit operator StringValue(string v)
        {
            return new StringValue(v);
        }

        private static StringValue ns;
        private static StringValue es;

        public static StringValue NullString
        {
            get
            {
                if (ns == null)
                {
                    ns = new StringValue(null);
                }
                return ns;
            }
        }

        public static StringValue EmptyString
        {
            get
            {
                if (es == null)
                {
                    es = new StringValue(string.Empty);
                }
                return es;
            }
        }

        protected override string GetTypeString()
        {
            return TypeString;
        }
    }
}