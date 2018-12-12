using HamstarHelpers.Components.Network;
using HamstarHelpers.Components.Network.Data;


namespace Starvation.NetProtocols {
	class ModSettingsProtocol : PacketProtocol {
		public StarvationConfigData ModSettings;



		////////////////

		//protected ModSettingsProtocol( PacketProtocolDataConstructorLock ctorLock ) : base( ctorLock ) { }
		protected ModSettingsProtocol( PacketProtocolDataConstructorLock ctorLock ) { }

		////////////////

		protected override void SetServerDefaults( int who ) {
			this.ModSettings = StarvationMod.Instance.Config;
		}

		////////////////
		
		protected override void ReceiveWithClient() {
			StarvationMod.Instance.ConfigJson.SetData( this.ModSettings );
		}
	}
}
