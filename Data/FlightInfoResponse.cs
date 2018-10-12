namespace ProjectFlight.Data
{
	/// <summary>
	/// Represents the response from the flight tracking API
	/// </summary>
	public struct FlightInfoResponse
	{
		/// <summary>
		/// Identifier broadcast by the aircraft
		/// </summary>
		public readonly string Icao;

		/// <summary>
		/// Aircraft registration number
		/// </summary>
		public readonly string Reg;

		/// <summary>
		/// First seen
		/// </summary>
		public readonly string Fseen;

		/// <summary>
		/// Seconds the aircraft has been tracked for
		/// </summary>
		public readonly int Tsecs;

		/// <summary>
		/// Latitude over the ground
		/// </summary>
		public readonly float Lat;

		/// <summary>
		/// Longitude over the ground
		/// </summary>
		public readonly float Long;

		/// <summary>
		/// Time position was last reported
		/// </summary>
		public readonly int PosTime;

		/// <summary>
		/// Speed in knots
		/// </summary>
		public readonly float Spd;

		/// <summary>
		/// Speed type
		/// </summary>
		public readonly int SpdTyp;

		/// <summary>
		/// Angle clockwise in degrees
		/// </summary>
		public readonly float Trak;

		/// <summary>
		/// Aircraft's model
		/// </summary>
		public readonly string Type;

		/// <summary>
		/// Description of aircraft's model
		/// </summary>
		public readonly string Mdl;

		/// <summary>
		/// Manufacturer's name
		/// </summary>
		public readonly string Man;

		/// <summary>
		/// Year manufactured
		/// </summary>
		public readonly int Year;

		/// <summary>
		/// Name of aircraft's operator
		/// </summary>
		public readonly string Op;

		/// <summary>
		/// Vertical speed in feet per minute
		/// </summary>
		public readonly int Vsi;

		/// <summary>
		/// Aircraft type
		/// </summary>
		public readonly int Species;

		/// <summary>
		/// Code and name of departure airport
		/// </summary>
		public readonly string From;

		/// <summary>
		/// Code and name of destination airport
		/// </summary>
		public readonly string To;

		/// <summary>
		/// If the aircraft is grounded
		/// </summary>
		public readonly bool Gnd;

		/// <summary>
		/// Call sign of the aircraft
		/// </summary>
		public readonly string Call;

		/// <summary>
		/// If the aircraft has a pic
		/// </summary>
		public readonly bool HasPic;

		/// <summary>
		/// Number of flights recorded
		/// </summary>
		public readonly int FlightsCount;
	}
}