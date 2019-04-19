using HamstarHelpers.Helpers.DebugHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			if( this.NeedsSaving( item ) ) {
				this.AddSpoilageTip( item, tooltips );
			}
		}

		
		public override void PostDrawInInventory( Item item, SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale ) {
			if( this.NeedsSaving( item ) ) {
				if( item.type < 0 || item.type >= Main.itemTexture.Length ) { return; }
				Texture2D tex = Main.itemTexture[item.type];
				if( tex == null ) { return; }

				float freshness = (float)this.ComputeBuffTime( item ) / (float)item.buffTime;
				float spoilage = 1f - freshness;

				float barHeight = 32f;

//DebugHelpers.Print("draw", "pos:{"+position.X.ToString("N2")+","+position.Y.ToString("N2")+"}, scale:"+scale.ToString("N2"), 20 );
				float posX = position.X + ((((float)tex.Width * scale) / 2f) - 16f);
				float posY = position.Y + ((((float)tex.Height * scale) / 2f) - 16f);
				var barPos = new Vector2(
					posX + 28f,
					posY + ((barHeight * spoilage) - 1f)
				);
				var scales = new Vector2(
					4f,
					((barHeight * freshness) + 2f)
				);
				var color = Color.Lerp( Color.Lime, Color.Red, spoilage );

				var unbarPos = new Vector2( barPos.X + 1f, posY );
				var unScales = new Vector2( 2f, (barHeight * spoilage) );
				
				sb.Draw( Main.magicPixel, unbarPos, new Rectangle(0,0,1,1), Color.Gray * 0.5f, 0f, default(Vector2), unScales, SpriteEffects.None, 1f );
				sb.Draw( Main.magicPixel, barPos, new Rectangle(0,0,1,1), color * 0.5f, 0f, default(Vector2), scales, SpriteEffects.None, 1f );
			}
		}


		////////////////

		private void AddSpoilageTip( Item item, List<TooltipLine> tooltips ) {
			int buffTime = this.ComputeBuffTime( item );
			float freshness = (float)buffTime / (float)item.buffTime;
			float spoilage = 1f - freshness;

			int spoiledAmt = ( item.buffTime - buffTime ) / 60;
			string spoiledFmt;

			if( spoiledAmt <= 60 ) {
				spoiledFmt = spoiledAmt + "s";
			} else {
				spoiledFmt = ( spoiledAmt / 60 ) + "m";
			}

			var tip = new TooltipLine( this.mod, "SpoilageTip", spoiledFmt + " of Well Fed duration lost." );
			tip.overrideColor = Color.Lerp( Color.Lime, Color.Red, spoilage );

			tooltips.Add( tip );
		}
	}
}
