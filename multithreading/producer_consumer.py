"""This is a implementation of multithreading design pattern Producer-Consumer."""
# -*- coding: utf-8 -*-

import thread
import threading
import time
import collections
import random

class Table:
    def __init__(self, max):
        self.max = max
        self.lock = threading.Lock()
        self.lock_wait = threading.Condition()
        self.cakes = collections.deque()

    def put(self, cake):
        while len(self.cakes) >= self.max:
            with self.lock_wait:
                self.lock_wait.wait()
        with self.lock:
            self.cakes.append(cake)
            print("appended:" + cake)
        with self.lock_wait:
            self.lock_wait.notifyAll()

    def take(self):
        while len(self.cakes) <= 0:
            with self.lock_wait:
                self.lock_wait.wait()
        with self.lock:
            cake = self.cakes.popleft()
            print("taken:" + cake)
        with self.lock_wait:
            self.lock_wait.notifyAll()
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
