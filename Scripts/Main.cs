using PugMod;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace DoubleChest {
    public class Main : IMod {
	    public const string Version = "1.0.1";
	    public const string InternalName = "DoubleChest";
	    public const string DisplayName = "Paintable Double Chest";
        
        public void EarlyInit() {
            Debug.Log($"[{DisplayName}]: Mod version: {Version}");

            API.Authoring.OnObjectTypeAdded += (entity, _, entityManager) => {
	            if (entityManager.GetComponentData<ObjectDataCD>(entity).objectID != ObjectID.Carpenter)
		            return;

	            var canCraftObjectsBuffer = entityManager.GetBuffer<CanCraftObjectsBuffer>(entity);
	            canCraftObjectsBuffer.Add(new CanCraftObjectsBuffer {
		            objectID = API.Authoring.GetObjectID("DoubleChest:DoubleChest"),
		            amount = 1
	            });
            };
        }
        
        public void Init() { }

        public void Shutdown() { }

        public void ModObjectLoaded(Object obj) {
            if (obj is GameObject gameObject && gameObject.TryGetComponent<PooledGraphicalObject>(out var pooledGraphicalObject))
                PooledGraphicalObjectConverter.Register(pooledGraphicalObject);
        }

        public void Update() { }
    }
}