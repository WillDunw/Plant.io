from hardware.geoLocation.geoLocationDeviceController import GeoLocationSubsystem
from hardware.plant.PlantSubsystem import PlantSubsystem
from hardware.security.security_subsystem import SecuritySubsystem
from connection_manager import ConnectionManager
from hardware.geoLocation.actuators import ACommand
from hardware.geoLocation.sensors import AReading

import asyncio

class Farm:
    
    loop_interval = 5
    
    def __init__(self):
        self.initSubsystems()
        self.connection_manager = ConnectionManager()        
        
    def initSubsystems(self) -> None:
            self. geo_location = GeoLocationSubsystem()
            self.security = SecuritySubsystem()
            self.plant = PlantSubsystem()
        
    async def loop(self) -> None:
        await self.connection_manager.connect()
        self.loop_interval = self.connection_manager.get_reported_telemetry()
        self.connection_manager.register_command_callback(
            self.control_subsystem_actuator
        )
        self.connection_manager.register_telemetry_callback(
            self.set_telemetry
        )
        while True:
            readings = self.read_all_sensors()

            await self.connection_manager.send_readings(readings)
            await asyncio.sleep(self.loop_interval)

    def control_subsystem_actuator(self, command: ACommand):
            self.plant.control_actuator(command)
            self.security.control_actuator(command)

    def set_telemetry(self,new_telemetry: int):
        if (self.loop_interval != new_telemetry):
                print("Setting to new telemetry interval: "  + str(new_telemetry))
                self.loop_interval = new_telemetry
        else:
            return
    def read_all_sensors(self) -> list[AReading]:
        readings = []
        readings.extend(self.plant.read_sensors())
        readings.extend(self.geo_location.read_sensors())
        readings.extend(self.security.read_sensors())
        
        return readings
    
async def farm_main():
    farm = Farm()
    

    await farm.loop()

if __name__ == "__main__":
    asyncio.run(farm_main())