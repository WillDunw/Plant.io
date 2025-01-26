#!/usr/bin/env python

from gpiozero import OutputDevice

from grove.grove_ws2813_rgb_led_strip import GroveWS2813RgbStrip
from rpi_ws281x import Color
from .actuators import ACommand, IActuator

from time import sleep


class LED(IActuator):
    _current_state: str
    type: ACommand.Type

    def __init__(self, gpio: int, type: ACommand.Type = ACommand.Type.LIGHT_ON_OFF,
                 initial_state: str = "off") -> None:
        self.type = type
        self._current_state = initial_state

        count = 10
        self.led = GroveWS2813RgbStrip(gpio, count)

    def validate_command(self, command: ACommand) -> bool:
        return self.type == command.target_type

    def control_actuator(self, data:dict) -> bool:
        if data['value'] == True:
            data['value'] = "on"
        elif data['value'] == False:
            data['value'] = "off"

        if (data['value'] != "on" and data['value'] != "off"):
            raise ValueError("Fan value must either be 'on' or 'off' or 'True' or 'False'")
        
        if(data['value'] == self._current_state):
            return False
        else:
            if(data['value'] == 'on'):
                print("TURNING LED ON")
                for i in range(self.led.numPixels()):
                    self.led.setPixelColor(i, Color(255,235,235))
                    self.led.show()


            else:
                print("TURNING LED OFF")
                for i in range(self.led.numPixels()):
                    self.led.setPixelColor(i, Color(0,0,0))
                    self.led.show()
            self._current_state = data['value']
            return True

if __name__ == "__main__":
    led = LED(22)

    while True:
        led.control_actuator({"value": "on"})
        sleep(2)
        led.control_actuator({"value": "off"})
        sleep(2)