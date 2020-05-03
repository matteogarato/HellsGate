import configparser
import requests
from requests_jwt import JWTAuth
import RPi.GPIO as GPIO


def main():
    print('Read Configuration')
    configParser.read(configFilePath)
    url = configParser.get('API', 'Url')
    secreToken = configParser.get('API', 'Token')
    RELAIS_1_GPIO = int(configParser.get('API', 'Token'))
    GPIO.setmode(GPIO.BCM) # GPIO Numbers instead of board numbers
    GPIO.setup(RELAIS_1_GPIO, GPIO.OUT) # GPIO Assign mode
    with open('/dev/tty0', 'r') as tty:
        while True:
            RFID_input = tty.readline().rstrip()
            auth = JWTAuth(secreToken)
            if (requests.get(url + RFID_input, auth=auth)):
                 GPIO.output(RELAIS_1_GPIO, GPIO.LOW) # out
                 GPIO.output(RELAIS_1_GPIO, GPIO.HIGH) # on