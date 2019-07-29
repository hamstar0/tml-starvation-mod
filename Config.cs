using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Starvation {
	public class StarvationConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		public bool DebugModeInfo = false;


		[DefaultValue( 1 )]
		public int WellFedAddedDrainPerTick = 1;

		[DefaultValue( 1 )]
		public int StarvationHarm = 1;

		[DefaultValue( 10 )]
		public int StarvationHarmRepeatDelayInTicks = 10;


		[DefaultValue( 3 )]
		public int PlayerStarterSoup = 3;


		[DefaultValue( 60 * 60 * 3 )]
		public int RespawnWellFedTickDuration = 60 * 60 * 3;    // 3 minutes


		[DefaultValue( 1f / 200f )]
		public float AddedWellFedDrainRatePerTickMultiplierPerMaxHealthOver100 = 1f / 200f;

		[DefaultValue( 1f / 100f )]
		public float AddedStarvationHarmPerTickMultiplierPerMaxHealthOver100 = 1f / 100f;


		public bool FoodSpoilageEnabled = false;

		[DefaultValue( 2f )]
		public float FoodSpoilageDurationScale = 2f;

		[DefaultValue( 60 * 60 * 10 )]
		public int FoodSpoilageMinTickDuration = 60 * 60 * 10;  // 10 minutes

		[DefaultValue( 60 * 60 * 60 * 3 )]
		public int FoodSpoilageMaxTickDuration = 60 * 60 * 60 * 3;  // 3 hours


		[DefaultValue( true )]
		public bool FoodIngredientsAlsoSpoil = true;

		[DefaultValue( 30 * 60 * 60 )]
		public int FoodIngredientSpoilageTickDuration = 30 * 60 * 60;   // 30 minutes


		[DefaultValue( true )]
		public bool CraftableUnlifeCrystal = true;

		[DefaultValue( true )]
		public bool UnlifeCrystalReturnsLifeCrystal = true;


		public IDictionary<int, float> TupperwareDropsNpcIdsAndChances = new Dictionary<int, float>(); //NPCID.Skeleton : 0.35f;

		[DefaultValue( 3f )]
		public float TupperwareSpoilageDurationScale = 3f;

		[DefaultValue( 30 )]
		public int TupperwareMaxStackSize = 30;

		public NPCDefinition TupperwareSellsFromMerchantByNpcId;	//= NPCID.SkeletonMerchant;


		[DefaultValue( true )]
		public bool BugNetRecipe = true;

		[DefaultValue( true )]
		public bool CustomPumpkinPieRecipe = true;

		[DefaultValue( true )]
		public bool FishbowlToGoldfishRecipe = true;


		public IDictionary<string, int> CustomWellFedTickDurations = new Dictionary<string, int>();



		////////////////

		[OnDeserialized]
		internal void OnDeserializedMethod( StreamingContext context ) {
			if( this.CustomWellFedTickDurations != null ) {
				return;
			}

			this.TupperwareSellsFromMerchantByNpcId = new NPCDefinition( "Terraria", "SkeletonMerchant" );

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
	}
}
