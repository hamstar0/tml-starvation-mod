using HamstarHelpers.Helpers.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation.Items {
	class UnlifeCrystalItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;



		////////////////

		public override void SetStaticDefaults() {
			var mymod = (StarvationMod)this.mod;

			this.DisplayName.SetDefault( "Unlife Crystal" );

			string tooltip = "Permanently decreases maximum life by 20";
			if( mymod.Config.UnlifeCrystalReturnsLifeCrystal ) {
				tooltip += "\nReturns a Life Crystal on use";
			}

			this.Tooltip.SetDefault( tooltip );
		}

		public override void SetDefaults() {
			this.item.width = UnlifeCrystalItem.Width;
			this.item.height = UnlifeCrystalItem.Height;
			this.item.consumable = true;
			this.item.useStyle = 4;
			this.item.useTime = 30;
			this.item.useAnimation = 30;
			this.item.UseSound = SoundID.Item4;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 1, 50, 0 );
			this.item.rare = 2;
		}

		public override bool UseItem( Player player ) {
			if( player.itemAnimation > 0 && player.itemTime == 0 ) {
				player.itemTime = item.useTime;
				return true;
			}
			return base.UseItem( player );
		}

		public override bool ConsumeItem( Player player ) {
			var mymod = (StarvationMod)this.mod;
			bool canUnheal = player.statLifeMax > 20;

			if( canUnheal ) {
				player.statLifeMax -= 20;

				if( mymod.Config.UnlifeCrystalReturnsLifeCrystal ) {
					Vector2 pos = player.Center - (new Vector2(UnlifeCrystalItem.Width, UnlifeCrystalItem.Height) / 2f);
					ItemHelpers.CreateItem( pos, ItemID.LifeCrystal, 1, UnlifeCrystalItem.Width, UnlifeCrystalItem.Height );
				}
			}

			return canUnheal;
		}

		////

		public override void AddRecipes() {
			var myrecipe = new UnlifeCrystalItemRecipe( this );
			myrecipe.AddRecipe();
		}
	}




	class UnlifeCrystalItemRecipe : ModRecipe {
		public UnlifeCrystalItemRecipe( UnlifeCrystalItem myitem ) : base( myitem.mod ) {
			var mymod = (StarvationMod)this.mod;

			this.AddTile( TileID.WorkBenches );

			this.AddIngredient( ItemID.FishingSeaweed, 1 );
			this.AddIngredient( ItemID.AshBlock, 1 );
			this.AddIngredient( ItemID.Gel, 10 );
			this.AddIngredient( ItemID.Glass, 10 );

			this.SetResult( myitem, 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = (StarvationMod)this.mod;
			return mymod.Config.CraftableUnlifeCrystal;
		}
	}
}
