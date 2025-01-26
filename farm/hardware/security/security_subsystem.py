from .subsystem import ISubsystem
from .sensors import AReading
from .actuators import ACommand
from .magneticDoor import MagneticDoor
from .microServo import MicroServo
from .motionSensor import MotionSensor
from .soundSensor import SoundSensor
from .buzzer import BuzzerActuator
import time
class SecuritySubsystem(ISubsystem):
    def __init__(self):
        self.sensors: list[ISensor] = self.initialize_sensors()
        self.actuators: list[IActuator] = self.initialize_actuators()
        self.mock_detected = 1
        self.mock_sound = 5

    def initialize_sensors(self):

        motion_sensor = MotionSensor(22, "Motion sensor", AReading.Type.MOTION) #currently not working, suspecting hardware issue rather than software.
        door = MagneticDoor(5, "Magnetic door sensor reed switch", AReading.Type.DETECTION)
        sound_sensor = SoundSensor(6, "Grove - Loudness Sensor", AReading.Type.VOLTAGE)
        return [motion_sensor, door, sound_sensor]
    def initialize_actuators(self):
        servo = MicroServo(24, ACommand.Type.SERVO, "close")
        buzzer = BuzzerActuator(1, ACommand.Type.BUZZER, "off")
        return [servo, buzzer]
    
    def read_sensors(self) -> list[AReading]:
        readings: list[AReading] = []
        for sensor in self.sensors:
            readings.extend(sensor.read_sensor())

        # readings.append(AReading(AReading.Type.DETECTION, AReading.Unit.DETECTED, self.mock_detected))
        # self.mock_detected = 0 if self.mock_detected == 1 else 1
        # readings.append(AReading(AReading.Type.MOTION, AReading.Unit.DETECTED, self.mock_detected))
        # readings.append(AReading(AReading.Type.VOLTAGE, AReading.Unit.VOLTAGE, self.mock_sound))

        self.mock_sound += 1

        return readings

    def control_actuator(self, command: ACommand):
            for actuator in self.actuators:
                if actuator.validate_command(command):
                    actuator.control_actuator(command.data)

if __name__ == "__main__":
    device = SecuritySubsystem()
    fake_buzzer_message_body_on = '{"value": "on"}'
    fake_buzzer_message_body_off = '{"value": "off"}'
    fake_servo_message_body_open = '{"value": "open"}'
    fake_servo_message_body_close = '{"value": "close"}'
    servo_open = ACommand(ACommand.Type.SERVO, fake_servo_message_body_open)
    servo_close = ACommand(ACommand.Type.SERVO, fake_servo_message_body_close)
    buzzer_on = ACommand(ACommand.Type.BUZZER, fake_buzzer_message_body_on)
    buzzer_off = ACommand(ACommand.Type.BUZZER, fake_buzzer_message_body_off)
    while True:
        readings = device.read_sensors()
        print(readings)
         #control servo
        device.control_actuator(servo_open, buzzer_on)
        time.sleep(1)
        device.control_actuator(servo_close, buzzer_off)
        time.sleep(1)
        