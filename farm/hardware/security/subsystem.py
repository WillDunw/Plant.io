# initialize?
# read sensors
# control actuator
# toggle actuator (optional)
from abc import ABC, abstractmethod
from .sensors import AReading, ISensor
from .actuators import ACommand, IActuator

class ISubsystem(ABC):

    @abstractmethod
    def __init__(self) -> None:
        pass

    @abstractmethod
    def initialize_sensors(self) -> list[ISensor]:
        pass

    @abstractmethod
    def initialize_actuators(self) -> list[IActuator]:
        pass

    @abstractmethod
    def read_sensors(self) -> list[AReading]:
        pass

    @abstractmethod
    def control_actuator(self, commands: ACommand) -> None:
        pass