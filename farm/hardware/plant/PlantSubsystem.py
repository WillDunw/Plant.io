from time import sleep

from dotenv import dotenv_values

from .sensors import AReading
from .actuators import ACommand
from .subsystem import ISubsystem

_config = dotenv_values("../../.env")

class PlantSubsystem(ISubsystem):
    def __init__(self) -> None:
        # INITIALIZING SENSORS
        self.initialize_sensors()

        # INITIALIZING ACTUATORS
        self.initialize_actuators()
        self.mock_temp = 5
        self.mock_humi = 3
    
    def initialize_sensors(self) -> None:
        if "false" == "True":
            from .mockSensor import MockSensor
            self.sensors = [
                MockSensor(1, "Mock Temp/Humi Sensor", AReading(AReading.Type.TEMPERATURE, AReading.Unit.CELCIUS, 1)),
                MockSensor(2, "Mock Water Level Sensor", AReading(AReading.Type.HUMIDITY, AReading.Unit.HUMIDITY, 30)),
                MockSensor(3, "Mock Soil Moisture Sensor", AReading(AReading.Type.HUMIDITY, AReading.Unit.HUMIDITY, 40))
            ]
        else:
            from grove.grove_temperature_humidity_aht20 import GroveTemperatureHumidityAHT20
            from .temp_humi_sensor import TempHumiSensor
            from .waterLevelSensor import WaterLeveSensor
            from .moistureLevelSensor import MoistureLevelSensor

            self.sensors = [
                TempHumiSensor(1, "AH20", AReading(AReading.Type.TEMPERATURE, AReading.Unit.CELCIUS, 1)),
                WaterLeveSensor(0x04, "Grove Water Sensor", AReading(AReading.Type.HUMIDITY, AReading.Unit.HUMIDITY, 1)),
                MoistureLevelSensor(2, "Grove Moisture Sensor", AReading(AReading.Type.HUMIDITY, AReading.Unit.HUMIDITY, 1))
            ]

    def initialize_actuators(self) -> None:
        if "false" == "True":
            self.sensors = [
                TempHumiSensor(1, "AH20", AReading(AReading.Type.TEMPERATURE, AReading.Unit.CELCIUS, 1)),
                WaterLeveSensor(0x04, "Grove Water Sensor", AReading(AReading.Type.HUMIDITY, AReading.Unit.HUMIDITY, 1)),
                MoistureLevelSensor(2, "Grove Moisture Sensor", AReading(AReading.Type.HUMIDITY, AReading.Unit.HUMIDITY, 1))
            ]

            from .mockActuator import MockActuator
            self.actuators = [
                MockActuator(4, ACommand.Type.LIGHT_ON_OFF), 
                MockActuator(5, ACommand.Type.FAN)
                ]
        else:
            from gpiozero import Motor
            from gpiozero import OutputDevice
            from .fan import Fan
            from .led import LED

            self._fanActuator = Fan(16)
            self._lightActuator = LED(18)
            self.actuators = [
                self._fanActuator,
                self._lightActuator,
                ]

    def read_sensors(self) -> list[AReading]:
        readings = []
        
        for sensor in self.sensors:
            readings.extend(sensor.read_sensor())
        # readings.extend(self._tempHumiSensor.read_sensor())
        # readings.extend(self._waterLevelSensor.read_sensor())
        # readings.extend(self._soilMoistureSensor.read_sensor())

        # readings.append(AReading(AReading.Type.HUMIDITY, AReading.Unit.HUMIDITY, self.mock_humi))
        # readings.append(AReading(AReading.Type.TEMPERATURE, AReading.Unit.CELCIUS, self.mock_temp))
        # self.mock_humi += 1
        # self.mock_temp += 0.5
        return readings
    
    def control_actuator(self, command: ACommand):
        for actuator in self.actuators:
            if actuator.validate_command(command):
                actuator.control_actuator(command.data)

    def read_light_state(self):
        return self._lightActuator._current_state

    def read_fan_state(self):
        return self._fanActuator._current_state

if __name__ == "__main__":
    plantSubsystem = PlantSubsystem()

    while True:
        print("Reading Sensors")
        print(plantSubsystem.read_sensors())
        print("Turning Fan On")
        print(plantSubsystem.control_actuator(ACommand(ACommand.Type.FAN, '{"value": "on"}')))
        sleep(2)
        print("Turning Fan Off")
        print(plantSubsystem.control_actuator(ACommand(ACommand.Type.FAN, '{"value": "off"}')))
        sleep(1)
        print("Turning Light On")
        print(plantSubsystem.control_actuator(ACommand(ACommand.Type.LIGHT_ON_OFF, '{"value": "on"}')))
        sleep(2)
        print("Turning Light Off")
        print(plantSubsystem.controlActuator(ACommand(ACommand.Type.LIGHT_ON_OFF, '{"value": "off"}')))

        sleep(2)