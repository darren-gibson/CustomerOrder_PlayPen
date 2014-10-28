namespace CustomerOrder.Query.EventPublication.Atom.UnitTests
{
    internal static class XmlHelper
    {
        public static string RemoveDefaultSchema(string xml)
        {
            // Required as it is inconsistent the order that the schemas are written in the Resharper runner vs. the Visual Studio runner
            var result = xml.Replace(@" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "");
            result = result.Replace(@" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""", "");

            return result;
        }
    }
}
