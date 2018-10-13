namespace ProjectFlight.Data
{
	/// <summary>
	/// Represents the response from the flight tracking API
	/// </summary>
	// TODO: Remove this class
	public class FlightInfoResponse
	{
		/// <summary>
		/// Identifier broadcast by the aircraft
		/// </summary>
		public string Icao;

		/// <summary>
		/// Aircraft registration number
		/// </summary>
		public string Reg;

		/// <summary>
		/// First seen
		/// </summary>
		public string Fseen;

		/// <summary>
		/// Seconds the aircraft has been tracked for
		/// </summary>
		public int Tsecs;

		/// <summary>
		/// Latitude over the ground
		/// </summary>
		public float Lat;

		/// <summary>
		/// Longitude over the ground
		/// </summary>
		public float Long;

		/// <summary>
		/// Time position was last reported
		/// </summary>
		public long PosTime;

		/// <summary>
		/// Speed in knots
		/// </summary>
		public float Spd;

		/// <summary>
		/// Speed type
		/// </summary>
		public int SpdTyp;

		/// <summary>
		/// Angle clockwise in degrees
		/// </summary>
		public float Trak;

		/// <summary>
		/// Aircraft's model
		/// </summary>
		public string Type;

		/// <summary>
		/// Description of aircraft's model
		/// </summary>
		public string Mdl;

		/// <summary>
		/// Manufacturer's name
		/// </summary>
		public string Man;

		/// <summary>
		/// Year manufactured
		/// </summary>
		public string Year;

		/// <summary>
		/// Name of aircraft's operator
		/// </summary>
		public string Op;

		/// <summary>
		/// Vertical speed in feet per minute
		/// </summary>
		public int Vsi;

		/// <summary>
		/// Aircraft type
		/// </summary>
		public int Species;

		/// <summary>
		/// Code and name of departure airport
		/// </summary>
		public string From;

		/// <summary>
		/// Code and name of destination airport
		/// </summary>
		public string To;

		/// <summary>
		/// If the aircraft is grounded
		/// </summary>
		public bool Gnd;

		/// <summary>
		/// Call sign of the aircraft
		/// </summary>
		public string Call;

		/// <summary>
		/// If the aircraft has a pic
		/// </summary>
		public bool HasPic;

		/// <summary>
		/// Number of flights recorded
		/// </summary>
		public int FlightsCount;
	}
}