using System;
using ProjectFlight.Enums;

namespace ProjectFlight.Data
{
	/// <summary>
	/// A more user friendly and descriptive version of <see cref="FlightInfoResponse"/>
	/// </summary>
	public class FlightInfo
	{
		/// <summary>
		/// Identifier broadcast by the aircraft
		/// </summary>
		public readonly string Identifier;

		/// <summary>
		/// Aircraft registration number
		/// </summary>
		public readonly string RegistrationNumber;

		/// <summary>
		/// First seen
		/// </summary>
		public readonly DateTime FirstSeen;

		/// <summary>
		/// Time the aircraft has been tracked for
		/// </summary>
		public readonly TimeSpan Tracked;

		/// <summary>
		/// Latitude over the ground
		/// </summary>
		public readonly float Latitude;

		/// <summary>
		/// Longitude over the ground
		/// </summary>
		public readonly float Longitude;

		/// <summary>
		/// Time position was last reported
		/// </summary>
		public readonly DateTime LastUpdate;

		/// <summary>
		/// Speed in knots
		/// </summary>
		// TODO: Convert this to something useful
		public readonly float Speed;

		/// <summary>
		/// Speed type
		/// </summary>
		public readonly ESpeedType SpeedType;

		/// <summary>
		/// Angle clockwise in degrees
		/// </summary>
		public readonly float Angle;

		/// <summary>
		/// Aircraft's model
		/// </summary>
		public readonly string Model;

		/// <summary>
		/// Description of aircraft's model
		/// </summary>
		public readonly string ModelDescription;

		/// <summary>
		/// Manufacturer's name
		/// </summary>
		public readonly string Manufacturer;

		/// <summary>
		/// Year manufactured
		/// </summary>
		public readonly int Year;

		/// <summary>
		/// Name of aircraft's operator
		/// </summary>
		public readonly string Operator;

		/// <summary>
		/// Vertical speed in feet per minute
		/// </summary>
		// TODO: Convert this to something useful
		public readonly int VerticalSpeed;

		/// <summary>
		/// Aircraft type
		/// </summary>
		public readonly EAircraftType Type;

		/// <summary>
		/// Code and name of departure airport
		/// </summary>
		public readonly string Departure;

		/// <summary>
		/// Code and name of destination airport
		/// </summary>
		public readonly string Destination;

		/// <summary>
		/// If the aircraft is grounded
		/// </summary>
		public readonly bool Grounded;

		/// <summary>
		/// Call sign of the aircraft
		/// </summary>
		public readonly string CallSign;

		/// <summary>
		/// If the aircraft has a pic
		/// </summary>
		public readonly bool HasPicture;

		/// <summary>
		/// Number of flights recorded
		/// </summary>
		public readonly int FlightsCount;

		/// <summary>
		/// Constructs from a <see cref="FlightInfoResponse"/>
		/// </summary>
		/// <param name="info"></param>
		public FlightInfo(FlightInfoResponse info)
		{
			Identifier         = info.Icao;
			RegistrationNumber = info.Reg;
			// TODO: This will probably fail
			FirstSeen          = DateTime.Parse(info.Fseen);
			Tracked            = TimeSpan.FromSeconds(info.Tsecs);
			Latitude           = info.Lat;
			Longitude          = info.Long;
			// TODO: This might be incorrect
			LastUpdate         = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
				.AddSeconds(info.PosTime);
			Speed              = info.Spd;
			SpeedType          = (ESpeedType) info.SpdTyp;
			Angle              = info.Trak;
			Model              = info.Type;
			ModelDescription   = info.Mdl;
			Manufacturer       = info.Man;
			Year               = info.Year;
			Operator           = info.Op;
			VerticalSpeed      = info.Vsi;
			Type               = (EAircraftType) info.Species;
			Departure          = info.From;
			Destination        = info.To;
			Grounded           = info.Gnd;
			CallSign           = info.Call;
			HasPicture         = info.HasPic;
			FlightsCount       = info.FlightsCount;
		}
	}
}