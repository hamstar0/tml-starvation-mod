using HamstarHelpers.Helpers.Debug;
using Starvation.Items;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace Starvation {
	class BugNetItemRecipe : ModRecipe {
		public BugNetItemRecipe( StarvationMod mymod ) : base( mymod ) {
			this.AddTile( TileID.WorkBenches );

			this.AddIngredient( ItemID.Wood, 5 );
			this.AddIngredient( ItemID.Cobweb, 20 );

			this.SetResult( ItemID.BugNet, 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = StarvationMod.Instance;
			return mymod.Config.BugNetRecipe;
		}
	}




	class FishbowlToGoldfishItemRecipe : ModRecipe {
		public FishbowlToGoldfishItemRecipe( StarvationMod mymod ) : base( mymod ) {
			this.AddTile( TileID.WorkBenches );

			this.AddIngredient( ItemID.FishBowl );

			this.SetResult( ItemID.Goldfish );
		}


		public override bool RecipeAvailable() {
			var mymod = StarvationMod.Instance;
			return mymod.Config.FishbowlToGoldfishRecipe;
		}
	}




	partial class StarvationItem : GlobalItem {
		public static void AddNewRecipes() {
			var mymod = StarvationMod.Instance;

			var bugNetRecipe = new BugNetItemRecipe( mymod );
			bugNetRecipe.AddRecipe();

			var fishbowlRecipe = new FishbowlToGoldfishItemRecipe( mymod );
			fishbowlRecipe.AddRecipe();
		}

		public static void ApplyRecipeModifications() {
			var mymod = StarvationMod.Instance;

			if( mymod.Config.CustomPumpkinPieRecipe ) {
				StarvationItem.ApplyPumpkinPieRecipeModification();
			}
		}


		////

		private static void ApplyPumpkinPieRecipeModification() {
			var mymod = StarvationMod.Instance;

			for( int i=0; i<Main.recipe.Length; i++ ) {
				Recipe recipe = Main.recipe[i];

				if( recipe.createItem.type != ItemID.PumpkinPie ) {
					continue;
				}

				for( int j=0; j<recipe.requiredItem.Length; j++ ) {
					if( recipe.requiredItem[j].type == ItemID.Pumpkin ) {
						recipe.requiredItem[j] = new Item();
						recipe.requiredItem[j].SetDefaults( ModContent.ItemType<MashedPumpkinItem>() );
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



		////////////////

		public void ApplyWellFedModifiers( Item item ) {
			var mymod = (StarvationMod)this.mod;
			var itemDef = new ItemDefinition( item.type );

			if( mymod.Config.PerItemWellFedTickDurations.ContainsKey(itemDef) ) {
				item.buffTime = mymod.Config.PerItemWellFedTickDurations[ itemDef ].Ticks;
			}
		}
	}
}
