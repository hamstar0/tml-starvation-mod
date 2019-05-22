using HamstarHelpers.Components.Config;
using System;
using System.Collections.Generic;
using Terraria.ID;


namespace Starvation {
	public class StarvationConfigData : ConfigurationDataBase {
		public readonly static string ConfigFileName = "Starvation Config.json";



		////////////////

		public string VersionSinceUpdate = "";

		public bool DebugModeInfo = false;

		public int WellFedAddedDrainPerTick = 1;
		public int StarvationHarm = 1;
		public int StarvationHarmRepeatDelayInTicks = 10;

		public int PlayerStarterSoup = 3;

		public int RespawnWellFedTickDuration = 60 * 60 * 3;    // 3 minutes

		public float AddedWellFedDrainRatePerTickMultiplierPerMaxHealthOver100 = 1f / 200f;
		public float AddedStarvationHarmPerTickMultiplierPerMaxHealthOver100 = 1f / 100f;

		public bool FoodSpoilageEnabled = false;
		public float FoodSpoilageRatePerSecond = 0.5f;
		public int FoodSpoilageMinDuration = 60 * 60 * 15;  // 15 minutes
		public int FoodSpoilageMaxDuration = 60 * 60 * 60 * 3;	// 3 hours

		public bool FoodIngredientsAlsoSpoil = true;
		public int FoodIngredientSpoilageTickDuration = 30 * 60 * 60;   // 30 minutes
		public float FoodIngredientSpoilageRatePerSecond = 0.5f;

		public bool CraftableUnlifeCrystal = true;
		public bool UnlifeCrystalReturnsLifeCrystal = true;

		public IDictionary<int, float> TupperwareDropsNpcIdsAndChances = new Dictionary<int, float>(); //NPCID.Skeleton : 0.35f;
		public float TupperwareSpoilageRate = 0.5f;
		public int TupperwareMaxStackSize = 30;
		public int TupperwareSellsFromMerchantByNpcId = NPCID.SkeletonMerchant;

		public bool CustomPumpkinPieRecipe = true;
		public IDictionary<string, int> CustomWellFedTickDurations = new Dictionary<string, int>();
		public bool FishbowlToGoldfishRecipe = true;


		////

		public string _OLD_SETTINGS_BELOW_ = "";

		public int StarvationHarmRate = 10;
		public float AddedWellFedDrainRatePerMaxHealthOver100 = (1f + (1f/3f)) / 100f;
		public float AddedStarvationHarmPerMaxHealthOver100 = (1f + (1f/3f)) / 100f;
		public float AddedWellFedDrainRateMultiplierPerMaxHealthOver100 = (2f/3f) / 100f;
		public float AddedStarvationHarmMultiplierPerMaxHealthOver100 = (2f/3f) / 100f;
		public int WellFedDrainRate = 2;
		public int StarvationHarmDelay = 10;
		public int RespawnWellFedDuration = 60 * 60 * 3;
		public int FoodIngredientSpoilageDuration = 30 * 60 * 60;
		public float FoodSpoilageRate = 1f;
		public float FoodIngredientSpoilageRate = 1f;



		////////////////

		public void SetDefaults() {
			this.CustomWellFedTickDurations.Clear();
			this.CustomWellFedTickDurations[ "Cooked Marshmallow" ] = 5 * 60 * 60;			// 5 minutes
			this.CustomWellFedTickDurations[ "Bowl of Soup" ] = 45 * 60 * 60;				// 45 minutes
			this.CustomWellFedTickDurations[ "Pumpkin Pie" ] = 25 * 60 * 60;				// 25 minutes
			this.CustomWellFedTickDurations[ "Cooked Fish" ] = 15 * 60 * 60;                // 15 minutes
			this.CustomWellFedTickDurations[ "Cooked Shrimp" ] = 15 * 60 * 60;				// 15 minutes
			this.CustomWellFedTickDurations[ "Sashimi" ] = 15 * 60 * 60;					// 15 minutes
			this.CustomWellFedTickDurations[ "Grub Soup" ] = 120 * 60 * 60;					// 120 minutes
			this.CustomWellFedTickDurations[ "Gingerbread Cookie" ] = 5 * 60 * 60;			// 5 minutes
			this.CustomWellFedTickDurations[ "Sugar Cookie" ] = 5 * 60 * 60;				// 5 minutes
			this.CustomWellFedTickDurations[ "Christmas Pudding" ] = 5 * 60 * 60;           // 5 minutes

			this.TupperwareDropsNpcIdsAndChances.Clear();
			this.TupperwareDropsNpcIdsAndChances[ NPCID.Skeleton ] = 0.35f;
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
					this.StarvationHarmRepeatDelayInTicks = this.StarvationHarmRate;
				}
			}
			if( versSince < new Version( 1, 3, 1 ) ) {
				if( this.FoodSpoilageRatePerSecond == 3f ) {
					this.FoodSpoilageRatePerSecond = newConfig.FoodSpoilageRatePerSecond;
				}
			}
			if( versSince < new Version( 1, 4, 0 ) ) {
				if( this.FoodSpoilageRatePerSecond == 2f ) {
					this.FoodSpoilageRatePerSecond = newConfig.FoodSpoilageRatePerSecond;
				}
				if( this.AddedWellFedDrainRatePerMaxHealthOver100 == ((1f + (1f/3f)) / 100f) ) {
					this.AddedWellFedDrainRatePerMaxHealthOver100 = newConfig.AddedWellFedDrainRatePerMaxHealthOver100;
				}
				if( this.WellFedAddedDrainPerTick == 4f ) {
					this.WellFedAddedDrainPerTick = newConfig.WellFedAddedDrainPerTick;
				}
			}
			if( versSince < new Version( 2, 0, 0 ) ) {
				this.SetDefaults();
			}
			if( versSince < new Version( 2, 1, 0) ) {
				if( this.CustomWellFedTickDurations.ContainsKey("Grub Soup") && this.CustomWellFedTickDurations["Grub Soup"] == (90 * 60 * 60) ) {
					this.CustomWellFedTickDurations["Grub Soup"] = newConfig.CustomWellFedTickDurations["Grub Soup"];
				}
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
