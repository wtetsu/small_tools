import thread
import threading
import time
import collections
import random

class RequestQueue:
    def __init__(self):
        self.queue = collections.deque()
        self.wait_lock = threading.Lock()
        self.get_lock = threading.Lock()
        self.put_lock = threading.Lock()
    
    def get_request(self):
        with self.get_lock:
            r = None
            while True:
                if len(self.queue) == 0:
                    self.wait_lock.acquire()
                else:
                    r = self.queue.popleft()
                    break
        return r
        
    def put_request(self, request):
        with self.put_lock:
            self.queue.append(request)
            if self.wait_lock.locked():
                self.wait_lock.release()

def server(requestQueue, threadname):
    i = 0
    while True:
        requestQueue.put_request(threadname + " > message" + str(i))
        time.sleep(random.random())
        i += 1

def client(requestQueue, threadname):
    while True:
        r = requestQueue.get_request()
        print(r + ":" + " > " + threadname)
        time.sleep(random.random())

if __name__ == '__main__':
    requestQueue = RequestQueue()
    thread.start_new_thread(server, (requestQueue, 'server_thread1',))
    thread.start_new_thread(server, (requestQueue, 'server_thread2',))
    thread.start_new_thread(client, (requestQueue, 'client_thread1',))
    thread.start_new_thread(client, (requestQueue, 'client_thread2',))
    time.sleep(1000000)
