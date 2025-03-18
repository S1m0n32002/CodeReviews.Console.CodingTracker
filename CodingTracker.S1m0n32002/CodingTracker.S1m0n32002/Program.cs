using CodingTracker.S1m0n32002.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        Settings.Load();

        Debug.WriteLine($"DbName: {Settings.Current.DbName}");
        Debug.WriteLine($"DbPath: {Settings.Current.DbPath}");
    }
}