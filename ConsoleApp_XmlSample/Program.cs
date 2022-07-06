using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_XmlSample
{
    class Program
    {
        static string xmlpath = @".\ConsoleApp_XmlSample.xml";


        static XmlContent currentsetting;
        static void Main(string[] args)
        {
            // if file not exist
            if(!CheckFile(xmlpath))
            {
                // create xml
                if(!CreateXmlFile(xmlpath))
                {
                    System.Diagnostics.Trace.WriteLine($"Create {xmlpath} failed");
                    return;
                }
            }

            if(CheckFile(xmlpath))
            {
                // read xml
                currentsetting = ReadXmlFile(xmlpath);
                if(currentsetting == null)
                {
                    System.Diagnostics.Trace.WriteLine($"Read {xmlpath} failed");
                    return;
                }
            }

            Console.WriteLine($"HelloWorldstr = {currentsetting.HelloWorldstr}");
            Console.WriteLine($"IntValue = {currentsetting.IntValue}");
            Console.WriteLine($"FloatValue = {currentsetting.FloatValue}");
            Console.WriteLine($"ByteValue = {currentsetting.ByteValue}");

            Console.WriteLine($"IntValues:");
            foreach (var tmp in currentsetting.IntValues)
            {
                Console.Write($"{tmp}, ");
            }

            Console.WriteLine($"\nStringValues:");
            int i = 0;
            foreach (var tmp in currentsetting.StringValues)
            {
                i++;
                Console.WriteLine($"[{i}]{tmp}");
            }

            Console.ReadLine();
        }

        static bool CreateXmlFile(string filepath)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(XmlContent));

                    //xmlSerializer.Serialize(fs, new XmlContent());// init with default value

                    // init with manual value
                    XmlContent xmlContent = new XmlContent()
                    {
                        HelloWorldstr = "Fuck the world",
                        IntValue = -3,
                        FloatValue = 3.3333f,
                        ByteValue = 0x01,
                        IntValues = new List<int>() { 1, 2, 3 },
                        StringValues = new List<string>() { "str1", "str2" }
                    };
                    xmlSerializer.Serialize(fs, xmlContent);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"{ex.Message}");
                System.Diagnostics.Trace.WriteLine($"{ex.StackTrace}");
                return false;
            }

            return true;
        }

        static XmlContent ReadXmlFile(string filepath)
        {
            XmlContent localXmlContent;
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(XmlContent));
                    localXmlContent = (XmlContent)xmlSerializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"{ex.Message}");
                System.Diagnostics.Trace.WriteLine($"{ex.StackTrace}");
                return null;
            }

            return localXmlContent;
        }

        static bool CheckFile(string filepath)
        {
            return File.Exists(filepath);
        }
    }
}

public class XmlContent
{
    public string HelloWorldstr { get; set; } = @"Hello World!";
    public int IntValue { get; set; } = 0;
    public float FloatValue { get; set; } = 1.2f;
    public byte ByteValue { get; set; } = 0x08;
    public List<int> IntValues = new List<int>();
    public List<string> StringValues = new List<string>();
}
