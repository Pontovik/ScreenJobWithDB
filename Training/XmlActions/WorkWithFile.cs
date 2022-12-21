using ScreenJob.Models;
using System.Xml.Serialization;

namespace ScreenJob.XmlActions
{
    public class WorkWithFile
    {
        public static NewOrders? GetData(string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(NewOrders));
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            NewOrders? newOrders = (NewOrders)formatter.Deserialize(fs);
            fs.Close();
            return newOrders;
        }
    }
}
