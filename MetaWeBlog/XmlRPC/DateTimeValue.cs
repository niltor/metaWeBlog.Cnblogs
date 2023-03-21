using SXL = System.Xml.Linq;

namespace Ater.MetaWeBlog.XmlRPC
{
    public class DateTimeValue : Value
    {
        public readonly System.DateTime Data;

        public DateTimeValue(System.DateTime value)
        {
            Data = value;
        }

        public static string TypeString => "dateTime.iso8601";

        protected override void AddToTypeEl(SXL.XElement parent)
        {
            string s = Data.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            s = s.Replace("-", "");
            parent.Value = s;
        }

        public static DateTimeValue XmlToValue(SXL.XElement parent)
        {
            _ = System.DateTime.Now;
            if (System.DateTime.TryParse(parent.Value, out System.DateTime dt))
            {
                return new DateTimeValue(dt);
            }

            string date = parent.Value.Trim('Z');// remove Z from SharePoint date

            System.DateTime x = System.DateTime.ParseExact(date, "yyyyMMddTHH:mm:ss", null);
            DateTimeValue y = new DateTimeValue(x);
            return y;
        }

        public static implicit operator DateTimeValue(System.DateTime v)
        {
            return new DateTimeValue(v);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is DateTimeValue p))
            {
                return false;
            }

            // Return true if the fields match:
            return Data.Day == p.Data.Day && Data.Month == p.Data.Month && Data.Year == p.Data.Year &&
                Data.Hour == p.Data.Hour && Data.Minute == p.Data.Minute && Data.Second == p.Data.Second;
        }

        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }

        protected override string GetTypeString()
        {
            return TypeString;
        }
    }
}