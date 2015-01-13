import os
import time
import sqlite3
import logging

dbPath = os.getcwd() + '\\Data\\2014.db'

logPath = os.getcwd() + '\\Logs\\' + time.strftime('%Y-%m-%d',time.localtime(time.time())) + '.log'
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
       if exists_url(url[1]) == False:
           sql = sql + "insert into urls(url,md5) values('{0}','{1}');\n".format(url[0],url[1])
   print(sql)
   conn.executescript(sql)
   conn.commit()
   conn.close()

def get_top_urls(top_num):
    sql = 'select  id,url from urls where is_readed in (0,1) order by add_time desc limit 0,{0}'.format(top_num)
    conn = sqlite3.connect(dbPath,timeout=1000)
    cur = conn.cursor()
    cur.execute(sql)
    rows = cur.fetchall()
    conn.close()
    return rows

def set_url_reading(ids):
    sql = 'update urls set is_readed = 1 where id in ({0})'.format(ids)
    conn = sqlite3.connect(dbPath,timeout=1000)
    cur = conn.cursor()
    cur.execute(sql)
    conn.commit()
    conn.close()
    return cur.rowcount

def add_tag(page_id,tags,cur):
    exists_tag_sql = "select id from tags where tag = '{0}' limit 0,1"
    insert_tag_sql = "insert into tags (tag) values('{0}')"
    insert_page_tag_sql = "insert into page_tag(page_id,tag_id) values({0},{1})"
    tag_id = 0
    for tag in tags:
        cur.execute(exists_tag_sql.format(tag))
        if cur.rowcount < 0:
           cur.execute(insert_tag_sql.format(tag))
           cur.execute("select last_insert_rowid()")
           row = cur.fetchone()
           tag_id = row[0]
        else:
            row = cur.fetchone()
            tag_id = row[0]
        cur.execute(insert_page_tag_sql.format(page_id,tag_id))

def add_page(url_id,encoding,content,title,description,tags):
    exists_page_saq = "select id from page_info where url_id={0}".format(url_id)
    sql = "insert into page_info (url_id,encoding,content,title,description) values ({0},'{1}','{2}','{3}','{4}')".format(url_id,encoding,content,title,description)
    conn = sqlite3.connect(dbPath,timeout=1000)
    cur = conn.cursor()
    try:
        cur.execute(exists_page_saq)
        row = cur.fetchone()
        if cur.rowcount < 0:
            cur.execute(sql)
            cur.execute("select last_insert_rowid()")
            row = cur.fetchone()
            page_id = row[0]
            add_tag(page_id,tags,cur)
        else:
            conn.close()
            return  row[0]
        conn.commit()
    except sqlite3.Error as e:
        print(e)
        conn.rollback()
    finally:
        conn.close()
    return page_id
