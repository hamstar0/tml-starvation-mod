using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle _1, Color drawColor, Color _2, Vector2 _3, float scale ) {
			if( this.StoredItemCount == 0 ) { return; }

			Texture2D itemTex = Main.itemTexture[this.item.type];
			float freshness = this.ComputeFreshnessPercent();

			StarvationItem.DrawFreshnessGaugeInventory( sb, itemTex, pos, freshness, scale );

			if( freshness <= 0 ) {
				StarvationItem.DrawSpoilageInventory( sb, itemTex, pos, drawColor, scale );
			}
		}

		public override void PostDrawInWorld( SpriteBatch sb, Color lightColor, Color _1, float _2, float scale, int whoAmI ) {
			if( this.StoredItemCount == 0 ) { return; }

			float freshness = this.ComputeFreshnessPercent();

			if( freshness <= 0 ) {
				StarvationItem.DrawSpoilageWorld( sb, this.item.Center, lightColor, scale );
			}
		}
	}
}
