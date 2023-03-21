using System.Collections.Generic;
using System.Linq;

namespace Ater.MetaWeBlog.XmlRPC
{
    public class MethodResponse
    {
        public ParameterList Parameters { get; private set; }

        public MethodResponse(string content)
        {
            Parameters = new ParameterList();

            System.Xml.Linq.LoadOptions lo = new System.Xml.Linq.LoadOptions();

            System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse(content, lo);
            System.Xml.Linq.XElement root = doc.Root;
            System.Xml.Linq.XElement fault_el = root.Element("fault");
            if (fault_el != null)
            {
                Fault f = Fault.ParseXml(fault_el);

                string msg = string.Format("XMLRPC FAULT [{0}]: \"{1}\"", f.FaultCode, f.FaultString);
                XmlRPCException exc = new XmlRPCException(msg)
                {
                    Fault = f
                };

                throw exc;
            }

            System.Xml.Linq.XElement params_el = root.GetElement("params");
            List<System.Xml.Linq.XElement> param_els = params_el.Elements("param").ToList();

            foreach (System.Xml.Linq.XElement param_el in param_els)
            {
                System.Xml.Linq.XElement value_el = param_el.GetElement("value");

                Value val = XmlRPC.Value.ParseXml(value_el);
                Parameters.Add(val);
            }
        }
    }
}