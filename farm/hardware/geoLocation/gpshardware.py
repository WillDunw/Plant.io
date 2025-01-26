import serial
import time
from .sensors import ISensor, AReading

class GPSHardware(ISensor):
    def __init__(self, gpio: int, model: str, type: AReading.Type) -> None:
        self.type = type
        self.model = model
        self.serial = serial.Serial('/dev/ttyAMA0', 9600, timeout=1)
        self.serial.reset_input_buffer()
        self.serial.flush()
        
    def parse_gps_data(self,line: str) -> list[AReading]:
        readings = []
        if "$GNGGA" in line:
            values = line.split(",")
            for index, value in enumerate(values):
                if value.replace(".","").isnumeric():
                    if index == 1:
                        readings.append(AReading(AReading.Type.COORDINATES, AReading.Unit.ALTITUDE, float(value)))
                    elif index == 2:
                        readings.append(AReading(AReading.Type.COORDINATES, AReading.Unit.LATITUDE, float(value)))
                    elif index == 4:
                        readings.append(AReading(AReading.Type.COORDINATES, AReading.Unit.LONGITUDE, float(value)))
    
        return readings
    
    def read_sensor(self) -> list[AReading]:
        try:
            line = self.serial.readline().decode('utf-8')
            readings = []

            while len(line) > 0:
                line = self.serial.readline().decode('utf-8')
                readings.extend(self.parse_gps_data(line))
                
                if(len(readings) > 0):
                    return readings

        except UnicodeDecodeError:
            line = self.serial.readline().decode('utf-8')
# gps = GPSHardware(1, "Air530", AReading.Type.COORDINATES)

# while True:
#     readings = gps.read_sensor()
#     if readings is not None:
#         for reading in readings:
#             print(f'{reading.reading_unit}: {reading.value}')

#     time.sleep(1)