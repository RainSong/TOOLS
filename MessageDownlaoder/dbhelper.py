import os
import time
import sqlite3

dbPath = os.getcwd() + '\\Data\\2014.db'

def exists_url(md5):
    conn = sqlite3.connect(dbPath,1000)
    sql = "select count(1) from urls where md5 = '{0}'".format(md5)
    cursor = conn.cursor()
    print(sql)
    cursor.execute(sql)
    dr = cursor.fetchone()
    conn.close()
    return dr[0] > 0

def add_url(urls):
   conn = sqlite3.connect(dbPath,timeout=1000)
   print('connnect db success')
   sql = ''
   for url in urls:
       #addTime = time.time()
       #addTime = time.localtime(addTime)
       #addTime = time.strftime('%Y-%m-%d %H:%M:%S',addTime)
       if exists_url(url[1]) == False:
           sql = sql + "insert into urls(url,md5) values('{0}','{1}');\n".format(url[0],url[1])
   print(sql)
   conn.executescript(sql)
   conn.commit()
   conn.close()
