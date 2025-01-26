#!/usr/bin/env python

from .actuators import ACommand, IActuator

class MockActuator(IActuator):
    _current_state: str
    type: ACommand.Type

    def __init__(self, gpio: int, type: ACommand.Type, initial_state: str = "off") -> None:
        self._current_state = initial_state
        self.type = type
        print("Mock Actuator Initialized")
    
    def validate_command(self, command: ACommand) -> bool:
        value = command.data['value']
        if isinstance(value, str):
            return (command.target_type == self.type) and (
                value in self._valid_values['on'] or
                value in self._valid_values['off'])
        else:
            return False
    
    def control_actuator(self, data: dict) -> bool:
        if data['value'].lower() in self._valid_values['on'] and self._current_state == 'off':
            print("Turning On")
            return True
        elif data['value'].lower() in self._valid_values['off'] and self._current_state == 'on':
            print("Turning Off")
            return False
        return False
    