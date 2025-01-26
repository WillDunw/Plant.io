using Schlime_Mobile_App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Location = Schlime_Mobile_App.Models.Location;

namespace Schlime_Mobile_App.Repos
{
    /*
     Team Name: Schlime
     Semester: Winter 2024
     Course: Application Development 3
     
     A mock datarepo for the geological information.
     */
    public class GeoLocationRepo
    {
        private string[] reading_types = { "pitch", "roll", "coordinates" };
        public ObservableCollection<Location> Locations { get; private set; }
        public ObservableCollection<Orientation> Orientations { get; private set; }
        public AReading pitch { get; set; }
        public AReading roll { get; set; }
        public AReading altitude { get; set; }
        public AReading longitude { get; set; }
        public AReading latitude { get; set; }

        public bool IsVibrating
        {
            get
            {
                if (Orientations.Count >= 2)
                {
                    return Orientations[Orientations.Count - 2].IsVibrating(Orientations.Last());
                }
                return false;
            }
        }

        public GeoLocationRepo()
        {
            //Locations = GenerateRandomLocations();
            //Orientations = GenerateRandomOrientations();
            Locations = new ObservableCollection<Location>();
            Orientations = new ObservableCollection<Orientation>();
        }
        public void GenerateReadingFromString(string readingType, string value, string unit)
        {
            try
            {
                bool newLocationDetected = false;
                bool newOrientationDetected = false;
                switch (readingType)
                {
                    case "pitch":
                        pitch = (new AReading(AReading.Unit.Angle, AReading.Type.Pitch, float.Parse(value), DateTime.Now));
                        newOrientationDetected = true;
                        break;
                    case "roll":
                        roll = (new AReading(AReading.Unit.Angle, AReading.Type.Roll, float.Parse(value), DateTime.Now));
                        newOrientationDetected = true;
                        break;
                    case "coordinates":
                        string coordinateType = unit;
                        switch (coordinateType)
                        {
                            case "altitude":
                                altitude = (new AReading(AReading.Unit.Coordinate, AReading.Type.Altitude, double.Parse(value), DateTime.Now));
                                newLocationDetected = true;
                                break;
                            case "longitude":
                                longitude = (new AReading(AReading.Unit.Coordinate, AReading.Type.Longitude, double.Parse(value), DateTime.Now));
                                newLocationDetected = true;
                                break;
                            case "latitude":
                                latitude = (new AReading(AReading.Unit.Coordinate, AReading.Type.Latitude, double.Parse(value), DateTime.Now));
                                newLocationDetected = true;
                                break;


                        }
                        break;

                }
                if (newLocationDetected && altitude != null && latitude != null && longitude != null)
                {
                    Location newLocation = new Location(altitude.Value, latitude.Value, longitude.Value);
                    AddLocation(newLocation);
                }
                if (newOrientationDetected && pitch != null && roll != null)
                {
                    Orientation newOrientation = new Orientation(pitch.Value, roll.Value);
                    AddOrientation(newOrientation);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public bool IsValidReading(string reading_type)
        {
            return reading_types.Contains(reading_type);
        }

        public void AddLocation(Location location)
        {
            Locations.Add(location);
        }

        public void AddOrientation(Orientation orientation)
        {
            Orientations.Add(orientation);
        }

        /// <summary>
        /// Generates a list of random locations.
        /// </summary>
        /// <returns>A list of Locations.</returns>
        public static List<Location> GenerateRandomLocations()
        {
            List<Location> randomLocations = new List<Location>();
            Random rand = new Random();

            for (int i = 0; i < 10; i++)
            {
                // Generate random altitude between 0 and 100
                float altitude = (float)(rand.NextDouble() * 100);

                // Generate random latitude between -90 and 90
                float latitude = (float)(rand.NextDouble() * 180 - 90);

                // Generate random longitude between -180 and 180
                float longitude = (float)(rand.NextDouble() * 360 - 180);

                // Create a new Location object with the random coordinates
                Location newLocation = new Location(altitude, latitude, longitude);
                randomLocations.Add(newLocation);
            }

            return randomLocations;
        }

        /// <summary>
        /// Generates a list of random orientations.
        /// </summary>
        /// <param name="numOrientations">The amount of orientations to generate.</param>
        /// <returns>A list of Orientations.</returns>
        public static List<Orientation> GenerateRandomOrientations(int numOrientations = 10)
        {
            Random random = new Random();
            List<Orientation> orientations = new List<Orientation>();
            // Generate random pitch and roll values between -180 and 180 degrees for the first orientation
            double pitchBase = random.NextDouble() * 360 - 180;
            double rollBase = random.NextDouble() * 360 - 180;
            for (int i = 0; i < numOrientations; i++)
            {
                // For the first three orientations, use the same pitch and roll values
                double pitch = (i < 3) ? pitchBase : random.NextDouble() * 360 - 180;
                double roll = (i < 3) ? rollBase : random.NextDouble() * 360 - 180;
                // Create the orientation object
                orientations.Add(new Orientation((float)pitch, (float)roll));
            }
            return orientations;
        }
    }
}
