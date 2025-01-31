from abc import ABC, abstractmethod
from enum import Enum


class AReading(ABC):
    """Abstract class for sensor readings. Can be instantiated directly or inherited.
    Also defines all possible types of readings and reading units using enums.
    """

    class Type(str, Enum):
        """Enum defining all possible types of readings that sensors might make.
        """
        # Add new reading types here.
        TEMPERATURE = 'temperature'
        HUMIDITY = 'humidity'
        MOISTURE = 'moisture'
        WATER_LEVEL = 'waterLevel'
        LUMINOSITY = 'luminosity'
        COORDINATES = 'coordinates'
        ACCELERATION = 'acceleration'
        PITCH = 'pitch'
        ROLL = 'roll'
        MOTION = 'motion'
        VOLTAGE = "voltage"
        DETECTION = "detection"

    class Unit(str, Enum):
        """Enum defining all possible units for sensor measuremens.
        """
        # Add new reading units here.
        MILLIMITERS = 'mm'
        CELCIUS = '°C'
        FAHRENHEIT = '°F'
        HUMIDITY = '% HR'
        UNITLESS = 'unitless'
        LATITUDE = 'latitude'
        LONGITUDE = 'longitude'
        ALTITUDE = 'altitude'
        ACCELERATIONX = 'accelerationx'
        ACCELERATIONY = 'accelerationy'
        ACCELERATIONZ = 'accelerationz'
        PITCH_ANGLE = 'pitch_angle'
        ROLL_ANGLE = 'roll_angle'
        DETECTED = 'detected'
        VOLTAGE = "Volts"
        
    # Class properties that must be defined in implementation classes
    reading_type: Type
    reading_unit: Unit
    value: float

    def __init__(self, type: Type, unit: Unit, value: float) -> None:
        self.reading_type = type
        self.reading_unit = unit
        self.value = value

    def __repr__(self) -> str:
        """String representation of a reading object
        """
        return f"{self.reading_type}: {self.value} {self.reading_unit}"


class ISensor(ABC):
    """Interface for all sensors.
    """

    # Class properties that must be defined in implementation classes
    _sensor_model: str
    reading_type: AReading.Type

    @abstractmethod
    def __init__(self, gpio: int,  model: str, type: AReading.Type):
        """Constructor for Sensor  class. May be called from childclass.

        :param str model: specific model of sensor hardware. Ex. AHT20 or LTR-303ALS-01
        :param ReadingType type: Type of reading this sensor produces. Ex. 'TEMPERATURE'
        """

    @abstractmethod
    def read_sensor(self) -> list[AReading]:
        """Takes a reading form the sensor

        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """
        pass
