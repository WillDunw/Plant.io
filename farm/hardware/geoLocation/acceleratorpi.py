from enum import Enum
import time
import evdev
import math
from .sensors import ISensor, AReading
import seeed_python_reterminal.core as rt
import seeed_python_reterminal.acceleration as rt_accel

#Shoutout Cindy who gave me helpful ideas on how to do this class
class AcceleratorPi(ISensor):
    def __init__(self, gpio: int, model: str, type: AReading.Type):
        self.gpio = gpio
        self.model = model
        self.type = type      
        self.device = device = rt.get_acceleration_device()
        
    def read_sensor(self) -> list[AReading]:
        accel_readings = self.sort_readings(self.get_readings())

        readings = []
        readings.append(self.calculate_pitch(accel_readings[0].value,accel_readings[1].value,accel_readings[2].value))
        readings.append(self.calculate_roll(accel_readings[0].value,accel_readings[1].value,accel_readings[2].value))
        return readings
        
    def sort_readings(self, readings: list[AReading]) -> list[AReading]:
        x = {}
        y = {}
        z = {}
        for reading in readings:
            if reading.reading_unit == AReading.Unit.ACCELERATIONX:
                x = reading
            elif reading.reading_unit == AReading.Unit.ACCELERATIONY:
                y = reading
            elif reading.reading_unit == AReading.Unit.ACCELERATIONZ:
                z = reading
                
        return [x,y,z]

    def get_readings(self) -> list[AReading]:
        x = False
        y = False
        z = False
        readings = []
        for event in self.device.read_loop():
            accelEvent = rt_accel.AccelerationEvent(event)
            name = str(accelEvent.name)
            if name == "AccelerationName.X":
                if (not x) and (accelEvent.value != 0):
                    readings.append(AReading(AReading.Type.ACCELERATION, AReading.Unit.ACCELERATIONX, float(accelEvent.value)))
                    x = True
            elif name == "AccelerationName.Y":
                if (not y) and (accelEvent.value != 0):
                    readings.append(AReading(AReading.Type.ACCELERATION, AReading.Unit.ACCELERATIONY, float(accelEvent.value)))
                    y = True
            elif name == "AccelerationName.Z":
                if (not z) and (accelEvent.value != 0):
                    readings.append(AReading(AReading.Type.ACCELERATION, AReading.Unit.ACCELERATIONZ, float(accelEvent.value)))
                    z = True
                
            if x and y and z:
                return readings
            
    def calculate_pitch(self, x: float, y: float, z: float) -> AReading:
        pitch_angle = 180 * math.atan2(x, math.sqrt((y*y) + (z*z)))/math.pi
        return AReading(AReading.Type.PITCH, AReading.Unit.PITCH_ANGLE, pitch_angle)

    def calculate_roll(self, x: float, y: float, z: float) -> AReading:
        roll_angle = 180 * math.atan2(y, math.sqrt((x*x) + (z*z)))/math.pi
        return AReading(AReading.Type.ROLL, AReading.Unit.ROLL_ANGLE, roll_angle)