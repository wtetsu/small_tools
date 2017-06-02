"""This is a implementation of multithreading design pattern Read-Write-Lock."""
# -*- coding: utf-8 -*-

import thread
import threading
import time
import random

class ReadWriteLock:
    def __init__(self):
        self._reading_readers = 0
        self._waiting_writers = 0
        self._writing_writers = 0
        self._lock = threading.Lock()
        self._prefer_writer = True
        self._lock_wait = threading.Condition()

    def read_lock(self):
        with self._lock:
            while self._writing_writers > 0 or (self._prefer_writer and self._waiting_writers > 0):
                with self._lock_wait:
                    self._lock_wait.wait()
            self._reading_readers += 1

    def read_unlock(self):
        with self._lock:
            self._reading_readers -= 1
            self._prefer_writer = True
            with self._lock_wait:
                self._lock_wait.notifyAll()

    def write_lock(self):
        with self._lock:
            self._waiting_writers += 1
            try:
                while self._reading_readers > 0 or self._writing_writers > 0:
                    with self._lock_wait:
                        self._lock_wait.wait()
            finally:
                self._waiting_writers -= 1
            self._writing_writers += 1

    def write_unlock(self):
        with self._lock:
            self._writing_writers -= 1
            self._prefer_writer = false
            with self._lock_wait:
                self._lock_wait.notifyAll()

class Data:
    def __init__(self):
        self._data = [None] * 10
        self._lock = ReadWriteLock()
    
    def read(self):
        r = [None] * 10
        self._lock.read_lock()
        try:
            for i in xrange(0, len(r)):
                r[i] = self._data[i]
                time.sleep(0.01)
        finally:
            self._lock.read_unlock()
        return r

    def write(self, ch):
        self._lock.write_lock()
        try:
            for i in xrange(0, len(self._data)):
                self._data[i] = ch
                time.sleep(0.01)
        finally:
            self._lock.write_unlock()

def reader_thread(data):
    while True:
        r = data.read()
        print(r)
        time.sleep(random.random())

def write_thread(data):
    i = 0;
    while True:
        data.write(str(i))
        time.sleep(random.random())
        i += 1

if __name__ == "__main__":
    data = Data()
    thread.start_new_thread(reader_thread, (data,))
    thread.start_new_thread(reader_thread, (data,))
    thread.start_new_thread(reader_thread, (data,))
    thread.start_new_thread(write_thread, (data,))
    thread.start_new_thread(write_thread, (data,))
    thread.start_new_thread(write_thread, (data,))
    time.sleep(1000000)

