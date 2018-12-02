# TODO:
# 1. M3U Playlist (Radio bot)
# 3. Rewrite utils
# 4. Use contex comands
# 5. Various comand (Random, etc)
# 6. Steam api
import sys
import re
import discord
from utils import *
from file_player import *
import asyncio
from subprocess import Popen
import random
import numpy
import sounddevice as sd
import collections
from pathlib import Path
from twisted.internet import reactor, protocol, endpoints
from twisted.protocols import basic
from twisted.internet.protocol import Protocol
from twisted.internet.protocol import Factory
import tcp_commands

class Bot:
    client = None
    voice_client = None
    text_channel = None
    player = None
    player_yt = None
    p_dl = None
    speak = False
    file_speak = False
    tcp_speak = False
    yt_speak = False
    curr_song = None
    #music_data = collections.deque(maxlen=100)
    music_data = b''
    # music_data = None
    socket_stream = None
    pcm_stream = None
    connected = []
    is_downloading = False
    events =[]

    def __init__(self, dc):
        self.client = dc

class Files:
    playlist = []
    is_downloading = False
    downloading_name = ""
    file = None
    current_song = ""
    counter = 0


    def __init__(self):
        pass

    def clear_playlist(self):
        self.playlist.clear()
        self.counter = 0


bot = Bot(discord.Client())

files = Files()

if not discord.opus.is_loaded():
    discord.opus.load_opus('opus')

q_file = "queue.dat"
yt_url_pattern = re.compile("^http(s)?:/\/(?:www\.)?youtube.com\/watch\?(?=.*v=\w+)(?:\S+)?$")
server_id = ''
token = ''
default_text_channel_id = ''
default_voice_channel_id = ''
password = ''
admin_role = ''
ip_address = '127.0.0.1'
port = 8888
# remove_file(q_file)

sd.default.device = 1  # change to desired device
sd.default.channels = 2
sd.default.dtype = 'int16'
sd.default.latency = 'low'
sd.default.samplerate = 48000

with open('config.cfg', 'r') as fin:
    for line in fin:
        ln = line.split('=')
        if ln[0] == 'SERVER_ID':
            server_id = ln[1][:-1]
        elif ln[0] == 'TOKEN':
            token = ln[1][:-1]
        elif ln[0] == 'DEFAULT_TEXT_CHANNEL_ID':
            default_text_channel_id = ln[1][:-1]
        elif ln[0] == 'DEFAULT_VOICE_CHANNEL_ID':
            default_voice_channel_id = ln[1][:-1]
        elif ln[0] == 'PASSWORD':
            if ln[1][:-1] == '':
                password = 'no_password'
            else:
                password = ln[1][:-1]
        elif ln[0] == 'ADMIN_ROLE':
            admin_role = ln[1][:-1]
        elif ln[0] == 'DEFAULT_DEVICE':
            sd.default.device = int(ln[1][:-1])
        elif ln[0] == 'IP_ADDRESS':
            ip_address = ln[1][:-1]
        elif ln[0] == 'PORT':
            try:
                port = int(ln[1][:-1])
            except Exception as ex:
                print('Wrong port Value')


# BOT COMMANDS
async def join(bot_, server_, channel, text_channel):
    if bot_.speak or bot_.tcp_speak or bot.file_speak:
        try:
            bot_.speak = False
            bot_.tcp_speak = False
            bot_.file_speak = False
            if bot_.player.is_playing:
                bot_.player.stop()
            if bot_.client.is_voice_connected(bot_.client.get_server(server_)):
                bot_.voice_client.disconnect()
            if bot_.voice_client:
                del bot_.voice_client
            if bot_.p_dl:
                del bot_.p_dl
            if bot_.player:
                del bot_.player
        except Exception as e:
            print(e)
            return

    if not bot_.client.is_voice_connected(bot_.client.get_server(server_)):
        await bot_.client.join_voice_channel(bot_.client.get_channel(channel))
    else:
        await bot_.client.move_member(list(filter(lambda x: x.id == bot_.client.user.id,
                                                  bot_.client.get_all_members()))[0],
                                      bot_.client.get_channel(channel))
    if not bot_.yt_speak:
        bot_.voice_client = bot_.client.voice_client_in(bot_.client.get_server(server_))
        bot_.player = Player(bot_.client, voice_client=bot_.voice_client)
        bot_.p_dl = Popen(["python", "yt_downloader.py"])
    else:
        voice_client = bot_.client.voice_client_in(bot_.client.get_server(server_))
        bot_.player.change_voice(voice_client)
    bot_.yt_speak = True
    await bot_.client.send_message(text_channel,
                                   'Bot ready for youtube songs:\nAdd songs with <@@ [link]>')



async def file_join(bot_, server_, channel, text_channel):
    if bot_.speak or bot_.tcp_speak or bot_.yt_speak:
        try:
            bot_.speak = False
            bot_.tcp_speak = False
            bot_.yt_speak = False
            if bot_.player.is_playing:
                bot_.player.stop()
            if bot_.client.is_voice_connected(bot_.client.get_server(server_)):
                bot_.voice_client.disconnect()
            if bot_.voice_client:
                del bot_.voice_client
            if bot_.p_dl:
                del bot_.p_dl
            if bot_.player:
                del bot_.player
        except Exception as e:
            print(e)
            return
    if not bot_.client.is_voice_connected(bot_.client.get_server(server_)):
        await bot_.client.join_voice_channel(bot_.client.get_channel(channel))
    else:
        await bot_.client.move_member(list(filter(lambda x: x.id == bot_.client.user.id,
                                                  bot_.client.get_all_members()))[0],
                                      bot_.client.get_channel(channel))
    if not bot_.file_speak:
        bot_.voice_client = bot_.client.voice_client_in(bot_.client.get_server(server_))
        bot_.player = FilePlayer(bot_.client, voice_client=bot_.voice_client, playlist=files.playlist)
    else:
        voice_client = bot_.client.voice_client_in(bot_.client.get_server(server_))
        bot_.player.change_voice(voice_client)
    bot_.file_speak = True
    await bot_.client.send_message(text_channel,
                                   'Bot ready for playing files')


async def add_song(bot_, msg, text_channel):
    if bot_.yt_speak:
        if yt_url_pattern.match(msg[1]):
            song_url = msg[1]
            try:
                this_song = fetch_song(song_url)
            except Exception as e:
                return
            bot_.player.queue(this_song)
            bot_.player.play()
            await bot_.client.send_message(text_channel,
                                           'Added {0} to playlist'.format(this_song.title))


async def skip(bot_, text_channel):
    if bot_.yt_speak or bot_.file_speak:
        bot_.player.skip()
        await bot_.client.send_message(text_channel,
                                       'Song skipped')


async def change_volume(bot_, msg, channel):
    if bot_.player:
        if not bot_.speak and not bot_.tcp_speak:
            if bot_.player.get_playlist():
                try:
                    volume = float(msg[2]) / 100
                    bot_.player.change_volume(volume)
                    print(bot_.player.volume)
                    await bot_.client.send_message(channel,
                                                   'Changed volume to {0}%'.format(volume*100))
                except Exception as e:
                    print("Not a float")
        else:
            try:
                volume = float(msg[2]) / 100
                bot_.player.volume = volume
                print(bot_.player.volume)
                await bot_.client.send_message(channel,
                                               'Changed volume to {0}%'.format(volume*100))
            except Exception as e:
                print("Not a float")


async def speak(bot_, server_, channel, text_channel):
    if bot_.tcp_speak or bot_.yt_speak or bot_.file_speak:
        try:
            bot_.tcp_speak = False
            bot_.yt_speak = False
            bot_.file_speak = False
            if bot_.player.is_playing:
                bot_.player.stop()
            if bot_.client.is_voice_connected(bot_.client.get_server(server_)):
                bot_.voice_client.disconnect()
            if bot_.voice_client:
                del bot_.voice_client
            if bot_.p_dl:
                del bot_.p_dl
            if bot_.player:
                del bot_.player
        except Exception as e:
            print(e)
            return
    if not bot_.client.is_voice_connected(bot_.client.get_server(server_)):
        await bot_.client.join_voice_channel(bot_.client.get_channel(channel))
    else:
        await bot_.client.move_member(
            list(filter(lambda x: x.id == bot_.client.user.id, bot_.client.get_all_members()))[0],
            bot_.client.get_channel(channel))
    bot_.voice_client = bot_.client.voice_client_in(bot_.client.get_server(server_))
    bot_.player = bot_.voice_client.create_stream_player(PCMStream())
    bot_.player.start()
    bot_.speak = True
    await bot_.client.send_message(text_channel,
                                   'Bot ready for line-in')


async def speak_tcp(bot_, server_, channel, text_channel):
    if bot_.speak or bot_.yt_speak or bot_.file_speak:
        try:
            bot_.speak = False
            bot_.yt_speak = False
            bot_.file_speak = False
            if bot_.player.is_playing:
                bot_.player.stop()
            if bot_.client.is_voice_connected(bot_.client.get_server(server_)):
                bot_.voice_client.disconnect()
            if bot_.voice_client:
                del bot_.voice_client
            if bot_.p_dl:
                del bot_.p_dl
            if bot_.player:
                del bot_.player
        except Exception as e:
            print(e)
            return
    if not bot_.client.is_voice_connected(bot_.client.get_server(server_)):
        await bot_.client.join_voice_channel(bot_.client.get_channel(channel))
    else:
        await bot_.client.move_member(
            list(filter(lambda x: x.id == bot_.client.user.id, bot_.client.get_all_members()))[0],
            bot_.client.get_channel(channel))
    bot_.voice_client = bot_.client.voice_client_in(bot_.client.get_server(server_))
    bot_.player = bot_.voice_client.create_stream_player(Stream(bot_))
    bot_.player.start()
    bot.music_data = b''
    bot_.tcp_speak = True
    await bot_.client.send_message(text_channel,
                                   'Bot ready for tcp stream')


async def rgg(bot_, msg, channel):
    try:
        with open('rgg\\' + msg[2] + '.txt', 'r') as f:
            await bot_.client.send_message(channel,
                                           'Play {0} on {1}'.format(random.choice(f.readlines()).rstrip(),
                                                                    msg[2]))
    except Exception as e:
        print(e)


async def purge(bot_, msg, roles, channel):
    if len(list(filter(lambda x: x.name == admin_role, roles))) > 0:
        try:
            int(msg[2])
            await bot_.client.purge_from(channel, limit=int(msg[2]))
        except Exception as e:
            print('Wrong value')


async def print_help(bot_, channel):
    try:
        help = '''
--------------------------------------------------------
|@@ join                    | Add bot to voice channel |
|@@ fjoin                   | Bot ready for files      |
|@@ leave                   | Bot leaves voice channel |
|@@ [link to youtube video] | Play music from youtube  |
|@@ playlist                | Bot write youtube or file playlist in chat |
|@@ clear                   | Clear playlist           |
|@@ skip                    | Skip youtube track       | 
|@@ volume [from 0 to 1]    | Change bot volume        |
|@@ purge [value]           | Delete [value] numbers of lines in chat |
|@@ speak                   | Bot speak from line-in   |
|@@ speak_tcp               | Bot speak from tcp       | 
--------------------------------------------------------
                                      '''
        await bot_.client.send_message(channel,
                                       help)
    except Exception as e:
        print(e)


async def leave(bot_, server_, text_channel):
    try:
        bot_.speak = False
        bot_.tcp_speak = False
        bot_.yt_speak = False
        bot_.file_speak = False
        if bot_.client.is_voice_connected(bot_.client.get_server(server_)):
            await bot_.voice_client.disconnect()
        if bot_.voice_client:
            del bot_.voice_client
        if bot_.p_dl:
            del bot_.p_dl
        if bot_.player:
            del bot_.player
        await bot_.client.send_message(text_channel,
                                       'Bot leaves voice channel')
    except Exception as e:
        print(e)
        return


async def playlist(bot_, channel):
    if bot_.yt_speak:
        tmp = bot_.player.get_playlist()
        count = 1
        message = "Playlist: \n"
        for i in tmp:
            message += str(count) + ". " + i.title + '\n'
            count += 1
        await bot_.client.send_message(channel, message)
    elif bot_.file_speak:
        tmp = bot_.player.get_playlist()
        count = 1
        message = "Playlist: \n"
        for i in tmp:
            filename_w_ext = os.path.basename(i)
            filename, _ = os.path.splitext(filename_w_ext)
            message += str(count) + ". " + filename + '\n'
            count += 1
        await bot_.client.send_message(channel, message)


async def clear_playlist(bot_, text_channel):
    if bot_.yt_speak or bot_.file_speak:
        if len(bot_.player.get_playlist()) > 0:
            bot_.player.clear_queue()
            await bot_.client.send_message(text_channel,
                                           'Playlist was cleared')


async def stop(bot_, text_channel):
    if bot_.player:
        if bot_.player.is_playing:
            bot_.player.stop()
            bot_.speak = False
            bot_.yt_speak = False
            bot_.tcp_speak = False
            await bot_.client.send_message(text_channel,
                                           'Bot stop playing')
# BOT COMMANDS END


class PCMStream:
    def __init__(self):
        self.stream = sd.InputStream()
        self.stream.start()

    def read(self, num_bytes):
        # frame is 4 bytes

        frames = int(num_bytes / 4)
        data = self.stream.read(frames)[0]
        return data.tobytes()


class Stream:
    bot_ = None

    def __init__(self, bot):
        self.bot_ = bot
        self.count = 0
        self.last_data = b'0' * 3840
        self.ready = False
        self.buffering = True

    def read(self, num_bytes):
        if len(self.bot_.music_data) == 0:
            self.ready = False
        if not self.ready:
            if len(self.bot_.music_data) < 400000:
                # if self.bot_.music_data is None:
                return b'0' * 3840
            else:
                self.ready = True
        if self.last_data == self.bot_.music_data[:num_bytes]:
            self.count += 1
            if self.count > 10:
                self.bot_.music_data = b''
                self.count = 0
                self.ready = False
                return b'0' * 3840
        else:
            self.count = 0
            self.last_data, self.bot_.music_data  = self.bot_.music_data[:num_bytes], self.bot_.music_data[num_bytes:]
        return self.last_data



@bot.client.event
async def on_message(message):
    msg = []
    if message.content.startswith('@@'):
        msg = message.content.split()
        try:
            if 'youtu' in msg[1]:
                asyncio.ensure_future(add_song(bot, msg, message.channel))
            elif 'fjoin' in msg[1]:
                asyncio.ensure_future(
                    file_join(bot, message.server.id, message.author.voice.voice_channel.id, message.channel))
            elif 'join' in msg[1]:
                asyncio.ensure_future(join(bot, message.server.id, message.author.voice.voice_channel.id, message.channel))
            elif 'skip' in msg[1]:
                asyncio.ensure_future(skip(bot, message.channel))
            elif 'stop' in msg[1]:
                asyncio.ensure_future(stop(bot, message.channel))
            elif 'playlist' in msg[1]:
                asyncio.ensure_future(playlist(bot, message.channel))
            elif 'volume' in msg[1]:
                asyncio.ensure_future(change_volume(bot, msg, message.channel))
            elif 'purge' in msg[1]:
                asyncio.ensure_future(purge(bot, msg, message.author.roles, message.channel))
            elif 'rgg' in msg[1]:
                asyncio.ensure_future(rgg(bot, msg, message.channel))
            elif 'speak_tcp' in msg[1]:
                asyncio.ensure_future(speak_tcp(bot, message.server.id, message.author.voice.voice_channel.id, message.channel))
            elif 'speak' in msg[1]:
                asyncio.ensure_future(speak(bot, message.server.id, message.author.voice.voice_channel.id, message.channel))
            elif 'help' in msg[1]:
                asyncio.ensure_future(print_help(bot, message.channel))
            elif 'leave' in msg[1]:
                asyncio.ensure_future(leave(bot, message.server.id, message.channel))
            elif 'clear' in msg[1]:
                asyncio.ensure_future(clear_playlist(bot, message.channel))

        except Exception as e:
            print(e)


def check_events():
    while len(bot.events) > 0:
        i = bot.events.pop(0)
        if 'youtu' in i[1] and not bot.speak:
            asyncio.ensure_future(add_song(bot, i, bot.client.get_channel(default_text_channel_id)))
        elif 'playlist' in [i[1]]:
            asyncio.ensure_future(playlist(bot, bot.client.get_channel(default_text_channel_id)))
        elif 'fjoin' in [i[1]]:
            asyncio.ensure_future(file_join(bot, server_id, default_voice_channel_id, bot.client.get_channel(default_text_channel_id)))
        elif 'join' in [i[1]]:
            asyncio.ensure_future(join(bot, server_id, default_voice_channel_id, bot.client.get_channel(default_text_channel_id)))
        elif 'skip' in [i[1]]:
            asyncio.ensure_future(skip(bot, bot.client.get_channel(default_text_channel_id)))
        elif 'stop' in [i[1]]:
            asyncio.ensure_future(stop(bot, bot.client.get_channel(default_text_channel_id)))
        elif 'volume' in [i[1]]:
            asyncio.ensure_future(change_volume(bot, i, bot.client.get_channel(default_text_channel_id)))
        elif 'rgg' in [i[1]]:
            asyncio.ensure_future(rgg(bot, i, bot.client.get_channel(default_text_channel_id)))
        elif 'speak_tcp' in [i[1]]:
            asyncio.ensure_future(speak_tcp(bot, server_id, default_voice_channel_id, bot.client.get_channel(default_text_channel_id)))
        elif 'speak' in [i[1]]:
            asyncio.ensure_future(speak(bot, server_id, default_voice_channel_id, bot.client.get_channel(default_text_channel_id)))
        elif 'help' in [i[1]]:
            asyncio.ensure_future(print_help(bot, bot.client.get_channel(default_text_channel_id)))
        elif 'leave' in [i[1]]:
            asyncio.ensure_future(leave(bot, server_id, bot.client.get_channel(default_text_channel_id)))
        elif 'clear' in i[1]:
            asyncio.ensure_future(clear_playlist(bot, bot.client.get_channel(default_text_channel_id)))



# @bot.client.event
async def background_loop():
    await bot.client.wait_until_ready()
    await bot.client.change_presence(game=discord.Game(name=''))
    tcp_commands.RunServer(bot, password, files, ip_address, port)
    #asyncio.ensure_future(tcp_commands.start_twisted())
    # while not bot.client.is_closed:
    #     for Member in bot.client.get_all_members():
    #         Roles = Member.roles
    #         if len(list(filter(lambda x: x.name == 'DOG', Roles))) > 0:
    #             if not bot.client.is_voice_connected(bot.client.get_server(server_id)):
    #                 await bot.client.join_voice_channel(ch)
    #             await bot.client.move_member(Member, ch)
    while not bot.client.is_closed:
        try:
            if bot.yt_speak:
                if bot.player.get_playlist():
                    if bot.curr_song != bot.player.get_playlist()[0]:
                        bot.curr_song = bot.player.get_playlist()[0]
                        await bot.client.change_presence(game=discord.Game(name=bot.curr_song.title))
                else:
                    await bot.client.change_presence(game=discord.Game(name=''))
        except Exception as e:
            print(e)

        check_events()

        if bot.tcp_speak:
            if bot.player.is_alive():
                pass
            else:
                if bot.player.error is None:
                    bot.player.stop()
                    bot.player = bot.voice_client.create_stream_player(Stream(bot))
                    bot.player.start()
        await asyncio.sleep(1)


@bot.client.event
async def on_ready():
    print('Logged in as')
    print(bot.client.user.name)
    print(bot.client.user.id)
    print('------')

bot.client.loop.create_task(background_loop())
bot.client.run(token)

