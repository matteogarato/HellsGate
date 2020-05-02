
import requests
from requests_jwt import JWTAuth

def main():
    with open('/dev/tty0', 'r') as tty:
        while True:
            RFID_input = tty.readline().rstrip()
            auth = JWTAuth('secretT0Ken')
            requests.get("http://jwt-protected.com", auth=auth)