using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public static void DrawContainerItemInventory( SpriteBatch sb, int storedItemType, Vector2 pos, float scale ) {
			Texture2D storedItemTex = Main.itemTexture[storedItemType];

			f
		}

		////

		public static void DrawItemStackInventory( SpriteBatch sb, Texture2D tupperTex, Vector2 pos, int stackSize, float scale ) {
			float posX = pos.X + ( ( ( (float)tupperTex.Width * scale ) * 0.5f ) - 16f );
			float posY = pos.Y + ( ( ( (float)tupperTex.Height * scale ) * 0.5f ) - 16f );

			var newPos = new Vector2(
				posX + 28f,
				posY + 28f
			);

			sb.DrawString( Main.fontItemStack, stackSize+"", newPos, Color.White );
		}



		////////////////

		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle _1, Color drawColor, Color _2, Vector2 _3, float scale ) {
			if( this.StoredItemStackSize == 0 ) { return; }

			Texture2D tupperTex = this.mod.GetTexture( this.Texture );
			float freshness = this.ComputeFreshnessPercent();

			if( freshness <= 0 ) {
				StarvationItem.DrawSpoilageInventory( sb, tupperTex, pos, drawColor, scale );
			} else {
				TupperwareItem.DrawContainerItemInventory( sb, this.StoredItemType, pos, scale );
				StarvationItem.DrawFreshnessGaugeInventory( sb, tupperTex, pos, freshness, scale );
			}

			TupperwareItem.DrawItemStackInventory( sb, tupperTex, pos, this.StoredItemStackSize, scale );
		}

		public override void PostDrawInWorld( SpriteBatch sb, Color lightColor, Color _1, float _2, float scale, int whoAmI ) {
			if( this.StoredItemStackSize == 0 ) { return; }

			float freshness = this.ComputeFreshnessPercent();

			if( freshness <= 0 ) {
				StarvationItem.DrawSpoilageWorld( sb, this.item.Center, lightColor, scale );
			}
		}
	}
}
