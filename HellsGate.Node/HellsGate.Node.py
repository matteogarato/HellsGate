import configparser
import json
import requests
import RPi.GPIO as GPIO
from uuid import getnode as get_mac
import time

configParser = configparser.RawConfigParser()
configFilePath = r'HellsGateNode.Config'

def main():
    try:
     print('Read Configuration')
     configParser.read(configFilePath)
     url = configParser.get('API', 'Url')
     secreTokenUrl = configParser.get('API', 'Token')
     accessUrl = configParser.get('API', 'Access')
     RELAIS_1_GPIO = int(configParser.get('API', 'PinOut'))
     openTime = int(configParser.get('API', 'OpenTime'))
     nodeName = configParser.get('API', 'NodeName')
     GPIO.setmode(GPIO.BCM) # GPIO Numbers instead of board numbers
     GPIO.setup(RELAIS_1_GPIO, GPIO.OUT) # GPIO Assign mode
     GPIO.output(RELAIS_1_GPIO, GPIO.LOW) # out
     mac = ':'.join(("%012X" % get_mac())[i:i + 2] for i in range(0, 12, 2))
     authenticateData = {'macAddress': mac,'nodeName': nodeName}
     headers = {'content-type': 'application/json'}
     data_json = json.dumps(authenticateData)
     response = requests.post(url + secreTokenUrl, data=data_json, headers=headers, verify=False)
     if(not response.ok):
         print("unauthorized" + response.content.decode("utf-8"))
         return
     decoded_response = json.loads(response.content.decode("utf-8"))
     auth = { 'Content-Type': 'text/plain','Authorization': 'Bearer ' + decoded_response['token']}
     print('Configured!')
     with open('/dev/tty0', 'r') as tty:
         while True:
             RFID_input = tty.readline().rstrip()
             if(RFID_input and len(RFID_input) == 10):
                 nodeData = {'cardNumber': RFID_input,'macAddress': mac,'nodeName': nodeName}
                 authResponse = requests.get(url + accessUrl, auth=auth, data=json.dumps(nodeData),headers=auth, verify=False)
                 if (response.ok):
                      print('Open')
                      GPIO.output(RELAIS_1_GPIO, GPIO.HIGH) # on
                      time.sleep(openTime)
                      GPIO.output(RELAIS_1_GPIO, GPIO.LOW) # out
                 else:
                     print("unauthorized")
                     GPIO.output(RELAIS_1_GPIO, GPIO.LOW) # out
    except Exception as e:
                print(e)
                main()

if __name__ == '__main__':
    main()