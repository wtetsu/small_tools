"""This is a implementation of multithreading design pattern Producer-Consumer."""
# -*- coding: utf-8 -*-

# acquire()
# locked()
# release()

import thread
import threading
import time
import collections
import random

class Table:
    def __init__(self, max):
        self.max = max
        self.lock = threading.Lock()
        self.lock_wait = threading.Lock()
        self.cakes = collections.deque()

    def put(self, cake):
        with self.lock:
            while len(self.cakes) >= self.max:
                pass
            self.cakes.append(cake)
            # if self.lock_wait.locked():
            #     print("release@")
            #     self.lock_wait.release()

    def take(self):
        with self.lock:
            while len(self.cakes) <= 0:
                pass
            cake = self.cakes.popleft()
            # if self.lock_wait.locked():
            #     print("release!")
            #     self.lock_wait.release()
        return cake

class Numbering:
    _id = 0

    @staticmethod
    def next():
        Numbering._id += 1
        return Numbering._id

def maker_thread(table, thread_name):
    while True:
        id = Numbering.next()
        cake = "cake no. %d by %s" % (id, thread_name)
        table.put(cake)
        time.sleep(random.random())

def eater_thread(table, thread_name):
    while True:
        table.take()
        time.sleep(random.random())

if __name__ == "__main__":
    table = Table(3)
    thread.start_new_thread(maker_thread, (table, "maker_thread-1",))
    thread.start_new_thread(maker_thread, (table, "maker_thread-2",))
    thread.start_new_thread(maker_thread, (table, "maker_thread-3",))
    thread.start_new_thread(eater_thread, (table, "eater_thread-1",))
    thread.start_new_thread(eater_thread, (table, "eater_thread-2",))
    thread.start_new_thread(eater_thread, (table, "eater_thread-3",))
    time.sleep(1000000)
