using Terraria;


namespace Starvation {
	public static partial class StarvationAPI {
		public static StarvationConfigData GetModSettings() {
			return StarvationMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			StarvationMod.Instance.ConfigJson.SaveFile();
		}

		////////////////

		//public static void CreateFriendlyBarrier( Player player, int maxRadius, int maxHp, float regenRatePerSecond, int defense, float shrinkResistScale ) {
		//	BarrierEntity.CreateBarrierEntity()
		//}
	}
}
