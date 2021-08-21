#include <Arduino.h>
#include <SoftwareSerial.h>

SoftwareSerial serial1(2, 3);

void setup() {
  	Serial.begin(9600);
	  Serial.println("Starting Uno");
    Serial.write('.');
    Serial.write('.');
    Serial.write('.');
    serial1.begin(9600);
}

void loop() {
  if (serial1.available())
    Serial.write(serial1.read());
  if (Serial.available())
    serial1.write(Serial.read());
}