from gpiozero import Servo
from time import sleep
from .actuators import ACommand, IActuator
from gpiozero.pins.pigpio import PiGPIOFactory
class MicroServo(IActuator):
    def __init__(self, gpio: int = 24, type: ACommand.Type = ACommand.Type.SERVO, initial_state: str = "close") -> None:
        # factory = PiGPIOFactory()
        # self.servo = Servo(gpio, pin_factory=factory)
        self.servo = Servo(gpio)
        self.type = type
        self._current_state = initial_state

    
    def validate_command(self, command: ACommand) -> bool:
        """Validates that a command can be used with the specific actuator.

        :param ACommand command: the command to be validated.
        :return bool: True if command can be consumed by the actuator.
        """
        return command.target_type == self.type

    
    def control_actuator(self, data: dict) -> bool:
        """Sets the actuator to the value passed as argument.

        :param str value: Value used to set the new state of the actuator. Value is parsed inside concrete classes.
        :return bool: True if the state of the actuator changed, false otherwise.
        """

        if data['value'] in ["True", "on"]:
            data['value'] = "open"
        elif data['value'] in ["False" ,"off"]:
            data['value'] = "close"

        # validate that the value is either 'on' or 'off'
        if (data['value'] != 'open' and data['value'] != 'close'):
            raise ValueError(
                'Servo value must be either "on" or "off" or "True" or "False" or "open" or "close"')

        # if the servo is already value, do nothing
        if (data['value'] == self._current_state):
            return False
        else:
            if (data['value'] == 'close'):
                self.servo.min()
            else:
                self.servo.max()
            self._current_state = data['value']
            print("Setting value of servo to", data['value'])
            return True


if __name__ == '__main__':
    gpio = int(input("Enter the gpio slot where the device is connected: "))
    servo = MicroServo(gpio, ACommand.Type.SERVO, "close")
    while True:
        servo.control_actuator("open")
        print("servo opened")
        sleep(1)
        servo.control_actuator("close")
        print("servo closed")
        sleep(1)