using PugMod;
using System.Linq;
using UnityEngine;

namespace DoubleChest {
    public class Main : IMod {
	    public const string Version = "1.0.2";
	    public const string InternalName = "DoubleChest";
	    public const string DisplayName = "Paintable Double Chest";
        
        public void EarlyInit() {
            Debug.Log($"[{DisplayName}]: Mod version: {Version}");
        }

        public void Init() {
            var carpentersWorkbench = DatabaseConversionUtility.GetPrefabList(Manager.ecs.pugDatabase).First(prefab => prefab.ObjectInfo.objectID == ObjectID.Carpenter);
            if (carpentersWorkbench.ObjectInfo.prefabInfos[0].ecsPrefab.TryGetComponent<CraftingAuthoring>(out var craftingAuthoring)) {
                craftingAuthoring.canCraftObjects.Add(new CraftingAuthoring.CraftableObject {
                    objectID = API.Authoring.GetObjectID("DoubleChest:DoubleChest"),
                    amount = 1
                });
            }
        }

        public void Shutdown() { }

        public void ModObjectLoaded(Object obj) {
            if (obj is GameObject gameObject && gameObject.TryGetComponent<PooledGraphicalObject>(out var pooledGraphicalObject))
                PooledGraphicalObjectConverter.Register(pooledGraphicalObject);

            if (obj is TextDataBlock textDataBlock)
	            textDataBlock.name = textDataBlock.name.Replace("-", ":");
        }

        public void Update() { }
    }
}