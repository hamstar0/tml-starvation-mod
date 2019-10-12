using HamstarHelpers.Classes.UI.ModConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader.Config;


namespace Starvation {
	class MyFloatInputElement : FloatInputElement { }




	public class IntTickSetting {
		[Range( 0, 60 * 60 * 60 )]
		public int Ticks = 1;
	}




	public class StarvationConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		public bool DebugModeInfo = false;


		[Range( 0, 100 )]
		[DefaultValue( 1 )]
		public int WellFedAddedDrainPerTick = 1;

		[Range( 0, 1000 )]
		[DefaultValue( 1 )]
		public int StarvationHarm = 1;

		[Range( 0, 1000 )]
		[DefaultValue( 10 )]
		public int StarvationHarmRepeatDelayInTicks = 10;


		[Range( 0, 99 )]
		[DefaultValue( 3 )]
		public int PlayerStarterSoup = 3;


		[Range( 0, 60 * 60 * 60 * 24 )]
		[DefaultValue( 60 * 60 * 3 )]
		public int RespawnWellFedTickDuration = 60 * 60 * 3;    // 3 minutes


		[Range( 0f, 100f )]
		[DefaultValue( 1f / 200f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AddedWellFedDrainRatePerTickMultiplierPerMaxHealthOver100 = 1f / 200f;

		[Range( 0f, 100f )]
		[DefaultValue( 1f / 100f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AddedStarvationHarmPerTickMultiplierPerMaxHealthOver100 = 1f / 100f;


		public bool FoodSpoilageEnabled = false;

		[Range( 0f, 100f )]
		[DefaultValue( 2f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float FoodSpoilageDurationScale = 2f;

		[Range( 0, 60 * 60 * 60 * 24 * 7 )]
		[DefaultValue( 60 * 60 * 10 )]
		public int FoodSpoilageMinTickDuration = 60 * 60 * 10;  // 10 minutes

		[Range( 0, 60 * 60 * 60 * 24 * 7 )]
		[DefaultValue( 60 * 60 * 60 * 3 )]
		public int FoodSpoilageMaxTickDuration = 60 * 60 * 60 * 3;  // 3 hours


		[DefaultValue( true )]
		public bool FoodIngredientsAlsoSpoil = true;

		[Range( 0, 60 * 60 * 60 * 24 * 7 )]
		[DefaultValue( 30 * 60 * 60 )]
		public int FoodIngredientSpoilageTickDuration = 30 * 60 * 60;   // 30 minutes


		[DefaultValue( true )]
		public bool CraftableUnlifeCrystal = true;

		[DefaultValue( true )]
		public bool UnlifeCrystalReturnsLifeCrystal = true;


		public Dictionary<NPCDefinition, float> TupperwareDropsNpcIdsAndChances = new Dictionary<NPCDefinition, float>(); //NPCID.Skeleton : 0.35f;

		[Range( 0f, 100f )]
		[DefaultValue( 3f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float TupperwareSpoilageDurationScale = 3f;

		[Range( 1, 999 )]
		[DefaultValue( 30 )]
		public int TupperwareMaxStackSize = 30;

		public NPCDefinition TupperwareSellsFromMerchantByNpc;	//= NPCID.SkeletonMerchant;


		[DefaultValue( true )]
		public bool BugNetRecipe = true;

		[DefaultValue( true )]
		public bool CustomPumpkinPieRecipe = true;

		[DefaultValue( true )]
		public bool FishbowlToGoldfishRecipe = true;


		public Dictionary<ItemDefinition, IntTickSetting> PerItemWellFedTickDurations = new Dictionary<ItemDefinition, IntTickSetting>();


		////

		[Header( "\n \nOBSOLETE SETTINGS BELOW" )]
		public Dictionary<ItemDefinition, int> CustomWellFedTickDurations = new Dictionary<ItemDefinition, int>();


		////////////////

		public StarvationConfig() {
			this.TupperwareSellsFromMerchantByNpc = new NPCDefinition( NPCID.SkeletonMerchant );

			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.CookedMarshmallow) ] = new IntTickSetting { Ticks = 5 * 60 * 60 };	// 5 minutes
			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.BowlofSoup) ] = new IntTickSetting { Ticks = 45 * 60 * 60 };			// 45 minutes
			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.PumpkinPie) ] = new IntTickSetting { Ticks = 25 * 60 * 60 };			// 25 minutes
			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.CookedFish) ] = new IntTickSetting { Ticks = 15 * 60 * 60 };			// 15 minutes
			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.CookedShrimp) ] = new IntTickSetting { Ticks = 15 * 60 * 60 };		// 15 minutes
			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.Sashimi) ] = new IntTickSetting { Ticks = 15 * 60 * 60 };			// 15 minutes
			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.GrubSoup) ] = new IntTickSetting { Ticks = 120 * 60 * 60 };			// 120 minutes
			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.GingerbreadCookie) ] = new IntTickSetting { Ticks = 5 * 60 * 60 };	// 5 minutes
			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.SugarCookie) ] = new IntTickSetting { Ticks = 5 * 60 * 60 };			// 5 minutes
			this.PerItemWellFedTickDurations[ new ItemDefinition(ItemID.ChristmasPudding) ] = new IntTickSetting { Ticks = 5 * 60 * 60 };	// 5 minutes
			
			this.TupperwareDropsNpcIdsAndChances[ new NPCDefinition(NPCID.Skeleton) ] = 0.35f;
		}

		////////////////

		public override ModConfig Clone() {
			var clone = (StarvationConfig)base.Clone();

			clone.TupperwareSellsFromMerchantByNpc = this.TupperwareSellsFromMerchantByNpc;
			clone.PerItemWellFedTickDurations = this.PerItemWellFedTickDurations
				.ToDictionary( kv=>kv.Key, kv=>kv.Value );
			clone.TupperwareDropsNpcIdsAndChances = this.TupperwareDropsNpcIdsAndChances
				.ToDictionary( kv => kv.Key, kv => kv.Value );

			return clone;
		}
	}
}
