
from gpiozero import Buzzer
from time import sleep
from .actuators import ACommand, IActuator
import json
import seeed_python_reterminal.core as rt


class BuzzerActuator(IActuator):
    def __init__(self, gpio: int, type: ACommand.Type, initial_state: str) -> None:
        self.type = type
        self._current_state = initial_state


    def validate_command(self, command: ACommand) -> bool:
        return command.target_type == self.type

    
    def control_actuator(self, data: dict) -> bool:
        """Sets the actuator to the value passed as argument.

        :param str value: Value used to set the new state of the actuator. Value is parsed inside concrete classes.
        :return bool: True if the state of the actuator changed, false otherwise.
        """
        if data['value'] in ["True", "on"]:
            data['value'] = "on"
        elif data['value'] in ["False" ,"off"]:
            data['value'] = "off"

        # validate that the value is either 'on' or 'off'
        if (data['value'] != 'on' and data['value'] != 'off'):
            raise ValueError(
                'Buzzer value must be either "on" or "off" or "True" or "False"')

        # if the servo is already value, do nothing
        if (data['value'] == self._current_state):
            return False
        else:
            if (data['value'] == 'on'):
                rt.buzzer = True
            else:
                rt.buzzer = False
            self._current_state = data['value']
            return True

if __name__ == '__main__':

    fake_buzzer_command_off: dict = json.loads('{"value": "off"}')
    fake_buzzer_command_on: dict = json.loads('{"value": "on"}')
    buzz = BuzzerActuator(1, ACommand.Type.BUZZER, "off")

    while True:
        buzz.control_actuator(fake_buzzer_command_off)
        print("buzzer off")
        sleep(1)
        buzz.control_actuator(fake_buzzer_command_on)
        print("buzzer on")
        sleep(1)