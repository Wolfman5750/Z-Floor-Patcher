using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
//using ICAIO_AI_FOR_MOD_NPCs.Utilities;
using Noggog;

namespace z_FloorPatcher
{
    public class z_FloorPatcher
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "ICAIO_AI_MOD_NPCs.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {


            HashSet<ModKey> vanilla = Implicits.Get(GameRelease.SkyrimSE).BaseMasters.ToHashSet();

            Console.WriteLine($"Iterating through placed objects ...");

            int ObjCount = 0;
            int NavCount = 0;

            foreach (var OBJContext in state.LoadOrder.PriorityOrder.PlacedObject().WinningContextOverrides(state.LinkCache))
            {
                //Console.WriteLine($"Obj: {OBJContext.Record.EditorID} Z: {OBJContext.Record.Placement.Position.Z}");
                    
                if (OBJContext.Record.Placement.Position.Z < -30000.0)
                {
                    Console.WriteLine($"Found Object out of bounds {OBJContext.Record.EditorID} Z: {OBJContext.Record.Placement.Position.Z}");
                    ObjCount++;


                    IPlacedObject ModObj = OBJContext.GetOrAddAsOverride(state.PatchMod);

                    P3Float P3 = new P3Float(OBJContext.Record.Placement.Position.X, OBJContext.Record.Placement.Position.Y, -30000);
                                 
                                       

                    
                    ModObj.Placement.Position=P3;




                }

            }

            //foreach (var NAVContext in state.LoadOrder.PriorityOrder.CellNavigationMesh().WinningContextOverrides(state.LinkCache))
            //{
            //    foreach (var NavVertex in NAVContext.Record.Data.Vertices)
            //    {
            //        if (NavVertex.Z < -30000)
            //        {
            //            IANavigationMeshGetter ModNav = NAVContext.GetOrAddAsOverride(state.PatchMod);
            //            continue;
            //        }
            //    }
            //}

            //foreach (var NAVContext in state.LoadOrder.PriorityOrder.WorldspaceNavigationMesh().WinningContextOverrides(state.LinkCache))
            //{
            //    foreach (var NavVertex in NAVContext.Record.Data.Vertices)
            //    {
            //        if (NavVertex.Z < -30000)
            //        {
            //            IANavigationMeshGetter ModNav = NAVContext.GetOrAddAsOverride(state.PatchMod);
            //            continue;
            //        }
            //    }
            //}

            foreach (var NAVContext in state.LoadOrder.PriorityOrder.ANavigationMesh().WinningContextOverrides(state.LinkCache))
            {
                bool NavTooLow = false;
                foreach (var NavVertex in NAVContext.Record.Data.Vertices)
                {
                    
                    if (NavVertex.Z < -30000)
                    {
                        NavTooLow = true;
                        Console.WriteLine($"Found Navmesh out of bounds: {NAVContext.Record.FormKey} Z: {NavVertex.Z}");
                        continue;
                                                
                    }

                   
                } 
                
                if (NavTooLow == true)
                {
                    NavCount++;
                    var ModNav = NAVContext.GetOrAddAsOverride(state.PatchMod);
                    
                    for (int i = 0; i < ModNav.Data.Vertices.Count; i++)
                    {
                        var pt = ModNav.Data.Vertices[i];
                        if (pt.Z < -30000)
                        {
                            P3Float P3 = new P3Float(pt.X, pt.Y, -30000);

                            ModNav.Data.Vertices[i] = P3;
                        }
                    }

                }
            }

            Console.WriteLine($"Found {ObjCount} objects below bounds and {NavCount} navmeshes below bounds ");

        }
        
       

    }
}
