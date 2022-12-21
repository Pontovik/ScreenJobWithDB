
using ScreenJob.XmlActions;

namespace ScreenJob
{
    public class Programm
    {

        public static void Main()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            DbActions.UpdateFromXml(@$"{path}orders.xml");
        }
    }
}



