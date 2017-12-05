using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Emit = System.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;
namespace ModInstaller
{
    class Program
    {
        public static string prefix = "";
        static void Main(string[] args)
        {
            if (!File.Exists("Assembly-CSharp.dll"))
            {
                prefix = "Subnautica_Data/Managed/";

            }
            var csharpasm = AssemblyDefinition.ReadAssembly(prefix+"Assembly-CSharp.dll");
            if (!HarmonyPatcher.CheckIsPatched(csharpasm))
            {
                PatchAssembly(csharpasm);
                //AddTechTypesToEnum(csharpasm);
                csharpasm.Write(prefix + "Assembly-CSharp.dll.q");
                if (File.Exists(prefix + "Assembly-CSharp.original.dll"))
                {
                    File.Delete(prefix + "Assembly-CSharp.original.dll");
                }
                File.Move(prefix + "Assembly-CSharp.dll", prefix + "Assembly-CSharp.original.dll");
                File.Move(prefix + "Assembly-CSharp.dll.q", prefix + "Assembly-CSharp.dll");
            }
            Console.WriteLine("Patching Finished!");
            Console.WriteLine("Press Enter To Exit...");
            Console.ReadLine();
        }
        public static void PatchAssembly(AssemblyDefinition assembly)
        {

            HarmonyPatcher.PatchIntoFile(assembly);
        }
        public static readonly Dictionary<string, int> customTechTypes = new Dictionary<string, int>() {

        };
        public static void AddTechTypesToEnum(AssemblyDefinition tag)
        {
            var techtype = tag.MainModule.GetType("TechType");

            if (techtype.IsEnum)
            {
                foreach (var a in customTechTypes)
                {
                    if (!techtype.Fields.Any((s) => s.Name == a.Key))
                    {

                        var c = new FieldDefinition(a.Key, techtype.Fields[1].Attributes, techtype);
                        // c.HasConstant = true;
                        c.Constant = a.Value;
                        techtype.Fields.Add(c);

                    }
                }
                
            }
        }
    }

        
}
