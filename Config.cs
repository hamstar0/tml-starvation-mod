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
		public float FoodSpoilageRateScale = 0.5f;
		public int FoodSpoilageMinTickDuration = 60 * 60 * 10;  // 10 minutes
		public int FoodSpoilageMaxTickDuration = 60 * 60 * 60 * 3;	// 3 hours

		public bool FoodIngredientsAlsoSpoil = true;
		public int FoodIngredientSpoilageTickDuration = 30 * 60 * 60;   // 30 minutes
		public float FoodIngredientSpoilageRateScale = 1f;

		public bool CraftableUnlifeCrystal = true;
		public bool UnlifeCrystalReturnsLifeCrystal = true;

		public IDictionary<int, float> TupperwareDropsNpcIdsAndChances = new Dictionary<int, float>(); //NPCID.Skeleton : 0.35f;
		public float TupperwareSpoilageRateScale = 0.5f;
		public int TupperwareMaxStackSize = 30;
		public int TupperwareSellsFromMerchantByNpcId = NPCID.SkeletonMerchant;

		public bool CustomPumpkinPieRecipe = true;
		public IDictionary<string, int> CustomWellFedTickDurations = new Dictionary<string, int>();
		public bool FishbowlToGoldfishRecipe = true;


		////

		public string _OLD_SETTINGS_BELOW_ = "";

		[Obsolete( "use StarvationHarm", true )]
		public int StarvationHarmRate = 10;
		[Obsolete( "use AddedWellFedDrainRateMultiplierForMaxHealthOver100", true )]
		public float AddedWellFedDrainRatePerMaxHealthOver100 = (1f + (1f/3f)) / 100f;
		[Obsolete( "use AddedStarvationHarmMultiplierForMaxHealthOver100", true )]
		public float AddedStarvationHarmPerMaxHealthOver100 = (1f + (1f/3f)) / 100f;
		[Obsolete( "use WellFedAddedDrainPerTick", true )]
		public int WellFedDrainRate = 2;
		[Obsolete( "use StarvationHarmRepeatDelayInTicks", true )]
		public int StarvationHarmDelay = 10;
		[Obsolete( "use RespawnWellFedTickDuration", true )]
		public int RespawnWellFedDuration = 60 * 60 * 3;
		[Obsolete( "use FoodIngredientSpoilageTickDuration", true )]
		public int FoodIngredientSpoilageDuration = 30 * 60 * 60;
		[Obsolete( "use FoodSpoilageRateScale", true)]
		public float FoodSpoilageRate = 1f;
		[Obsolete( "use FoodIngredientSpoilageRateScale", true )]
		public float FoodIngredientSpoilageRatePerSecond = 1f;
		[Obsolete( "use FoodSpoilageRateScale", true )]
		public float FoodSpoilageRatePerSecond = 0.5f;
		[Obsolete( "use FoodSpoilageMinTickDuration", true )]
		public int FoodSpoilageMinDuration = 60 * 60 * 10;  // 10 minutes
		[Obsolete( "use FoodSpoilageMaxTickDuration", true )]
		public int FoodSpoilageMaxDuration = 60 * 60 * 60 * 3;  // 3 hours
		[Obsolete( "use TupperwareSpoilageRateScale", true )]
		public float TupperwareSpoilageRate = 0.5f;



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

			if( versSince < new Version(1, 4, 0) ) {
				if( this.WellFedAddedDrainPerTick == 4f ) {
					this.WellFedAddedDrainPerTick = newConfig.WellFedAddedDrainPerTick;
				}
			}
			if( versSince < new Version(2, 0, 0) ) {
				this.SetDefaults();
			}
			if( versSince < new Version(2, 1, 0) ) {
				if( this.CustomWellFedTickDurations.ContainsKey("Grub Soup") && this.CustomWellFedTickDurations["Grub Soup"] == (90 * 60 * 60) ) {
					this.CustomWellFedTickDurations["Grub Soup"] = newConfig.CustomWellFedTickDurations["Grub Soup"];
				}
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
