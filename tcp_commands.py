from twisted.internet import reactor, protocol, endpoints
from twisted.protocols import basic
from twisted.protocols.policies import TimeoutMixin
import threading
import asyncio
import time
from pathlib import Path
import os

class Stream(basic.LineReceiver, TimeoutMixin):
    def __init__(self,bot):
        self.bot = bot

    def connectionMade(self):
        print("Connection made")
        self.setTimeout(120)

    def connectionLost(self, reason):
        print("Connection lost")


    def dataReceived(self, data):
        self.resetTimeout()
        if self.bot.tcp_speak:
            try:
                print("Bytes: " + str(len(data)))
                self.bot.music_data+=data
            except Exception as e:
                print(e)


class StreamFactory(protocol.ServerFactory):
    #protocol = File

    def __init__(self, bot):
        self.bot = bot

    def buildProtocol(self, addr):
        return Stream(self.bot)

class File(basic.LineReceiver,TimeoutMixin):
    def __init__(self,bot, files):
        self.bot = bot
        self.files = files
        self.state = 'READY'

    def connectionMade(self):
        print("Connection made")
        self.setTimeout(120)

    def connectionLost(self, reason):
        self.state = "READY"
        print("Connection lost")

    def handle_READY(self, msg):
        message = msg
        try:
            message = message.decode("UTF-8")
        except Exception as e:
            print(e)
        file = Path('cache/files/' + message)
        if not file.is_file():
            self.files.downloading_name = 'cache/files/' + message
            self.files.file = open(self.files.downloading_name, 'ab')
            self.files.is_downloading = True
            print(message)
            self.transport.write('success'.encode('UTF-8'))
            print('Start Downloading...')
            self.state = "DOWNLOADING"
        else:
            print('exist')
            self.files.downloading_name = 'cache/files/' + message
            self.files.playlist.append(self.files.downloading_name)
            self.transport.write('exist'.encode('UTF-8'))

    def handle_DOWNLOADING(self, data):
        message = ""
        if len(data) == 3:
            try:
                message = data.decode("UTF-8")
                if message == 'end':
                    self.files.is_downloading = False
                    self.files.playlist.append(self.files.downloading_name)
                    self.files.downloading_name = ''
                    self.files.file.close()
                    self.files.counter += 1
                    self.state = "READY"
                    self.transport.write('success'.encode('UTF-8'))
                else:
                    self.files.is_downloading = False
                    self.files.file.close()
                    os.remove(self.files.downloading_name)
                    self.state = "READY"
                    self.transport.write('error'.encode('UTF-8'))
            except Exception as e:
                print(e)
        else:
            self.files.file.write(data)

    def dataReceived(self, data):
        self.resetTimeout()
        print("Received data")
        if self.state == "READY":
            self.handle_READY(data)
        elif self.state == "DOWNLOADING":
            self.handle_DOWNLOADING(data)


class FileFactory(protocol.ServerFactory):
    #protocol = File

    def __init__(self, bot, files):
        self.bot = bot
        self.files = files

    def buildProtocol(self, addr):
        return File(self.bot, self.files)


class Command(basic.LineReceiver, TimeoutMixin):
    def connectionMade(self):
        print("Connection made")
        self.setTimeout(120)

    def connectionLost(self, reason):
        print("Connection lost")


    def dataReceived(self, data):
        self.resetTimeout()
        print("Received data")
        message = ''
        addr = self.transport.getPeer().host
        try:
            message = data.decode("UTF-8")
        except Exception as e:
            self.transport.write('error'.encode('UTF-8'))
            print(e)
        if addr in self.factory.clients:
            if message.startswith('@@'):
                msg = message.split()
                if len(msg) >= 2:
                    self.factory.bot.events.append(msg)
                    self.transport.write('success'.encode('UTF-8'))
                else:
                    self.transport.write('error'.encode('UTF-8'))
            else:
                if message == "disconnect":
                    self.factory.clients.append(addr)
                    self.transport.write('success'.encode('UTF-8'))
                elif message == self.factory.password:
                    self.transport.write('success'.encode('UTF-8'))
                else:
                    self.transport.write('error'.encode('UTF-8'))
        else:
            if message == self.factory.password:
                self.factory.clients.append(addr)
                self.transport.write('success'.encode('UTF-8'))
            elif message == "disconnect":
                self.transport.write('success'.encode('UTF-8'))
            else:
                self.transport.write('error'.encode('UTF-8'))

class CommandFactory(protocol.ServerFactory):
    protocol = Command

    def __init__(self, bot, password):
        self.bot = bot
        self.password = password
        self.clients =[]



class RunServer(object):
    def __init__(self, bot, password, files, ip, port):
        self.bot = bot
        self.password = password
        self.files = files
        self.ip = ip
        self.port = port
        self.thread = threading.Thread(target=self.run, args=())
        self.thread.daemon = True  # Daemonize thread
        self.thread.start()  # Start the execution



    def run(self):
        fingerEndpoint = endpoints.serverFromString(reactor, "tcp:{0}:interface={1}".format(str(self.port), self.ip))
        fingerEndpoint.listen((CommandFactory(self.bot, self.password)))
        fingerEndpoint_2 = endpoints.serverFromString(reactor, "tcp:{0}:interface={1}".format(str(self.port+1), self.ip))
        fingerEndpoint_2.listen((StreamFactory(self.bot)))
        fingerEndpoint_2 = endpoints.serverFromString(reactor, "tcp:{0}:interface={1}".format(str(self.port+2), self.ip))
        fingerEndpoint_2.listen((FileFactory(self.bot, self.files)))
        reactor.run(installSignalHandlers=False)

