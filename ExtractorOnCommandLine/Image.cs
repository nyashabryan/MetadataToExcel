using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtractorOnCommandLine
{
    class Image
    {
        private readonly string ImageLocation;
        private readonly string ImageName;
        private readonly int ImageSize;
        private readonly DateTime DateTaken;
        private readonly Device CameraDevice;
        private readonly GPS Location;
        private readonly string UniqueID;
        
        public Image()
        {
            ;
        }

        public Image(string imageLocation, string imageName, int imageSize,
            DateTime dateTaken, Device cameraDevice, GPS location,
            string uniqueID)
        {
            this.ImageLocation = imageLocation;
            this.ImageName = imageName;
            this.ImageSize = imageSize;
            this.DateTaken = dateTaken;
            this.CameraDevice = cameraDevice;
            this.Location = location;
            this.UniqueID = uniqueID;
        }

        private DateTime MakeDate(string dateTaken)
        {
            return new DateTime();
        }

        override
        public string ToString()
        {
            return this.ImageLocation;
        }

        public class GPS
        {
            private readonly decimal longitude;
            private readonly decimal latitude;

            private GPS(decimal longitude, decimal latitude)
            {
                this.longitude = longitude;
                this.latitude = latitude;
            }

            public static GPS FromDegrees(decimal longitude, decimal latitude)
            {
                return new GPS(longitude, latitude);
            }
            
            public static GPS FromDegrees(string location)
            {
                var r = new Regex(@"^[0-9]+.[0-9]+. \w [0-9]+.[0-9]+. \w$");
                if (!r.IsMatch(location)) throw new SystemException(
                    "Invalid format of GPS coordinates.");
                string[] parts = location.Split(' ');
                string longitudeSign = "";
                string latitudeSign = "";
                if (parts[0].Equals('W'))
                {
                    longitudeSign = "-";
                }
                if (parts[1].Equals('S'))
                {
                    latitudeSign = "-";
                }
                return new GPS(
                    decimal.Parse($"{longitudeSign}{parts[0]}"),
                    decimal.Parse($"{latitudeSign}{parts[2]}")
                    );
            }

            public static GPS FromDegreesSecondsMinutes(string location)
            {
                var r = new Regex(
                    @"^([0-9]+. [0-9]+. [0-9]+. \w( )*)+$");
                var r2 = new Regex(
                    @"^([0-9]+.[0-9]+.[0-9]+. \w( )*)+$");
                if (r2.IsMatch(location))
                {
                    string[] parts = location.Split(' ');
                    if (!parts.Length.Equals(8))
                        throw new SystemException("Invalid format of GPS coordinates.");

                    decimal longitude = GPS.DegreesMinutesSecondsToDegrees(
                        decimal.Parse(parts[0]),
                        decimal.Parse(parts[1]),
                        decimal.Parse(parts[2])
                        );

                    decimal latitude = GPS.DegreesMinutesSecondsToDegrees(
                        decimal.Parse(parts[4]),
                        decimal.Parse(parts[5]),
                        decimal.Parse(parts[6])
                        );

                    if (parts[3].Equals('W'))
                        longitude *= -1;
                    if (parts[7].Equals('S'))
                        latitude *= -1;

                    return new GPS(longitude, latitude);
                }
                else
                {
                    throw new SystemException("Invalid format of GPS coordinates.");
                }
            }

            public static decimal DegreesMinutesSecondsToDegrees(
                decimal degrees, decimal minutes, decimal seconds
                )
            {
                return degrees + minutes / 60m + seconds / 3600m;
            }
        }

        public class Device
        {
            private readonly string device;
            private readonly string model;
            private readonly string software;

            Device() : this("", "", "")
            {

            }

            Device(string device, string model, string software)
            {
                this.device = device;
                this.model = model;
                this.software = software;
            }

            override
            public string ToString()
            {
                return $"{this.device} : {this.model} : {this.software}";
            }
        }
    }
}
