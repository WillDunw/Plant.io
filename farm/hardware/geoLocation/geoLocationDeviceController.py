from .acceleratorpi import AcceleratorPi
from .gpshardware import GPSHardware 
# from subsystem import ISubsystem
from .sensors import AReading
from .actuators import ACommand
import time

class GeoLocationSubsystem():
    def __init__(self) -> None:
        self.initialize_actuators()
        self.initialize_sensors()
        
    def initialize_sensors(self) -> None:
        self.sensors = [
            GPSHardware(1, "Air530", AReading.Type.COORDINATES),
            AcceleratorPi(1, "reTerminal", AReading.Type.ACCELERATION)
        ]
        
    def read_sensors(self) -> list[AReading]:
        readings = []
        for sensor in self.sensors:
            r = sensor.read_sensor()
            if r != None:
                readings.extend(r)

        # readings.append(AReading(AReading.Type.ACCELERATION, AReading.Unit.ACCELERATIONX, -47.0))
        # readings.append(AReading(AReading.Type.ACCELERATION, AReading.Unit.ACCELERATIONY, 8.0))
        # readings.append(AReading(AReading.Type.ACCELERATION, AReading.Unit.ACCELERATIONZ, -1109.0))
        # readings.append(AReading(AReading.Type.COORDINATES, AReading.Unit.LATITUDE, 4526.30103))

        # readings.append(AReading(AReading.Type.COORDINATES, AReading.Unit.LONGITUDE, 7406.90012))
        # readings.append(AReading(AReading.Type.COORDINATES, AReading.Unit.ALTITUDE, 205338.0))
        # readings.append(AReading(AReading.Type.PITCH, AReading.Unit.PITCH_ANGLE, -2.4267098))
        # readings.append(AReading(AReading.Type.ROLL, AReading.Unit.ROLL_ANGLE, 0.412937))
        
        return readings
    
    def initialize_actuators(self) -> None:
        pass
        #we do not have any actuators for this subsystema side from the buzzer which carlos is doing 

    def control_actuators(self, commands: list[ACommand]):
        pass
        #again, we do not have any actuators
        
device = GeoLocationSubsystem()