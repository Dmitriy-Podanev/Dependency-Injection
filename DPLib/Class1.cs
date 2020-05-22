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
     
        private  Dictionary<Type, object> instancesObjects = new Dictionary<Type, object>();
        public ObjectContext()
        {

            var assembly = Assembly.Load("DPLib");
            Type[] types = assembly.GetTypes();
            foreach (var typ in types)
            {
                var attributes = typ.GetCustomAttributes(typeof(AppComponent), false);
                if (attributes.Length == 0)
                    continue;
               
                instancesObjects.Add(typ,Activator.CreateInstance(typ));
            }
        }





        public T GetComponent<T>() 
        {
            var temp= default(object);
            foreach (var service in instancesObjects)
            {
                if (service.Key==typeof(T))
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
              // var asd= field.ReflectedType;
              // var asas = instancesObjects[typeof(DataProvider)];
                foreach (var instancesObject in instancesObjects)
                {
                    var azz = instancesObjects[typeof(T)];
                   // field.SetValue(instancesObject.Value, instancesObject.Value);
                  // var ccc= field.ReflectedType.GetType();
                  // var asdasd = field.FieldType;
                  var asdasd = field.ReflectedType;
                 // var pd = field.GetValue(typeof(DataService));
                    if (field.FieldType == instancesObject.Key)
                    {
                      field.SetValue(azz, instancesObject.Value);//TODO должна передать поле instance
                      //Console.WriteLine("Задал поле");
                      
                       

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
        [Inject] public DataProvider _dataProvider; //get//TODO присвоить сслыкам занчение активатора из context
        [Inject] public WebProvider _webProvider; //get

        public void ProcessData(string str)
        {
            
          
            
            string data = _dataProvider.GetData().ToString();
            data = (data + " " +str).ToUpper();
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