#!/usr/bin/env python

import grove.i2c
from .sensors import AReading, ISensor

from grove.adc import ADC

from grove.grove_moisture_sensor import GroveMoistureSensor 
from time import sleep

class customADC(ADC):
    def __init__(self, address=0x04, bus=1):
        self.address=address
        self.bus=grove.i2c.Bus(bus)

class CustomMoistureSensor(GroveMoistureSensor):
    def __init__(self, channel):
        self.channel = channel
        self.adc = customADC()

class MoistureLevelSensor(ISensor):
    _sensor_model: str
    reading_type: AReading.Type

    def __init__(self, gpio: int, model: str, type: AReading.Type):
        self._sensor_model = model
        self.reading_type = type

        # self._sensor = GroveMoistureSensor(gpio)
        self._sensor = CustomMoistureSensor(0)
    
    def read_sensor(self) -> list[AReading]:
        return [AReading(AReading.Type.MOISTURE, AReading.Unit.HUMIDITY, self._sensor.moisture)]