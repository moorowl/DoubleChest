using HarmonyLib;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace DoubleChest {
	[HarmonyPatch]
	public static class ExpandCraftingWindowPatches {
		private const int MaxWindows = 4;
		private const int MaxWindowsInVanilla = 3;
		
		[HarmonyPatch(typeof(SimpleCraftingUIContainer), "Awake")]
		[HarmonyPostfix]
		private static void InstantiateNewWindows(SimpleCraftingUIContainer __instance) {
			if (__instance.simpleCraftingUIs == null || __instance.simpleCraftingUIs.Count == 0)
				return;

			if (__instance.simpleCraftingUIs.Count >= MaxWindows)
				return;

			while (__instance.simpleCraftingUIs.Count < MaxWindows) {
				var windowPrefab = __instance.simpleCraftingUIs[^1];
				if (windowPrefab == null)
					return;

				var windowInstance = Object.Instantiate(windowPrefab.gameObject, windowPrefab.transform.parent);
				if (!windowInstance.TryGetComponent<SimpleCraftingUI>(out var windowInstanceUI)) {
					Object.Destroy(windowInstance);
					return;
				}

				__instance.simpleCraftingUIs.Add(windowInstanceUI);
				windowInstanceUI.Init();
			}
		}

		[HarmonyPatch(typeof(CraftingCategoryNavigationUI), "LateUpdate")]
		private static void FixControllerNavigation(CraftingCategoryNavigationUI __instance) {
			if (__instance.root == null || !__instance.root.activeSelf)
				return;
			
			var count = Manager.ui.simpleCraftingUIContainer.amountOfWindowsShowing;
			if (Manager.ui.simpleCraftingUIContainer.amountOfWindowsShowing <= MaxWindowsInVanilla)
				return;

			__instance.root.transform.localPosition = new Vector3(-2.5f * count - 0.3125f, 0f, 0f);
		}
	}
}