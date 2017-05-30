"""This is a implementation of multithreading design pattern Balking."""
# -*- coding: utf-8 -*-

import thread
import threading
import time
import random

class Data:
    def __init__(self, filename, content):
        self.lock = threading.Lock()
        self.file_nane = filename
        self.content = content
        self.changed = False

    def change(self, new_content):
        with self.lock:
            self.content = new_content
            self.changed = True

    def save(self, caller):
        with self.lock:
            if self.changed:
                self.__do_save()
                self.changed = False
                print("Saved from " + caller + "(" + self.content + ")")
    
    def __do_save(self):
        with open(self.file_nane, 'w') as file:
            file.write(self.content)

def saver_thread(data):
    while True:
        data.save("from saver_thread")
        time.sleep(random.random())

def changer_thread(data):
    i = 0
    while True:
        data.change("No." + str(i))
        time.sleep(random.random())
        data.save("from changer_thread")
        i += 1

if __name__ == "__main__":
    data = Data("data.tmp", "empty")
    thread.start_new_thread(saver_thread, (data,))
    thread.start_new_thread(changer_thread, (data,))
    time.sleep(1000000)
