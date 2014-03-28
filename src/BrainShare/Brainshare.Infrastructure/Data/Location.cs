using System;
using System.Globalization;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Brainshare.Infrastructure.Data
{
    public struct Location : IBsonSerializable
    {
        public const double LatitudeMinValue = -90;
        public const double LatitudeMaxValue = 90;
        public const double LongitudeMinValue = -180;
        public const double LongitudeMaxValue = 180;

        private const double PIx = 3.141592653589793;
        public const double EarthRaduisInKm = 6378.16;
        public const double EarthRaduisInMiles = 3959;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location(double latitude, double longitude)
            : this()
        {
            Latitude = latitude;
            Longitude = longitude;
            Validate(this);
        }

        public static Location Parse(string latitude, string longitude)
        {
            var result = new Location(double.Parse(latitude, CultureInfo.InvariantCulture), double.Parse(longitude, CultureInfo.InvariantCulture));
            Validate(result);
            return result;
        }

        /// <summary>
        /// Convert degrees to Radians
        /// </summary>
        /// <param name="x">Degrees</param>
        /// <returns>The equivalent in radians</returns>
        public static double Radians(double x)
        {
            return x * PIx / 180;
        }

        public static double DisnatceInMiles(Location x, Location y)
        {
            return Disnatce(x, y, EarthRaduisInMiles);
        }

        public static double Disnatce(Location x, Location y, double radius)
        {
            double dlon = Radians(x.Longitude - y.Longitude);
            double dlat = Radians(x.Latitude - y.Latitude);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(x.Latitude)) * Math.Cos(Radians(y.Latitude)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * radius;
        }

        private static void Validate(Location location)
        {
            Validate(location.Latitude, location.Longitude);
        }

        private static void Validate(double latitude, double longitude)
        {
            if (latitude >= LatitudeMinValue && latitude <= LatitudeMaxValue && longitude >= LongitudeMinValue && longitude <= LongitudeMaxValue)
            {
                return;
            }
            throw new ArgumentOutOfRangeException(String.Format("Point ({0},{1}) must be in earth-like bounds of long : [-180, 180), lat : [-90, 90] ", latitude, longitude));
        }

        public string GetLatitudeString()
        {
            return Latitude.ToString(CultureInfo.InvariantCulture);
        }

        public string GetLongitudeString()
        {
            return Longitude.ToString(CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return string.Format("{0},{1}", GetLatitudeString(), GetLongitudeString());
        }

        public static bool TryParse(string latitude, string longitude, out Location location)
        {
            try
            {
                location = Parse(latitude, longitude);
                return true;
            }
            catch
            {
                location = default(Location);
                return false;
            }
        }

        public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            if (nominalType != typeof(Location))
                throw new ArgumentException("Cannot deserialize anything but self");
            var ser = new ArraySerializer<double>();
            var arr = ((double[])ser.Deserialize(bsonReader, typeof(double[]), options));
            return new Location(arr[1], arr[0]);
        }

        public bool GetDocumentId(out object id, out Type idNominalType, out IIdGenerator idGenerator)
        {
            id = null;
            idGenerator = null;
            idNominalType = null;
            return false;
        }

        public void Serialize(BsonWriter bsonWriter, Type nominalType, IBsonSerializationOptions options)
        {
            if (nominalType != typeof(Location))
                throw new ArgumentException("Cannot serialize anything but self");
            var ser = new ArraySerializer<double>();
            ser.Serialize(bsonWriter, typeof(double[]),
                          new[] { Longitude, Latitude }, options);
        }

        public void SetDocumentId(object id)
        {
            return;
        }
    }
}