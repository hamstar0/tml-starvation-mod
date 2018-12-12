using HamstarHelpers.Components.Config;
using System;


namespace Starvation {
	public class StarvationConfigData : ConfigurationDataBase {
		public readonly static string ConfigFileName = "Starvation Config.json";



		////////////////

		public string VersionSinceUpdate = "";

		//public bool DebugModeInfo = false;

		public int WellFedDrainRate = 4;
		public int StarvationHarm = 1;
		public int StarvationHarmRate = 10;



		////////////////

		public void SetDefaults() { }
		
		
		////////////////

		public bool UpdateToLatestVersion() {
			var mymod = StarvationMod.Instance;
			var newConfig = new StarvationConfigData();
			newConfig.SetDefaults();

			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( versSince >= mymod.Version ) {
				return false;
			}

			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
