using System;
using System.CodeDom;
using System.Reflection;

using System.Collections;
using DPLib;

namespace TestsProgaramms
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectContext context = new ObjectContext();

            Console.WriteLine("****************");
            DataService dataService = context.GetComponent<DataService>();
            dataService.ProcessData("Привет");








        }
    }
}
