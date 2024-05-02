// See https://aka.ms/new-console-template for more information
using System;
using System.Reflection;
using System.IO;
/*
 * Starting was at 2023.7.6.
 * This was made to get AtmLoader's dependency.
 * for exercise.
 */
class Program
{
    static void Main()
    {
        string exePath = Environment.CurrentDirectory + "\\AtmLoader.exe";

        try
        {
            Assembly assembly = Assembly.LoadFrom(exePath);

            // Get the list of referenced assemblies
            AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();

            Console.WriteLine("AtmLoader.exe dependencies\n");
            //List<string> dependentFiles = new List<string>();
            foreach (AssemblyName referencedAssembly in referencedAssemblies)
            {
                string dependentFilePath = Environment.CurrentDirectory + "\\" + referencedAssembly.Name + ".dll";
                if (File.Exists(dependentFilePath))
                {
                    Console.WriteLine("existed: " + dependentFilePath);
                }
                else
                {
                    Console.WriteLine("required :" + referencedAssembly.Name + ".dll");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}