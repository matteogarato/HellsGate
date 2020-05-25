import configparser
#import requests
#import RPi.GPIO as GPIO
from uuid import getnode as get_mac
import time

configParser = configparser.RawConfigParser()
configFilePath = r'HellsGateNode.Config'

def main():
    try:
     print('Read Configuration')
     configParser.read(configFilePath)
     url = configParser.get('API', 'Url')
     secreToken = configParser.get('API', 'Token')
     RELAIS_1_GPIO = int(configParser.get('API', 'PinOut'))
     openTime = int(configParser.get('API', 'OpenTime'))
     nodeName = configParser.get('API', 'nodeName')
     #GPIO.setmode(GPIO.BCM) # GPIO Numbers instead of board numbers
     #GPIO.setup(RELAIS_1_GPIO, GPIO.OUT) # GPIO Assign mode
     auth = {'Authorization': 'token {}'.format(secreToken)}
     mac = get_mac()
     with open('/dev/tty0', 'r') as tty:
         while True:
             RFID_input = tty.readline().rstrip()
             if(RFID_input):
                 #todo: authentication call
                 #nodeData = '{"cardNumber": "{}",  "macAddress": "{}",  "nodeName": "{}"}'.format(RFID_input,mac,nodeName)
                 print(RFID_input)
                 #if (requests.get(url, auth=auth, data=json.dumps(nodeData))):
                 #     print('Open')
                 #     GPIO.output(RELAIS_1_GPIO, GPIO.HIGH) # on
                 #     time.sleep(openTime)
                 #     GPIO.output(RELAIS_1_GPIO, GPIO.LOW) # out
                 #else:
                 #    GPIO.output(RELAIS_1_GPIO, GPIO.LOW) # out
    except Exception as e:
                print(e)
                main()

if __name__ == '__main__':
    main()