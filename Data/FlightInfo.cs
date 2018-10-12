using System;
using System.ComponentModel.DataAnnotations;
using ProjectFlight.Enums;

namespace ProjectFlight.Data
{
	/// <summary>
	/// A more user friendly and descriptive version of <see cref="FlightInfoResponse"/>
	/// </summary>
	public class FlightInfo
	{
		/// <summary>
		/// Internal ID used by the database, primary key
		/// </summary>
		[Key]
		[MaxLength(8)]
		public string Id { get; set; }

		/// <summary>
		/// Identifier broadcast by the aircraft
		/// </summary>
		[MaxLength(6)]
		public string Identifier { get; set; }

		/// <summary>
		/// Aircraft registration number
		/// </summary>
		// TODO: Unknown length
		public string RegistrationNumber { get; set; }

		/// <summary>
		/// First seen
		/// </summary>
		public DateTime FirstSeen { get; set; }

		/// <summary>
		/// Time the aircraft has been tracked for
		/// </summary>
		public TimeSpan Tracked { get; set; }

		/// <summary>
		/// Latitude over the ground
		/// </summary>
		public float Latitude { get; set; }

		/// <summary>
		/// Longitude over the ground
		/// </summary>
		public float Longitude { get; set; }

		/// <summary>
		/// Time position was last reported
		/// </summary>
		public DateTime LastUpdate { get; set; }

		/// <summary>
		/// Speed in knots
		/// </summary>
		// TODO: Convert this to something useful
		public float Speed { get; set; }

		/// <summary>
		/// Speed type
		/// </summary>
		public ESpeedType SpeedType { get; set; }

		/// <summary>
		/// Angle clockwise in degrees
		/// </summary>
		public float Angle { get; set; }

		/// <summary>
		/// Aircraft's model
		/// </summary>
		// TODO: Unknown length
		public string Model { get; set; }

		/// <summary>
		/// Description of aircraft's model
		/// </summary>
		// TODO: Unknown length
		public string ModelDescription { get; set; }

		/// <summary>
		/// Manufacturer's name
		/// </summary>
		// TODO: Unknown length
		public string Manufacturer { get; set; }

		/// <summary>
		/// Year manufactured
		/// </summary>
		public short Year { get; set; }

		/// <summary>
		/// Name of aircraft's operator
		/// </summary>
		// TODO: Unknown length
		public string Operator { get; set; }

		/// <summary>
		/// Vertical speed in feet per minute
		/// </summary>
		// TODO: Convert this to something useful
		public int VerticalSpeed { get; set; }

		/// <summary>
		/// Aircraft type
		/// </summary>
		public EAircraftType Type { get; set; }

		/// <summary>
		/// Code and name of departure airport
		/// </summary>
		// TODO: Unknown length
		public string Departure { get; set; }

		/// <summary>
		/// Code and name of destination airport
		/// </summary>
		// TODO: Unknown length
		public string Destination { get; set; }

		/// <summary>
		/// If the aircraft is grounded
		/// </summary>
		public bool Grounded { get; set; }

		/// <summary>
		/// Call sign of the aircraft
		/// </summary>
		// TODO: Unknown length
		public string CallSign { get; set; }

		/// <summary>
		/// If the aircraft has a pic
		/// </summary>
		public bool HasPicture { get; set; }

		/// <summary>
		/// Number of flights recorded
		/// </summary>
		public int FlightsCount { get; set; }

		/// <summary>
		/// Default constructor used by database creation
		/// </summary>
		public FlightInfo()
		{
		}

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