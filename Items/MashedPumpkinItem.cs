using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Starvation.Items {
	class MashedPumpkinItem : ModItem {
		public static int Width = 16;
		public static int Height = 16;



		////////////////

		public override void SetStaticDefaults() {
			var mymod = (StarvationMod)this.mod;

			this.DisplayName.SetDefault( "Mashed Pumpkin" );

			this.Tooltip.SetDefault( "Used for making pumpkin pie" );
		}

		public override void SetDefaults() {
			this.item.width = MashedPumpkinItem.Width;
			this.item.height = MashedPumpkinItem.Height;
			this.item.maxStack = 30;
			this.item.value = Item.buyPrice( 0, 0, 2, 50 );
			this.item.rare = 0;
		}

		////

		public override void AddRecipes() {
			var myrecipe = new ModRecipe( this.mod );
			myrecipe.AddTile( TileID.WorkBenches );
			myrecipe.AddIngredient( ItemID.Pumpkin, 10 );
			myrecipe.SetResult( this, 1 );
			myrecipe.AddRecipe();
		}
	}
}
