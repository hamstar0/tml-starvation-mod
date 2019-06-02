using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation.Items {
	partial class TupperwareItem : ModItem {
		public static void DrawContainerItemInventory( SpriteBatch sb, int storedItemType, Vector2 pos, float scale ) {
			if( storedItemType < 0 || storedItemType >= Main.itemTexture.Length ) { return; }

			var mymod = StarvationMod.Instance;
			Texture2D storedItemTex = Main.itemTexture[ storedItemType ];
			Texture2D tupperTex = ModLoader.GetTexture( mymod.GetItem<TupperwareItem>().Texture );
			if( tupperTex == null || storedItemTex == null ) { return; }

			float customScale = 0.75f;
			float storedWid = storedItemTex.Width * customScale;
			float storedHei = storedItemTex.Width * customScale;

			float halfTupperWid = ( (float)tupperTex.Width * scale ) * 0.5f;
			float halfTupperHei = ( (float)tupperTex.Height * scale ) * 0.5f;
			float halfStoredWid = ( (float)storedWid * scale ) * 0.5f;
			float halfStoredHei = ( (float)storedHei * scale ) * 0.5f;
			float posX = ( pos.X + halfTupperWid ) - halfStoredWid;
			float posY = ( pos.Y + halfTupperHei ) - halfStoredHei;

			var srcRect = new Rectangle( 0, 0, storedItemTex.Width, storedItemTex.Height );
			Color color = Color.White * 0.65f;
			float newScale = scale * customScale;

			sb.Draw( storedItemTex, new Vector2(posX, posY), srcRect, color, 0f, default(Vector2), newScale, SpriteEffects.None, 1f );
		}

		////

		public static void DrawItemStackInventory( SpriteBatch sb, Vector2 pos, int stackSize, float scale ) {
			var mymod = StarvationMod.Instance;
			Texture2D tupperTex = ModLoader.GetTexture( mymod.GetItem<TupperwareItem>().Texture );
			
			var newPos = new Vector2(
				pos.X,
				pos.Y + (20f * scale)
			);

			sb.DrawString( Main.fontItemStack, stackSize+"", newPos, Color.White, 0f, default(Vector2), scale, SpriteEffects.None, 1f );
		}



		////////////////

		public override void PostDrawInInventory( SpriteBatch sb, Vector2 pos, Rectangle _1, Color drawColor, Color _2, Vector2 _3, float scale ) {
			if( this.StoredItemStackSize == 0 ) { return; }

			Texture2D tupperTex = ModLoader.GetTexture( this.Texture );
			float freshnessPercent = this.ComputeContainedItemsFreshnessPercent();
			if( freshnessPercent == -1 ) {
				return;
			}

			if( freshnessPercent == 0 ) {
				StarvationItem.DrawSpoilageInventory( sb, tupperTex, pos, drawColor, scale );
			} else {
				TupperwareItem.DrawContainerItemInventory( sb, this.StoredItemType, pos, scale );
				StarvationItem.DrawFreshnessGaugeInventory( sb, tupperTex, pos, freshnessPercent, scale );
			}

			TupperwareItem.DrawItemStackInventory( sb, pos, this.StoredItemStackSize, scale );
		}

		public override void PostDrawInWorld( SpriteBatch sb, Color lightColor, Color _1, float _2, float scale, int whoAmI ) {
			if( this.StoredItemStackSize == 0 ) { return; }

			float freshnessPercent = this.ComputeContainedItemsFreshnessPercent();

			if( freshnessPercent == 0 ) {
				StarvationItem.DrawSpoilageWorld( sb, this.item.Center, lightColor, scale );
			}
		}
	}
}
