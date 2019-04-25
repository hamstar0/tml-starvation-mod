using HamstarHelpers.Components.Config;
using System;


namespace Starvation {
	public class StarvationConfigData : ConfigurationDataBase {
		public readonly static string ConfigFileName = "Starvation Config.json";



		////////////////

		public string VersionSinceUpdate = "";

		//public bool DebugModeInfo = false;

		public int WellFedDrainRate = 2;	// Ticks
		public int StarvationHarm = 1;
		public int StarvationHarmDelay = 10;	// Ticks

		public int PlayerStarterSoup = 3;

		public int RespawnWellFedDuration = 60 * 60 * 3;    // 3 minutes

		public float AddedWellFedDrainRateMultiplierPerMaxHealthOver100 = (2f / 3f) / 100f;	// CORRECTLY triples at 400 max hp
		public float AddedStarvationHarmMultiplierPerMaxHealthOver100 = (2f / 3f) / 100f;

		public bool FoodSpoilageEnabled = false;
		public float FoodSpoilageRate = 1f;

		public bool CraftableUnlifeCrystal = true;


		////

		public string _OLD_SETTINGS_BELOW_ = "";

		public int StarvationHarmRate = 10;
		public float AddedWellFedDrainRatePerMaxHealthOver100 = (1f + (1f/3f)) / 100f;
		public float AddedStarvationHarmPerMaxHealthOver100 = (1f + (1f/3f)) / 100f;



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

			if( versSince < new Version(1, 1, 3) ) {
				if( this.VersionSinceUpdate != "" ) {
					this.StarvationHarmDelay = this.StarvationHarmRate;
				}
			}
			if( versSince < new Version( 1, 3, 1 ) ) {
				if( this.FoodSpoilageRate == 3f ) {
					this.FoodSpoilageRate = newConfig.FoodSpoilageRate;
				}
			}
			if( versSince < new Version( 1, 4, 0 ) ) {
				if( this.FoodSpoilageRate == 2f ) {
					this.FoodSpoilageRate = newConfig.FoodSpoilageRate;
				}
				if( this.AddedWellFedDrainRatePerMaxHealthOver100 == ((1f + (1f/3f)) / 100f) ) {
					this.AddedWellFedDrainRatePerMaxHealthOver100 = newConfig.AddedWellFedDrainRatePerMaxHealthOver100;
				}
				if( this.WellFedDrainRate == 4f ) {
					this.WellFedDrainRate = newConfig.WellFedDrainRate;
				}
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
