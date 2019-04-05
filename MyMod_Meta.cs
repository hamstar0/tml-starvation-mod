using HamstarHelpers.Components.Config;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-starvation-mod";

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + StarvationConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( !StarvationMod.Instance.ConfigJson.LoadFile() ) {
				StarvationMod.Instance.ConfigJson.SaveFile();
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var configData = new StarvationConfigData();
			configData.SetDefaults();

			StarvationMod.Instance.ConfigJson.SetData( configData );
			StarvationMod.Instance.ConfigJson.SaveFile();
		}
	}
}
