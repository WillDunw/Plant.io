from abc import ABC, abstractmethod
from .sensors import ISensor, AReading
from grove.grove_mini_pir_motion_sensor import GroveMiniPIRMotionSensor
import time

class MotionSensor(ISensor):
    def __init__(self, gpio: int,  model: str, type: AReading.Type):
        self.model = model
        self.type = type
        self.sensor = GroveMiniPIRMotionSensor(gpio)
        self.motion_detected = 0
        self.sensor.on_detect = self._on_motion_detected #for just a reference
    def _on_motion_detected(self):
        self.motion_detected = 1
    def read_sensor(self) -> list[AReading]:
        """Takes a reading form the sensor

        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """
        self.sensor._handle_event(None, None)  # Simulate reading motion
        reading = AReading(self.type, AReading.Unit.DETECTED, self.motion_detected)
        self.motion_detected = 0
        return [reading]

if __name__ == '__main__':
    gpio = int(input("Enter the gpio slot where the device is connected: "))
    motion_sensor = MotionSensor(gpio, "Motion sensor", AReading.Type.MOTION)
    while True:
        time.sleep(1)
        print(motion_sensor.read_sensor())