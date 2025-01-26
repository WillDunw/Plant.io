[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/ZKGepmmw)
# <div align='center'> 420-6A6-AB Application Development III
# <div align='center'> 420-6P3-AB Connected Objects 
# <div align='center'> Winter 2024

### <div align='center'> Final Project

This is a pair of programs intended to be used for a remote grow-container.

Our mobile app targets C# .NET 8.0 and uses MAUI for UI, it also uses the MVVM architectural framework. It has been tested with and developed for the Android platform. For optimal compatibility, it is recommended to run on a Pixel 5 (or higher) using API version 34 (or higher). The application provides different features for two respective user-types: fleet-owners and technicians. Fleet owners are provided access to diagnostic data regarding a grow-container such as vibration detection, alarm control and location data. Technicians are provided analytics for moisture, temperature, humidity and water level for the crops inside a specific container as well as control of lighting and ventilation. 

The mobile application sends C2D commands to an Azure IOT Hub that forwards the commands to a Raspberry Pi at the container. 

The Raspberry Pi runs the second portion of the project; a Python backend that reads values from sensors and can control actuators given C2D commmands retreived from the Azure IOT Hub. Readings are collected on a 10 second interval by default and are sent together as one message. All collected readings are:

- Temperature
- Humidity
- Moisture 
- Water Level
- Vibration Level
- Location
- Motion Level (Motion sensor)
- Sound Level
- Door lock state

All controllable actuators are:

- Fan (to control ventilation and temperature)
- Light
- Buzzer (emits a loud alarm sound)
- Door (servo controlling door state)

Charts and diagnostic information update live in our mobile app as soon as readings are received.

## <div align='center'> Controlling Actuators

### <ins>Component Connection Guide

| Component | GPIO Port | Component Type |
|-----------|:---------:|:--------------:|
| GPS sensor| UART base | Sensor         |
| Accelerator| Built-In | Sensor         |
| Fan | 16 | Actuator
| Light | 18 | Actuator
| AHT20 Temp & Humidity Sensor | 1 | Sensor
| Water Level Sensor | 4 | Sensor
| Soil Moisture Sensor | 2 | Sensor
| PIR Motion Sensor | 22 | Sensor
| Magnetic door sensor reed switch | 5 | Sensor
| Sound Sensor/ Noise Detector | 6 | Sensor
| MG90S 180° Micro Servo | 24 | Actuator

### <ins>Servo</ins>

Strategy: C2D

Reason: We chose C2D as it functions without requiring a response immediately. This is beneficial when device connectivity is spotty as the command is queued and will execute when the connection is recovered as opposed to being lost.

Turn Servo on: {"value": "open"}

Turn Servo off: {"value": "close"}

*Note: send command with custom property of "command-type" and a value of "servo"*

### <ins>Fan</ins>

Strategy: C2D

Reason: We chose C2D for similar reasons as with the Servo. Poor device connection quality wont cause the message to be lost. C2D was also simpler to implement and use.

Turn Fan on: {"value": "on"}

Turn Fan off: {"value": "off"}

*Note: send command with custom property of "command-type" and a value of "fan"*

### <ins>Light</ins>

Strategy: C2D

Reason: We again chose C2D for similar reasons as the Servo the Light. The simple nature of C2D and its versatility made it a prime candidate for the light actuator.

Turn Light on: {"value": "on"}

Turn Light off: {"value": "off"}

*Note: send command with custom property of "command-type" and a value of "light"*

### <ins>Buzzer</ins>

Strategy: C2D

Reason: We used C2D for the same reasons as the other actuators. It is reliable in poor network conditions, such as when a client or fleet owner does not have perfect access to the internet.

Turn Buzzer on: {"value": "on"}

Turn Buzzer off: {"value": "off"}

*Note: send command with custom property of "command-type" and a value of "buzzer"*

## <div align='center'> Contributions

Griffin: Plant subsystem / Viewmodel, UI design, Reading parsing |
Carlos: Security subsystem and Viewmodel, small UI changes| 
William: GeoLocation Subsystem, telemetry -> sending commands and receiving readings through azure
