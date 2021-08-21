#include <Arduino.h>
#include <SoftwareSerial.h>
#include <ESP8266WiFi.h>
#include <ESPAsyncTCP.h>
#include <ESPAsyncWebServer.h>
#include <OneParamRewrite.cpp>

#define WIFISSID "**********"
#define PASSWORD "**********"

SoftwareSerial serial1(D2, D3);
AsyncWebServer server(80);

const char *ssid = WIFISSID;
const char *password = PASSWORD;
const char *PARAM_MESSAGE = "message";
const int BLINK_PIN = D4;

String readResponse(int timeoutMs);

void notFound(AsyncWebServerRequest *request)
{
  request->send(404, "text/plain", "404");
}

void blink(int pin, int duration = 500, int number = 500)
{
  for (int i = 0; i < number; i++)
  {
    digitalWrite(pin, LOW);
    delay(duration);

    digitalWrite(pin, HIGH);
    delay(duration);
  }
}

void setup()
{
  pinMode(BLINK_PIN, OUTPUT);
  digitalWrite(BLINK_PIN, HIGH);

  Serial.begin(9600);
  serial1.begin(9600);

  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  if (WiFi.waitForConnectResult(60 * 1000) != WL_CONNECTED)
  {
    Serial.printf("WiFi Failed!\n");
    digitalWrite(BLINK_PIN, LOW);
    return;
  }

  Serial.print("IP Address: ");
  Serial.println(WiFi.localIP());
  blink(BLINK_PIN, 100, 5);

  server.on("/", HTTP_GET, [](AsyncWebServerRequest *request) { 
    request->send(200, "text/plain", "Optoma UHD42. IP address: " + WiFi.localIP().toString() + ". Free memory:" + ESP.getFreeHeap());
  });

  server.on("/cmd", HTTP_GET, [](AsyncWebServerRequest *request) {
    if (request->hasParam(PARAM_MESSAGE))
    {
      auto message = request->getParam(PARAM_MESSAGE)->value();
      
      //serial1.flush();
      serial1.print(message);
      // serial1.print(0x50);
      // serial1.print(0x4f);
      // serial1.print(0x57);
      // serial1.print(0x52);
      // serial1.print(0x3f);
      // serial1.print(0x3f);
      // serial1.print(0x3f);
      // serial1.print(0xf3f);
      serial1.print('\r');
      // serial1.print('\n');
      Serial.println(message);
      message = readResponse(500);
      request->send(200, "text/plain", message);
    }
    else
    {
      request->send(1000, "text/plain");
    }
  });

  server.addRewrite( new OneParamRewrite("/cmd/{cmd}", "/cmd?message={cmd}") );

  server.onNotFound(notFound);
  server.begin();
}

String readResponse(int timeoutMs)
{
  auto timeout = millis() + timeoutMs;
  String result = "";

  while (millis() < timeout && !serial1.available())
    ; // wait for data

  while (millis() < timeout)
      if (serial1.available())
        result += (char)serial1.read();

  return result;
}

void loop()
{
  if (serial1.available())
  {
    Serial.write(serial1.read());
  }

  if (Serial.available())
  {
    serial1.write(Serial.read());
  }
}