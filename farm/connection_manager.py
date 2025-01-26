import asyncio
from typing import Callable

from hardware.geoLocation.actuators import ACommand
from hardware.geoLocation.sensors import AReading

from azure.iot.device.aio import IoTHubDeviceClient
from azure.iot.device import Message

from dotenv import dotenv_values
import json

class ConnectionConfig:
    """Represents all information required to successfully connect client to cloud gateway.
    """

    # Key names for configuration values inside .env file. See .env.example
    # Constants included as static class property
    DEVICE_CONN_STR = "IOTHUB_DEVICE_CONNECTION_STRING"

    def __init__(self, device_str: str) -> None:
        self._device_connection_str = device_str


class ConnectionManager:
    """Component of HVAC system responsible for communicating with cloud gateway.
    Includes registering command and reading endpoints and sending and receiving data.
    """

    def __init__(self) -> None:
        """Constructor for ConnectionManager and initializes an internal cloud gateway client.
        """
        self._connected = False
        self._config: ConnectionConfig = self._load_connection_config()
        self._client = IoTHubDeviceClient.create_from_connection_string(
            self._config._device_connection_str)
        

    def _load_connection_config(self) -> ConnectionConfig:
        """Loads connection credentials from .env file in the project's top-level directory.

        :return ConnectionConfig: object with configuration information loaded from .env file.
        """
        conn_string = dotenv_values(".env")[ConnectionConfig.DEVICE_CONN_STR]
        return ConnectionConfig(conn_string)

    def _on_message_received(self, message: Message) -> None:
        """Callback for handling new messages received from cloud gateway. Once the message is
        received and processed, it dispatches an ACommand to DeviceManager using _command_callback()

        :param Message message: Incoming cloud gateway message. Messages with actuator commands
        must contain a custom property of "command-type" and a json encoded string as the body.
        """
        
        #TODO: implement your own actuators here
        # command_type = None
        
        
        # if message.custom_properties['command-type'] == 'light-pulse':
        #     command_type = ACommand.Type.LIGHT_PULSE
        # elif message.custom_properties['command-type'] == 'fan':
        #     command_type = ACommand.Type.FAN
        # elif message.custom_properties['command-type'] == 'light-on-off':
        #     command_type = ACommand.Type.LIGHT_ON_OFF
        # elif message.custom_properties['command-type'] == 'servo':
        #     command_type = ACommand.Type.SERVO
        #     subsystem_command = "security"

        try:
            command_type = message.custom_properties["command-type"]
            body = str(message.data, encoding='utf-8')
            print(body)
            command = ACommand(command_type, body)
            print(f"Received following message: {command}")
            self._command_callback(command)
        except Exception as e:
            print(e)
        
        # self._command_callback(ACommand(command_type, message.data), subsystem_command)

    async def connect(self) -> None:
        """Connects to cloud gateway using connection credentials and setups up a message handler
        """
        await self._client.connect()
        self._connected = True
        print("Connected")
        # Setup the callback handler for on_message_received of the IoTHubDeviceClient instance.
        self._client.on_message_received = self._on_message_received
        #setup twin and twin properties ig
        self._twin = await self._client.get_twin()
        print(self._twin)
        self._client.on_twin_desired_properties_patch_received = self.receive_twin_desired_property
        
    def register_command_callback(self, command_callback: Callable[[ACommand], None]) -> None:
        """Registers an external callback function to handle newly received commands.

        :param Callable[[ACommand], None] command_callback: function to be called whenever a new command is received.
        """
        self._command_callback = command_callback
        

    async def send_readings(self, readings: list[AReading]) -> None:
        """Send a list of sensor readings as messages to the cloud gateway.

        :param list[AReading] readings: List of readings to be sent.
        """
        msg_body = "["

        first_message = True
        for reading in readings:
            value_str = str(reading.value)
            unit_str = str(reading.reading_unit.value)
            reading_str = str(reading.reading_type.value)
            msg_line = ""
            if first_message:
                msg_line = '{"value": "' + value_str + '", "unit": "' + unit_str + '" , "reading-type": "' + reading_str + '"}' 
                first_message = False
            else:
                msg_line = ',{"value": "' + value_str + '", "unit": "' + unit_str + '" , "reading-type": "' + reading_str + '"}' 
            
            msg_body += msg_line

        msg_body += "]"
        msg = Message(msg_body)
        # await self._client.send_message(msg)
        # print(f"Sent message: {msg}")

    async def update_twin_reported_property(self, patch: dict):
        print("Updating reported properties to the following patch: " + str(patch))
        await self._client.patch_twin_reported_properties(patch)
        
    def get_reported_telemetry(self):
        telemetry = 5
        if "telemetryInterval" in self._twin["reported"]:
            telemetry = int(self._twin["reported"]["telemetryInterval"])
        return telemetry
        
    async def receive_twin_desired_property(self, patch: dict):
        print("Received the following desired patch: " + str(patch))
        #update the twin
        
        self._twin = await self._client.get_twin()
        #change the interval as soon as it is received
        telemetry = int(self._twin["desired"]["telemetryInterval"])
        self._telemetry_callback(telemetry)
         #create a patch without the $version property (causes error)
        reported_patch = {"telemetryInterval": telemetry}
        #update reported property to the newest patch
        await self.update_twin_reported_property(reported_patch)
        
    def register_telemetry_callback(self, set_telemetry_callback: Callable[[int], None]):
        self._telemetry_callback = set_telemetry_callback
    
