# initialize?
# read sensors
# control actuator
# toggle actuator (optional)

from sensors import AReading
from actuators import ACommand

class ISubsystem(ABC):

    @abstractmethod
    def __init__(self) -> None:
        pass

    @abstractmethod
    def initialize_sensors(self) -> None:
        pass

    @abstractmethod
    def initialize_actuators(self) -> None:
        pass

    @abstractmethod
    def read_sensors(self) -> list[AReading]:
        pass
    
    @abstractmethod
    def control_actuators(self, commands: list[ACommand]) -> None:
        pass