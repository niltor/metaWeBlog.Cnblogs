using SXL = System.Xml.Linq;

namespace Ater.MetaWeBlog.XmlRPC
{
    public class MethodCall
    {
        public ParameterList Parameters { get; private set; }
        public string Name { get; private set; }

        public MethodCall(string name)
        {
            Name = name;
            Parameters = new ParameterList();
        }

        public SXL.XDocument CreateDocument()
        {
            SXL.XDocument doc = new SXL.XDocument();
            SXL.XElement root = new SXL.XElement("methodCall");

            doc.Add(root);

            SXL.XElement method = new SXL.XElement("methodName");
            root.Add(method);

            method.Add(Name);

            SXL.XElement params_el = new SXL.XElement("params");
            root.Add(params_el);

            foreach (Value p in Parameters)
            {
                SXL.XElement param_el = new SXL.XElement("param");
                params_el.Add(param_el);

                _ = p.AddXmlElement(param_el);
            }

            return doc;
        }
    }
}