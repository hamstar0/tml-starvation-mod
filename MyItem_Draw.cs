using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public static void DrawFreshnessGaugeInventory( SpriteBatch sb, Texture2D itemTex, Vector2 pos, float freshness, float scale ) {
			if( itemTex == null ) {
				return;
			}

//DebugHelpers.Print("draw", "pos:{"+position.X.ToString("N2")+","+position.Y.ToString("N2")+"}, scale:"+scale.ToString("N2"), 20 );
			float barHeight = 32f;
			float spoilage = 1f - freshness;

			float posX = pos.X + ( ( ( (float)itemTex.Width * scale ) * 0.5f ) - 16f );
			float posY = pos.Y + ( ( ( (float)itemTex.Height * scale ) * 0.5f ) - 16f );

			var barPos = new Vector2(
				posX + 28f,
				posY + ( ( barHeight * spoilage ) - 1f )
			);
			var unbarPos = new Vector2(
				barPos.X + 1f,
				posY
			);

			var scales = new Vector2(
				4f,
				( ( barHeight * freshness ) + 2f )
			);
			var unScales = new Vector2(
				2f,
				barHeight * spoilage
			);

			var color = Color.Lerp( Color.Lime, Color.Red, spoilage );

			sb.Draw( Main.magicPixel, unbarPos, new Rectangle( 0, 0, 1, 1 ), Color.Gray * 0.5f, 0f, default( Vector2 ), unScales, SpriteEffects.None, 1f );
			sb.Draw( Main.magicPixel, barPos, new Rectangle( 0, 0, 1, 1 ), color * 0.5f, 0f, default( Vector2 ), scales, SpriteEffects.None, 1f );
		}

		////

		public static bool DrawSpoilageInventory( SpriteBatch sb, Texture2D itemTex, Vector2 pos, Color color, float scale ) {
			Texture2D spoiledTex = StarvationMod.Instance.GetTexture( "Items/SpoiledFood" );
			if( spoiledTex == null || itemTex == null ) {
				return false;
			}

			float halfItemWid = ( (float)itemTex.Width * scale ) * 0.5f;
			float halfItemHei = ( (float)itemTex.Height * scale ) * 0.5f;
			float halfSpoilWid = ( (float)spoiledTex.Width * scale ) * 0.5f;
			float halfSpoilHei = ( (float)spoiledTex.Height * scale ) * 0.5f;
			float posX = ( pos.X + halfItemWid ) - halfSpoilWid;
			float posY = ( pos.Y + halfItemHei ) - halfSpoilHei;

			var srcRect = new Rectangle( 0, 0, spoiledTex.Width, spoiledTex.Height );

			sb.Draw( spoiledTex, new Vector2( posX, posY ), srcRect, color, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );

			return true;
		}

		public static bool DrawSpoilageWorld( SpriteBatch sb, Vector2 centerPos, Color color, float scale ) {
			Texture2D spoiledTex = StarvationMod.Instance.GetTexture( "Items/SpoiledFood" );
			if( spoiledTex == null ) {
				return false;
			}
			
			float halfSpoilWid = (float)spoiledTex.Width * scale * 0.5f;
			float halfSpoilHei = (float)spoiledTex.Height * scale * 0.5f;
			float posX = centerPos.X - halfSpoilWid;
			float posY = centerPos.Y - halfSpoilHei;

			var pos = new Vector2( posX, posY ) - Main.screenPosition;
			var srcRect = new Rectangle( 0, 0, spoiledTex.Width, spoiledTex.Height );

			sb.Draw( spoiledTex, pos, srcRect, color, 0f, default( Vector2 ), scale, SpriteEffects.None, 1f );

			return true;
		}



		////////////////
		
		public override bool PreDrawInWorld( Item item, SpriteBatch sb, Color lightColor, Color _1, ref float _2, ref float scale,
				int whoAmI ) {
			if( !this.NeedsSaving( item ) ) {
				return true;
			}

			//float freshness = (float)this.ComputeRemainingBuffTime( item ) / (float)item.buffTime;
			float freshness = this.ComputeRemainingBuffTime(item) / (float)this.ComputeMaxSpoilageDuration(item);

			if( freshness <= 0f ) {
				StarvationItem.DrawSpoilageWorld( sb, item.Center, lightColor, scale );
				return false;
			}

			return true;
		}

		public override bool PreDrawInInventory( Item item, SpriteBatch sb, Vector2 pos, Rectangle frame, Color drawColor, Color itemColor,
				Vector2 origin, float scale ) {
			if( !this.NeedsSaving( item ) ) {
				return true;
			}
			
			float freshness = this.ComputeRemainingBuffTime(item) / (float)this.ComputeMaxSpoilageDuration(item);

			if( freshness <= 0f ) {
				StarvationItem.DrawSpoilageInventory( sb, Main.itemTexture[item.type], pos, drawColor, scale );
				return false;
			}

			return true;
		}

		////

		public override void PostDrawInInventory( Item item, SpriteBatch sb, Vector2 pos, Rectangle _1, Color _2, Color _3, Vector2 _4,
				float scale ) {
			if( !this.NeedsSaving( item ) ) {
				return;
			}
			if( item.type < 0 || item.type >= Main.itemTexture.Length ) {
				return;
			}
			
			float freshness = this.ComputeRemainingBuffTime(item) / (float)this.ComputeMaxSpoilageDuration(item);

			StarvationItem.DrawFreshnessGaugeInventory( sb, Main.itemTexture[item.type], pos, freshness, scale );
		}
	}
}
