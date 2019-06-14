using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	class RotItem : ModItem {
		public static int Width = 16;
		public static int Height = 16;



		////////////////

		public static bool IsRotted( Item item ) {
			var myitem = item.GetGlobalItem<StarvationItem>();
			if( !myitem.NeedsSaving(item) ) {
				return false;
			}

			float timeLeftPercent;
			if( !myitem.ComputeTimeLeftPercent(item, out timeLeftPercent) ) {
				return false;
			}

			return timeLeftPercent <= 0;
		}



		////////////////

		public override void SetStaticDefaults() {
			var mymod = (StarvationMod)this.mod;

			this.DisplayName.SetDefault( "Rot" );

			this.Tooltip.SetDefault( "\"Who unplugged the refrigerator?!\"" );
		}

		public override void SetDefaults() {
			this.item.width = RotItem.Width;
			this.item.height = RotItem.Height;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 0, 0, 25 );
			this.item.rare = -1;
		}
	}
}
