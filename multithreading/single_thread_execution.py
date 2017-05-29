"""This is a implementation of multithreading design pattern Single Thread Excecution."""
# -*- coding: utf-8 -*-

import thread
import threading
import time

class Gate:
    def __init__(self):
        self.lock = threading.Lock()
        self.counter = 0
        self.name = None
        self.address = None

    def go(self, name, address):
        with self.lock:
            self.counter += 1
            if self.counter % 10000 == 0:
                print("** " + str(self.counter) + " **")
            self.name = name
            self.address = address
            self.check()

    def check(self):
        if self.name[0] != self.address[0]:
            print("*** BROKEN **" + str(self.counter))

def userthread(gate, name, address):
    while True:
        gate.go(name, address)

if __name__ == "__main__":
    gate = Gate()
    thread.start_new_thread(userthread, (gate, "Alice", "Alaska",))
    thread.start_new_thread(userthread, (gate, "Bobby", "Brazail",))
    thread.start_new_thread(userthread, (gate, "Chris", "Canada",))
    time.sleep(1000000)
