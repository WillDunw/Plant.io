#!/usr/bin/env python

from .sensors import AReading, ISensor
import random

class MockSensor(ISensor):
    def __init__(self, gpio: int, model: str, type: AReading.Type):
        self._sensor_model = model
        self.reading_type = type
    
    def read_sensor(self) -> list[AReading]: 
        return [AReading(self.reading_type, AReading.Unit.CELCIUS, random.randint(1,100))]