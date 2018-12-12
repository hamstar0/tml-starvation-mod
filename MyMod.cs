using HamstarHelpers.Components.Config;
using System;
using Terraria.ModLoader;


namespace Starvation {
	partial class StarvationMod : Mod {
		public static StarvationMod Instance { get; private set; }



		////////////////
		
		public JsonConfig<StarvationConfigData> ConfigJson { get; private set; }
		public StarvationConfigData Config { get { return this.ConfigJson.Data; } }



		////////////////

		public StarvationMod() {
			this.ConfigJson = new JsonConfig<StarvationConfigData>(
				StarvationConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath,
				new StarvationConfigData()
			);
		}

		////////////////

		public override void Load() {
			StarvationMod.Instance = this;

			this.LoadConfig();
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.Config.SetDefaults();
				this.ConfigJson.SaveFile();
				ErrorLogger.Log( "Starvation config " + this.Version.ToString() + " created." );
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Starvation updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}

		public override void Unload() {
			StarvationMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			if( args.Length == 0 ) { throw new Exception( "Undefined call type." ); }

			string callType = args[0] as string;
			if( args == null ) { throw new Exception( "Invalid call type." ); }

			var newArgs = new object[args.Length - 1];
			Array.Copy( args, 1, newArgs, 0, args.Length - 1 );

			return StarvationAPI.Call( callType, newArgs );
		}
	}
}
