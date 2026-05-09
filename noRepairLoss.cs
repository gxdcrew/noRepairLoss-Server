using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Servers;
using System.Threading.Tasks;
using System.Collections.Generic;
using SemanticVersioning;
using System;

namespace EquipmentsAR;

public record ModMetadata : AbstractModMetadata {
   public override string ModGuid { get; init; } = "com.luckyy.NoRepairLoss";
   public override string Name { get; init; } = "No Repair Loss";
   public override string Author { get; init; } = "luckyyy";
   public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");
   public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
   public override string License { get; init; } = "MIT";
   public override List<string>? Contributors { get; init; } = new();
   public override bool? IsBundleMod { get; init; } = false;
   public override List<string>? Incompatibilities { get; init; } = new();
   public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; } = new();
   public override string? Url { get; init; } = string.Empty;
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class RepairWipeHook(DatabaseServer databaseServer) : IOnLoad {
   public Task OnLoad() {
      Console.WriteLine("[NRL Server] Initializing 0 Repair Loss patch...");

      var itemsDb = databaseServer.GetTables().Templates.Items;
      int modifiedCount = 0;

      foreach(var item in itemsDb.Values) {
         var props = item.Properties;
         if(props != null) {
            if (props.MaxRepairDegradation > 0 || props.MinRepairDegradation > 0 ||
               props.MaxRepairKitDegradation > 0 || props.MinRepairKitDegradation > 0) {
               props.MinRepairDegradation = 0;
               props.MaxRepairDegradation = 0;
               props.MinRepairKitDegradation = 0;
               props.MaxRepairKitDegradation = 0;
               modifiedCount++;
            }
         }
      }

      Console.WriteLine($"[NRL Server] Successfully removed repair loss from {modifiedCount} items");

      return Task.CompletedTask;
   }
}