using System;


namespace Starvation {
	public static partial class StarvationAPI {
		internal static object Call( string callType, params object[] args ) {
			switch( callType ) {
			case "GetModSettings":
				return StarvationAPI.GetModSettings();
			case "SaveModSettingsChanges":
				StarvationAPI.SaveModSettingsChanges();
				return null;
			}

			throw new Exception( "No such api call " + callType );
		}
	}
}
