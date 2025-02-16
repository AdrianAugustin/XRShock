#include <Arduino.h>
#define VERSION "3.0"
const int inputPin = 2;
const int outputPin = 11;
const unsigned long recordTime =1000000;

const int maxSamples = 150; // Limited storage
unsigned long durations[maxSamples];
bool signalStates[maxSamples];
int sampleCount = 0;



void setup() {
    pinMode(inputPin, INPUT);
    pinMode(outputPin, OUTPUT);
    Serial.begin(115200);
}

void playBinaryString(const char* binaryString) {
    Serial.println("Playing binary string...");
    digitalWrite(outputPin,LOW);
    delayMicroseconds(3100);
    digitalWrite(outputPin,HIGH);
    delayMicroseconds(1400);
    digitalWrite(outputPin,LOW);
    delayMicroseconds(750);

    while (*binaryString) {
        if (*binaryString == '1') {
            digitalWrite(outputPin, HIGH);
            delayMicroseconds(750);
            digitalWrite(outputPin, LOW);
            delayMicroseconds(250);
        } else if (*binaryString == '0') {
            digitalWrite(outputPin, HIGH);
            delayMicroseconds(250);
            digitalWrite(outputPin, LOW);
            delayMicroseconds(750);
        }
        binaryString++;
    }
    digitalWrite(outputPin, LOW); // Ensure output is LOW at the end
}


void printRecording() {
    Serial.println("Recorded Signal:");
    for (int i = 0; i < sampleCount; i++) {
        Serial.print("State: ");
        Serial.print(signalStates[i]);
        Serial.print(" | Duration: ");
        Serial.println(durations[i]);
    }
}

void recordAndReplayStuff(){

    Serial.println("Recording...");
    sampleCount = 0;
    unsigned long lastChangeTime = micros();
    bool lastState = digitalRead(inputPin);
    
    while (micros() - lastChangeTime < recordTime && sampleCount < maxSamples) {
        bool currentState = digitalRead(inputPin);
        if (currentState != lastState) {
            durations[sampleCount] = micros() - lastChangeTime;
            signalStates[sampleCount] = lastState;
            lastChangeTime = micros();
            lastState = currentState;
            sampleCount++;
        }
    }
    
    Serial.println("Done...");
    delay(3000);
    Serial.println("Playback...");
    for (int i = 0; i < sampleCount; i++) {
        digitalWrite(outputPin, signalStates[i]);
        delayMicroseconds(durations[i]);
    }
    printRecording();
    digitalWrite(outputPin, LOW); // Ensure the signal ends in LOW state
    Serial.println("Done. Restarting...");
    delay(2000); // Wait before restarting
}



void loop() {
         
recordAndReplayStuff();


}




