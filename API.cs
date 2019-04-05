using Terraria;


namespace Starvation {
	public static partial class StarvationAPI {
		public static StarvationConfigData GetModSettings() {
			return StarvationMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			StarvationMod.Instance.ConfigJson.SaveFile();
		}
	}
}
