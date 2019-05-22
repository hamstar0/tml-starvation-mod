using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.ItemHelpers;
using Starvation.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public static void ApplyRecipeMods() {
			var mymod = StarvationMod.Instance;

			if( mymod.Config.CustomPumpkinPieRecipe ) {
				StarvationItem.ApplyCustomPumpkinPieRecipe();
			}
		}

		public static void ApplyNewRecipes() {
			var mymod = StarvationMod.Instance;

			if( mymod.Config.FishbowlToGoldfishRecipe ) {
				StarvationItem.AddCustomFishbowlToGoldfishRecipe();
			}
		}


		////

		private static void ApplyCustomPumpkinPieRecipe() {
			var mymod = StarvationMod.Instance;

			for( int i=0; i<Main.recipe.Length; i++ ) {
				Recipe recipe = Main.recipe[i];

				if( recipe.createItem.type != ItemID.PumpkinPie ) {
					continue;
				}

				for( int j=0; j<recipe.requiredItem.Length; j++ ) {
					if( recipe.requiredItem[j].type == ItemID.Pumpkin ) {
						recipe.requiredItem[j] = new Item();
						recipe.requiredItem[j].SetDefaults( mymod.ItemType<MashedPumpkinItem>() );
						continue;
					}

					if( recipe.requiredItem[j].IsAir ) {
						recipe.requiredItem[j] = new Item();
						recipe.requiredItem[j].SetDefaults( ItemID.Hay );
						recipe.requiredItem[j].stack = 10;

						recipe.requiredItem[j+1] = new Item();
						recipe.requiredItem[j+1].SetDefaults( ItemID.BlinkrootSeeds );
						recipe.requiredItem[j+1].stack = 1;
						break;
					}
				}
				break;
			}
		}


		private static void AddCustomFishbowlToGoldfishRecipe() {
			var mymod = StarvationMod.Instance;
			var myrecipe = new ModRecipe( mymod );
			myrecipe.AddIngredient( ItemID.FishBowl );
			myrecipe.AddTile( TileID.WorkBenches );
			myrecipe.SetResult( ItemID.Goldfish );
			myrecipe.AddRecipe();
		}



		////////////////

		public void ApplyWellFedModifiers( Item item ) {
			var mymod = (StarvationMod)this.mod;
			string itemName = ItemIdentityHelpers.GetQualifiedName( item.type );

			if( mymod.Config.CustomWellFedTickDurations.ContainsKey(itemName) ) {
				item.buffTime = mymod.Config.CustomWellFedTickDurations[ itemName ];
			}
		}
	}
}
