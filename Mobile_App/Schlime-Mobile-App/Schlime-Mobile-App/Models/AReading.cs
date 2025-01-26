using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Models
{
    public class AReading
    {

        public enum Type
        {
            Humidity,
            Temperature,
            WaterLevel,
            Altitude,
            Longitude,
            Latitude,
            Motion,
            Moisture,
            Voltage,
            Detection,
            Pitch,
            Roll
        }

        public enum Unit
        {
            Coordinate,
            Percent,
            Celsius,
            Detected,
            Volts,
            Angle
        }

        private double _value;
        private Type _type;
        private Unit _unit;

        public double Value { get { return _value; } set { _value = value; } }
        public Type ReadingType { get { return _type; } set { _type = value; } }
        public Unit ReadingUnit { get { return _unit; } set { _unit = value; } }
        public DateTime Time { get; set; }

        public AReading(Unit unit, Type type, double value, DateTime time)
        {
            Value = value;
            ReadingUnit = unit;
            ReadingType = type;
            Time = time;
        }
    }
}