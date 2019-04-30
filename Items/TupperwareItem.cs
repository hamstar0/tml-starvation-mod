using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	class TupperwareItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;



		////////////////

		public override void SetStaticDefaults() {
			var mymod = (StarvationMod)this.mod;

			this.DisplayName.SetDefault( "Tupperware" );

			string tooltip = "Stores a stack of a given food or ingredient";
			if( mymod.Config.FoodSpoilageEnabled ) {
				tooltip += "\nHelps preserve food against spoilage";
			}

			this.Tooltip.SetDefault( tooltip );
		}

		public override void SetDefaults() {
			this.item.width = TupperwareItem.Width;
			this.item.height = TupperwareItem.Height;
			this.item.maxStack = 30;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 ); // Sells for 1g, 50s
			this.item.rare = 2;
		}


		////////////////

		public override bool CanRightClick() {
			return base.CanRightClick();
		}

		public override void RightClick( Player player ) {
			base.RightClick( player );
		}
	}
}
