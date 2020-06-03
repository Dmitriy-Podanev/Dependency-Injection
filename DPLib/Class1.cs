using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;


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

        public string ProcessData(string s)
        {
            if (s!=null && s.GetType() == typeof(string))
            {
                string data = _dataProvider.GetData().ToString(CultureInfo.InvariantCulture);
                data = data + " " + s;
               return _webProvider.SendData(data);
            }
            else
            {
                throw new ArgumentException();
            }
           
        }


    }

    [AppComponent]
    public class DataProvider
    {
        public string GetData()
        {

            return "SAMPLE DATA";
        }
    }

    [AppComponent]
    public class WebProvider
    {

        public string SendData(string data)
        {
            Console.WriteLine(data);
            return data; 

        }
    }


}