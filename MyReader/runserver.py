"""
This script runs the MyReader application using a development server.
"""

from os import environ
from MyReader import app

if __name__ == '__main__':
    HOST = environ.get('SERVER_HOST', '127.0.0.1')
    try:
        PORT = int(environ.get('SERVER_PORT', '5555'))
    except ValueError:
        PORT = 5555
    app.run(HOST, PORT)
