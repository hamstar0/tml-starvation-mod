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
			if( !mymod.Config.CustomPumpkinPieRecipe ) {
				return;
			}

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



		////////////////

		public void ApplyWellFedModifiers( Item item ) {
			var mymod = (StarvationMod)this.mod;
			string itemName = ItemIdentityHelpers.GetQualifiedName( item.type );

			if( mymod.Config.CustomWellFedTickDurations.ContainsKey(itemName) ) {
				item.buffTime = mymod.Config.CustomWellFedTickDurations[itemName];
			}
		}
	}
}
