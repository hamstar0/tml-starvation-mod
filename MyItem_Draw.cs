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
				this.AddSpoilageTips( item, tooltips );
			}
		}


		////////////////

		public override bool PreDrawInWorld( Item item, SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI ) {
			if( !this.NeedsSaving( item ) ) {
				return true;
			}

			float freshness = (float)this.ComputeBuffTime( item ) / (float)item.buffTime;
			float spoilage = 1f - freshness;

			if( spoilage == 1f ) {
				Texture2D spoiledTex = this.mod.GetTexture( "Items/SpoiledFood" );
				Texture2D itemTex = Main.itemTexture[item.type];
				if( spoiledTex == null || itemTex == null ) {
					return true;
				}

				float halfSpoilWid = (float)spoiledTex.Width * scale * 0.5f;
				float halfSpoilHei = (float)spoiledTex.Height * scale * 0.5f;
				float posX = item.Center.X - halfSpoilWid;
				float posY = item.Center.Y - halfSpoilHei;

				var pos = new Vector2( posX, posY ) - Main.screenPosition;
				var srcRect = new Rectangle( 0, 0, spoiledTex.Width, spoiledTex.Height );

				sb.Draw( spoiledTex, pos, srcRect, lightColor, 0f, default(Vector2), scale, SpriteEffects.None, 1f );

				return false;
			}

			return true;
		}

		public override bool PreDrawInInventory( Item item, SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale ) {
			if( !this.NeedsSaving( item ) ) {
				return true;
			}

			float freshness = (float)this.ComputeBuffTime( item ) / (float)item.buffTime;
			float spoilage = 1f - freshness;

			if( spoilage == 1f ) {
				Texture2D spoiledTex = this.mod.GetTexture( "Items/SpoiledFood" );
				Texture2D itemTex = Main.itemTexture[item.type];
				if( spoiledTex == null || itemTex == null ) {
					return true;
				}

				float halfItemWid = ( (float)itemTex.Width * scale ) * 0.5f;
				float halfItemHei = ( (float)itemTex.Height * scale ) * 0.5f;
				float halfSpoilWid = ( (float)spoiledTex.Width * scale ) * 0.5f;
				float halfSpoilHei = ( (float)spoiledTex.Height * scale ) * 0.5f;
				float posX = (position.X + halfItemWid) - halfSpoilWid;
				float posY = (position.Y + halfItemHei) - halfSpoilHei;

				var srcRect = new Rectangle( 0, 0, spoiledTex.Width, spoiledTex.Height );

				sb.Draw( spoiledTex, new Vector2(posX, posY), srcRect, drawColor, 0f, default(Vector2), scale, SpriteEffects.None, 1f );

				return false;
			}

			return true;
		}

		////

		public override void PostDrawInInventory( Item item, SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale ) {
			if( !this.NeedsSaving( item ) ) {
				return;
			}
			if( item.type < 0 || item.type >= Main.itemTexture.Length ) {
				return;
			}

			Texture2D tex = Main.itemTexture[item.type];
			if( tex == null ) {
				return;
			}

			float freshness = (float)this.ComputeBuffTime( item ) / (float)item.buffTime;
			float spoilage = 1f - freshness;

			float barHeight = 32f;

//DebugHelpers.Print("draw", "pos:{"+position.X.ToString("N2")+","+position.Y.ToString("N2")+"}, scale:"+scale.ToString("N2"), 20 );
			float posX = position.X + ((((float)tex.Width * scale) * 0.5f) - 16f);
			float posY = position.Y + ((((float)tex.Height * scale) * 0.5f) - 16f);

			var barPos = new Vector2(
				posX + 28f,
				posY + ((barHeight * spoilage) - 1f)
			);
			var unbarPos = new Vector2(
				barPos.X + 1f,
				posY
			);
			
			var scales = new Vector2(
				4f,
				((barHeight * freshness) + 2f)
			);
			var unScales = new Vector2(
				2f,
				barHeight * spoilage
			);

			var color = Color.Lerp( Color.Lime, Color.Red, spoilage );
				
			sb.Draw( Main.magicPixel, unbarPos, new Rectangle(0,0,1,1), Color.Gray * 0.5f, 0f, default(Vector2), unScales, SpriteEffects.None, 1f );
			sb.Draw( Main.magicPixel, barPos, new Rectangle(0,0,1,1), color * 0.5f, 0f, default(Vector2), scales, SpriteEffects.None, 1f );
		}


		////////////////

		private void AddSpoilageTips( Item item, List<TooltipLine> tooltips ) {
			var mymod = (StarvationMod)this.mod;
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

			var tip1 = new TooltipLine( this.mod,
				"SpoilageRate",
				"Spoils for " + mymod.Config.FoodSpoilageRate + " seconds of 'Well Fed' duration for every second until use"
			);

			string tip2Text;
			Color tip2Color;
			if( spoilage < 1f ) {
				tip2Text = spoiledFmt + " of Well Fed duration lost.";
				tip2Color = Color.Lerp( Color.Lime, Color.Red, spoilage );
			} else {
				tip2Text = "Spoiled!";
				tip2Color = new Color( 64, 96, 32 );
			}

			var tip2 = new TooltipLine( this.mod, "SpoilageAmount", tip2Text );
			tip2.overrideColor = tip2Color;

			tooltips.Add( tip1 );
			tooltips.Add( tip2 );
		}
	}
}
