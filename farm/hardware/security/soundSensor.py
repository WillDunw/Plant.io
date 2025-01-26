import grove.i2c
from .sensors import ISensor, AReading
from grove.adc import ADC
import time

class SoundSensor(ADC, ISensor):
    def __init__(self, gpio: int = 0, model: str = "Sound Sensor", type: AReading.Type = AReading.Type.VOLTAGE, bus=1):
        self.gpio = gpio
        self.bus = grove.i2c.Bus(bus)
        self.model = model
        self.type = type
        self.address=0x04

    def read_sensor(self) -> list[AReading]:
        """Reads data from the sensor.

        :return: A list of sensor readings.
        """
        voltage = self.read_voltage(self.gpio)
        reading = AReading(self.type, AReading.Unit.VOLTAGE, voltage)
        return [reading]



if __name__ == '__main__':
    while True:
        adc = SoundSensor(6, "Sound sensor", AReading.Type.VOLTAGE) 
        print(adc.read_sensor())
        time.sleep(1)
