using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace DPLib
{
    public interface Service
    {
        string GetData();
    }


    public class ObjectContext
    {

        private Dictionary<Type, object> instancesObjects = new Dictionary<Type, object>();
        public ObjectContext()
        {

            var assembly = Assembly.Load("DPLib");
            Type[] types = assembly.GetTypes();
            foreach (var typ in types)
            {
                var attributes = typ.GetCustomAttributes(typeof(AppComponent), false);
                if (attributes.Length == 0)
                    continue;
                instancesObjects.Add(typ, Activator.CreateInstance(typ));
            }
        }

        public T GetComponent<T>()
        {
            var temp = default(object);
            foreach (var service in instancesObjects)
            {
                if (service.Key == typeof(T))
                {
                    temp = service.Value;
                    break;
                }

            }


            foreach (var field in instancesObjects[typeof(T)].GetType().GetFields())
            {
                var attributes = field.GetCustomAttributes(typeof(Inject), false);

                if (attributes.Length == 0)
                    continue;

                foreach (var instancesObject in instancesObjects)
                {
                    if (field.FieldType == instancesObject.Key)
                    {
                        field.SetValue(instancesObjects[typeof(T)], instancesObject.Value);
                    }
                }

            }
            return (T)temp;
        }

    }

    public class AppComponent : Attribute { }

    public class Inject : Attribute { }

    [AppComponent]
    public class DataService
    {
        [Inject] public DataProvider _dataProvider;
        [Inject] public WebProvider _webProvider;

        public void ProcessData()
        {
            string data = _dataProvider.GetData().ToString(CultureInfo.InvariantCulture);
            _webProvider.SendData(data);
        }


    }

    [AppComponent]
    public class DataProvider // Возвращает время
    {
        public DateTime GetData()
        {

            return DateTime.Now;
        }
    }

    [AppComponent]
    public class WebProvider// Должен выводить Полученное время в Консоль 
    {

        public void SendData(string data)
        {
            Console.WriteLine(data);

        }
    }


}