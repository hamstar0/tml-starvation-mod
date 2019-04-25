using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation.Items {
	class UnlifeCrystal : ModItem {
		public static int Width = 22;
		public static int Height = 22;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Unlife Crystal" );
			this.Tooltip.SetDefault( "Decreases maximum life by 20" );
		}

		public override void SetDefaults() {
			this.item.width = UnlifeCrystal.Width;
			this.item.height = UnlifeCrystal.Height;
			this.item.consumable = true;
			this.item.useStyle = 4;
			this.item.useTime = 30;
			this.item.useAnimation = 30;
			this.item.UseSound = SoundID.Item4;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 1, 50, 0 ); // Sells for 1g, 50s
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
			bool canUnheal = player.statLifeMax > 20;

			if( canUnheal ) {
				player.statLifeMax -= 20;
			}

			return canUnheal;
		}


		public override void AddRecipes() {
			var myrecipe = new UnlifeCrystalItemRecipe( this );
			myrecipe.AddRecipe();
		}
	}




	class UnlifeCrystalItemRecipe : ModRecipe {
		public UnlifeCrystalItemRecipe( UnlifeCrystal myitem ) : base( myitem.mod ) {
			var mymod = (StarvationMod)this.mod;

			this.AddTile( TileID.WorkBenches );

			this.AddIngredient( ItemID.Seaweed, 1 );
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
