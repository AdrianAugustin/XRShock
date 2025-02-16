#include <Arduino.h>
#include <EEPROM.h>
#define MAX_LEN_VER 32
#define EEPROM_ADDR_DEVID  0
#define EEPROM_ADDR_SIGNAL_REPEATS  4
#define EEPROM_ADDR_VERSION  8

unsigned int DevID;
unsigned int Repeats;
char DevVer[MAX_LEN_VER+1];

const int outputPin = 11;
const byte numChars = 32;
char receivedChars[numChars];
char mode[12] ={0};
char argument[16]={0};
int powerlevel = -1;
int channel=-1;



void setupDeviceInformation(){
  EEPROM.get(EEPROM_ADDR_DEVID, DevID);
  if (DevID == 0xFFFF || DevID == 0) {
    DevID = 15465;
    EEPROM.put(EEPROM_ADDR_DEVID, DevID);
  }
  EEPROM.get(EEPROM_ADDR_SIGNAL_REPEATS, Repeats);
  if (Repeats == 0xFFFF || Repeats == 0) {
    Repeats = 5;
    EEPROM.put(EEPROM_ADDR_SIGNAL_REPEATS, Repeats);
  }


  for (int i = 0; i < MAX_LEN_VER; i++) {
    DevVer[i] = EEPROM.read(EEPROM_ADDR_VERSION + i);
  }
  DevVer[MAX_LEN_VER] = '\0';

  if (DevVer[0] == 0xFF || DevVer[0] == '\0') { 
    strcpy(DevVer, "3.0");
    for (int i = 0; i < MAX_LEN_VER; i++) {
        EEPROM.write(EEPROM_ADDR_VERSION + i, DevVer[i]);
    }
  }
}
void setup() {
    pinMode(outputPin, OUTPUT);
    setupDeviceInformation();
    Serial.begin(115200);
}

void playBinaryString(const char* binaryString) {
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
    digitalWrite(outputPin, LOW);
}


void ExecuteCommand(char* payload,unsigned int repeats ){
    for (size_t i = 0; i < repeats; i++)
    {
        playBinaryString(payload);
        delayMicroseconds(2000);
    }
}

    char* GeneratePayload(uint16_t transmitterID, uint8_t channel, uint8_t mode, uint8_t strength) {
    static char binaryPayload[44];
    uint32_t payload = 0;

    payload |= ((uint32_t)transmitterID & 0xFFFF) << 16; // Transmitter ID (16 bits)
    payload |= ((uint32_t)channel & 0x03) << 12;         // Channel (4 bits)
    payload |= ((uint32_t)mode & 0x0F) << 8;            // Mode (4 bits)
    payload |= ((uint32_t)strength & 0xFF);             // Strength (8 bits)

    // Compute checksum (8-bit sum of all bytes)
    uint8_t checksum = ((payload >> 24) & 0xFF) + ((payload >> 16) & 0xFF) + ((payload >> 8) & 0xFF) + (payload & 0xFF);

    for (int i = 0; i < 32; i++) {
        binaryPayload[i] = (payload & (1UL << (31 - i))) ? '1' : '0';
    }

    // Append checksum (8 bits)
    for (int i = 0; i < 8; i++) {
        binaryPayload[32 + i] = (checksum & (1 << (7 - i))) ? '1' : '0';
    }

    binaryPayload[40] = '0'; 
    binaryPayload[41] = '0'; 
    binaryPayload[42] = '\n';

    return binaryPayload;
}

void executeCommand(int cmdType) {
  sscanf(argument, "%d,%d", &powerlevel, &channel);
  ExecuteCommand(GeneratePayload(DevID, 0, cmdType, powerlevel), Repeats);
}

void handleVibrate() { executeCommand(2);Serial.println("Vibrating"); }
void handleShock() { executeCommand(1); Serial.println("Shocking");}
void handleBeep() { executeCommand(3); Serial.println("Beeping");}
void handleVersion() { Serial.print("DeviceVersion="); Serial.println(DevVer); }
void handleReps() { Serial.print("Repeats="); Serial.println(Repeats); }
void handleDevID() { Serial.print("DeviceID="); Serial.println(DevID); }
void handleSetVersion() {
  strcpy(DevVer, argument);
  for (int i = 0; i < MAX_LEN_VER; i++) {
      EEPROM.update(EEPROM_ADDR_VERSION + i, DevVer[i]);
  }
  Serial.print("NewDeviceVersion="); Serial.println(DevVer);
}
void handleSetReps() {
  Repeats = atoi(argument);
  EEPROM.update(EEPROM_ADDR_SIGNAL_REPEATS + sizeof(int), Repeats);
  Serial.print("NewRepeats="); Serial.println(Repeats);
}
void handleSetDevID() {
  DevID = atoi(argument);
  EEPROM.update(EEPROM_ADDR_DEVID, DevID);
  Serial.print("NewDeviceID="); Serial.println(DevID);
}

struct Command {
  const char* name;
  void (*handler)();
};

Command commands[] = {
  {"Vibrate", handleVibrate},
  {"Shock", handleShock},
  {"Tone", handleBeep},
  {"Version", handleVersion},
  {"Reps", handleReps},
  {"DevID", handleDevID},
  {"SetVersion", handleSetVersion},
  {"SetReps", handleSetReps},
  {"SetDevID", handleSetDevID}
};

void TryParseForCommand() {
  sscanf(receivedChars, "%[^:]:%[^:]", mode, argument);
  
  for (unsigned int i = 0; i < sizeof(commands) / sizeof(commands[0]); i++) {
      if (strcmp(mode, commands[i].name) == 0) {
          commands[i].handler();
          return;
      }
  }
}

void recvWithEndMarker()
{
  static byte ndx = 0;
  char endMarker = '\n';
  char rc;

  while (Serial.available() > 0 )
  {
    rc = Serial.read();

    if (rc != endMarker)
    {
      receivedChars[ndx] = rc;
      ndx++;
      if (ndx >= numChars)
      {
        ndx = numChars - 1;
      }
    }
    else
    {
      receivedChars[ndx] = '\0'; // terminate the string
      if (ndx >= 3)
      {
        TryParseForCommand();
      }
      ndx = 0;
    }
  }
}

void loop() {
recvWithEndMarker();
}




