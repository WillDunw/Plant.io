from abc import ABC, abstractmethod
from .sensors import ISensor, AReading
import RPi. GPIO as GPIO
import time

class MagneticDoor(ISensor):
    def __init__(self, gpio: int,  model: str, type: AReading.Type):
        GPIO.setmode(GPIO.BCM)
        self.model = model
        self.type = type
        self.gpio = gpio
        GPIO.setup(gpio, GPIO .IN, pull_up_down = GPIO.PUD_UP)

    def read_sensor(self) -> list[AReading]:
        """Takes a reading form the sensor

        :return list[AReading]: List of readinds measured by the sensor. Most sensors return a list with a single item.
        """
        input = GPIO.input(self.gpio)
        value = 0
        if input == 0:
            value = 1
        reading = AReading(self.type, AReading.Unit.DETECTED, value)
        return [reading]

if __name__ == '__main__':
    gpio = int(input("Enter the gpio slot where the device is connected: "))
    door = MagneticDoor(gpio, "door", AReading.Type)
    while True:
        print(door.read_sensor())
        time.sleep(1)