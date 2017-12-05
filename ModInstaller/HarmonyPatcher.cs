using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
namespace ModInstaller
{
    public static class HarmonyPatcher
    {
        public static bool CheckIsPatched(AssemblyDefinition asm)
        {
            //var ModAssembly = AssemblyDefinition.ReadAssembly("ModPatcher.dll");
            //var PatchMethod = ModAssembly.MainModule.GetType("Patcher").Methods.Single(x => x.Name == "Patch");
            var type = asm.MainModule.GetType("GameInput");
            var method = type.Methods.First(x => x.Name == "Awake");
            return method.Body.Instructions[0].OpCode == OpCodes.Call;
        }
        public static void PatchIntoFile(AssemblyDefinition asm)
        {
                var ModAssembly = AssemblyDefinition.ReadAssembly(Program.prefix + "ModPatcher.dll");
                var PatchMethod = ModAssembly.MainModule.GetType("Patcher").Methods.Single(x => x.Name == "Patch");
                var type = asm.MainModule.GetType("GameInput");
                var method = type.Methods.First(x => x.Name == "Awake");
                method.Body.GetILProcessor().InsertBefore(method.Body.Instructions[0], Instruction.Create(OpCodes.Call, method.Module.Import(PatchMethod)));

        }

    }
}
