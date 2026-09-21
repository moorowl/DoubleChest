using PugMod;
using System.Linq;
using UnityEngine;

namespace DoubleChest {
    public class Main : IMod {
	    public const string Version = "1.1.2";
	    public const string InternalName = "DoubleChest";
	    public const string DisplayName = "Paintable Double Chest";
        
        public void EarlyInit() {
            Debug.Log($"[{DisplayName}]: Mod version: {Version}");
        }

        public void Init() {
            InjectCraftableObject(ObjectID.Carpenter, new CraftingAuthoring.CraftableObject {
                objectID = API.Authoring.GetObjectID("DoubleChest:DoubleChest"),
                amount = 1
            });
            
            PugDatabase.objectsByType.Remove(new ObjectDataCD());
        }

        public void Shutdown() { }

        public void ModObjectLoaded(Object obj) { }

        public void Update() { }

        private static void InjectCraftableObject(ObjectID existingCraftingStation, CraftingAuthoring.CraftableObject craftableObject) {
            var craftingStationData = ScriptableData.GetDataBlocks<EntityAuthoringDataBlock>()
                .Select(dataBlock => dataBlock.prefab?.GetComponent<IEntityMonoBehaviourData>())
                .Where(entityMonoBehaviourData => entityMonoBehaviourData != null)
                .First(entityMonoBehaviourData => entityMonoBehaviourData.ObjectInfo.objectID == existingCraftingStation);

            if (!craftingStationData.GameObject.TryGetComponent<CraftingAuthoring>(out var craftingAuthoring))
                return;
            
            var emptyCraftableObjectIndex = craftingAuthoring.canCraftObjects.FindIndex(x => x.objectID == ObjectID.None);
            if (emptyCraftableObjectIndex > -1)
                craftingAuthoring.canCraftObjects[emptyCraftableObjectIndex] = craftableObject;
            else
                craftingAuthoring.canCraftObjects.Add(craftableObject);
        }
    }
}