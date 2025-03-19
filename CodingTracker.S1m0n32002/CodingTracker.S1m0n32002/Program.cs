using CodingTracker.S1m0n32002.Models;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Settings.Load();
        Settings.Save();
        Debug.WriteLine($"DbName: {Settings.Current.DbName}");
        Debug.WriteLine($"DbPath: {Settings.Current.DbPath}");
    }
}