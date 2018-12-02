import os
import threading
import time

import asyncio
from concurrent.futures import ProcessPoolExecutor
import inspect

class FilePlayer(object):
    def __init__(self, bot, voice_client, playlist):
        self.voice_client = voice_client
        self.media_player = None
        self.is_playing = False
        self.timeout = 0
        self.bot = bot
        self.playlist = playlist
        self.volume = 0.05
        self.thread = None
        self.is_alive = True

        # start this player class as a new thread
        self.thread = threading.Thread(target=self.run, args=())
        self.thread.daemon = True  # Daemonize thread
        self.thread.start()  # Start the execution

    def play(self):
        if self.playlist:
            if self.voice_client:
                if self.playlist:
                    if not self.is_playing:
                        self.media_player = self.voice_client.create_ffmpeg_player(self.playlist[0], after=self.on_song_finished)
                        self.media_player.volume = self.volume
                        self.timeout = 0
                        self.is_playing = True
                        self.media_player.start()
            else:
                print("Voice client not ready yet")

    def on_song_finished(self):
        print('Song finished')
        self.playlist = self.get_playlist()
        self.playlist.pop(0)
        self.is_playing = False
        self.play()

    def skip(self):
        print('Skipping song')
        self.media_player.stop()
        # on_song_finished() will be automatically called which will start the next song

    def queue(self, song):
        self.playlist = self.get_playlist()
        self.playlist.append(song)

    def clear_queue(self):
        self.playlist = self.get_playlist()
        del self.playlist[1:]

    def remove(self, index):
        self.playlist.pop(index)

    def get_playlist(self):
        return self.playlist

    def run(self):
        while self.is_alive:
            self.play()
            time.sleep(1)

    def change_voice(self, vc):
        self.voice_client = vc

    def change_volume(self, volume):
        self.volume = volume
        self.media_player.volume = volume

    def check_vars(self):
        print(self.media_player.__dict__)
        print(self.media_player)
        print(inspect.getmembers(self.media_player, predicate=inspect.ismethod))

    def stop(self):
        self.media_player.stop()
        self.clear_queue()
        del self.media_player
        self.is_alive = False