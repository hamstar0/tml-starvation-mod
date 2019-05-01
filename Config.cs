using HamstarHelpers.Components.Config;
using System;
using System.Collections.Generic;
using Terraria.ID;


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
		public bool FoodIngredientsAlsoSpoil = true;
		public int FoodIngredientSpoilageDuration = 30 * 60 * 60;	// 30 minutes; no "Well Fed" rate modifier

		public bool CraftableUnlifeCrystal = true;
		public bool UnlifeCrystalReturnsLifeCrystal = true;

		public int TupperwareDropsFromNpcId = NPCID.Skeleton;
		public float TupperwareDropChance = 0.35f;
		public float TupperwareSpoilageRate = 0.5f;

		public IDictionary<string, int> CustomWellFedDurations = new Dictionary<string, int>();
		public bool CustomPumpkinPieRecipe = true;


		////

		public string _OLD_SETTINGS_BELOW_ = "";

		public int StarvationHarmRate = 10;
		public float AddedWellFedDrainRatePerMaxHealthOver100 = (1f + (1f/3f)) / 100f;
		public float AddedStarvationHarmPerMaxHealthOver100 = (1f + (1f/3f)) / 100f;



		////////////////

		public void SetDefaults() {
			this.CustomWellFedDurations.Clear();
			this.CustomWellFedDurations[ "Cooked Marshmallow" ] = 5 * 60 * 60;			// 5 minutes
			this.CustomWellFedDurations[ "Bowl of Soup" ] = 45 * 60 * 60;				// 45 minutes
			this.CustomWellFedDurations[ "Pumpkin Pie" ] = 25 * 60 * 60;				// 25 minutes
			this.CustomWellFedDurations[ "Cooked Fish" ] = 15 * 60 * 60;                // 15 minutes
			this.CustomWellFedDurations[ "Cooked Shrimp" ] = 15 * 60 * 60;				// 15 minutes
			this.CustomWellFedDurations[ "Sashimi" ] = 15 * 60 * 60;					// 15 minutes
			this.CustomWellFedDurations[ "Grub Soup" ] = 90 * 60 * 60;					// 90 minutes
			this.CustomWellFedDurations[ "Gingerbread Cookie" ] = 5 * 60 * 60;			// 5 minutes
			this.CustomWellFedDurations[ "Sugar Cookie" ] = 5 * 60 * 60;				// 5 minutes
			this.CustomWellFedDurations[ "Christmas Pudding" ] = 5 * 60 * 60;			// 5 minutes
		}


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
			if( versSince < new Version( 1, 5, 0 ) ) {
				this.SetDefaults();
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
