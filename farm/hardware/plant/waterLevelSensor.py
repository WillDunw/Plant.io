#!/usr/bin/env python

import grove.i2c
from .sensors import AReading, ISensor

from grove.adc import ADC

from grove.grove_water_sensor import GroveWaterSensor
from time import sleep

class customADC(ADC):
    def __init__(self, address=0x04, bus=1):
        self.address=address
        self.bus=grove.i2c.Bus(bus)

class CustomWaterSensor(GroveWaterSensor):
    def __init__(self, channel):
        self.channel = channel
        self.adc = customADC()

class WaterLeveSensor(ISensor):
    _sensor_model: str
    reading_type: AReading.Type

    def __init__(self, gpio: int, model: str, type: AReading.Type):
        self._sensor_model = model
        self.reading_type = type

        #self._sensor = GroveWaterSensor(gpio)
        self._sensor = CustomWaterSensor(0)
    
    def read_sensor(self) -> list[AReading]:
        return [AReading(AReading.Type.WATER_LEVEL, AReading.Unit.HUMIDITY, self._sensor.value)]