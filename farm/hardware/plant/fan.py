#!/usr/bin/env python

from gpiozero import Motor
from gpiozero import OutputDevice

from .actuators import ACommand, IActuator

from time import sleep


class Fan(IActuator):
    _current_state: str
    type: ACommand.Type

    def __init__(self, gpio: int, type: ACommand.Type = ACommand.Type.FAN,
                 initial_state: str = "off") -> None:
        self.fan = OutputDevice(gpio, active_high=True, initial_value=False)
        self._current_state = initial_state
        self.type = type

    def validate_command(self, command: ACommand) -> bool:
        return self.type == command.target_type

    # Returns true if the state of the actuator has changed, false if it stayed the same
    # Code is ripped from Assignment-2 fan_control.py file that was provided because it was better than mine
    def control_actuator(self, data:dict) -> bool:
        if data['value'] == "True":
            data['value'] = "on"
        elif data['value'] == "False":
            data['value'] = "off"

        # validate that the value is either 'on' or 'off'
        if (data['value'] != 'on' and data['value'] != 'off'):
            raise ValueError(
                'Fan value must be either "on" or "off" or "True" or "False"')

        # if the fan is already value, do nothing
        if (data['value'] == self._current_state):
            return False
        else:
            if (data['value'] == 'on'):
                self.fan.on()
            else:
                self.fan.off()
            self._current_state = data['value']
            return True

if __name__ == "__main__":
    fan = Fan(16)

    while True:
        #print("turning light on")
        fan.control_actuator({"value": "on"})
        sleep(3)
        #print("turning light off")
        fan.control_actuator({"value": "off"})
        sleep(2)