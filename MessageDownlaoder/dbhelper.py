#coding:utf-8

import os
import time
import sqlite3
import logging

dbPath = os.path.split(os.path.realpath(__file__))[0] + '\\Data\\meizitu2015.db'

logPath = os.path.split(os.path.realpath(__file__))[0] + '\\Logs\\' + time.strftime('%Y-%m-%d',time.localtime(time.time())) + '.log'


def get_time_now():
    time_now = time.time()
    time_now = time.localtime(time_now)
    str_time_now = time.strftime('%Y-%m-%d %H:%M:%S',time_now)
    return str_time_now

def exists_url(md5,cur):
    sql = "select count(1) from url where md5 = '{0}'".format(md5)
    cur.execute(sql)
    row = cur.fetchone()
    return row != None and row[0] > 0

def add_url(urls):
   conn = sqlite3.connect(dbPath,timeout=1000)
   cur = conn.cursor()
   sql = ''
   for url in urls:
       if exists_url(url[1],cur) == False:
           sql = sql + "insert into url(url,md5,add_time,is_readed) values('{0}','{1}','{2}',0);\n".format(url[0],url[1],get_time_now())
   cur.executescript(sql)
   conn.commit()
   conn.close()

def get_top_urls(top_num):
    sql = 'select  id,url,md5 from url where is_readed in (0,1) order by add_time desc limit 0,{0}'.format(top_num)
    conn = sqlite3.connect(dbPath,timeout=1000)
    cur = conn.cursor()
    cur.execute(sql)
    rows = cur.fetchall()
    conn.close()
    return rows

def set_url_reading(ids):
    sql = 'update url set is_readed = 1 where id in ({0})'.format(ids)
    conn = sqlite3.connect(dbPath,timeout=1000)
    cur = conn.cursor()
    cur.execute(sql)
    conn.commit()
    conn.close()
    return cur.rowcount

def set_url_readed(url_id):
    sql = 'update url set is_readed = 2 where id = {0}'.format(url_id)
    conn = sqlite3.connect(dbPath,timeout=1000)
    cur = conn.cursor()
    cur.execute(sql)
    conn.commit()
    conn.close()
    return cur.rowcount

def set_url_readerror(url_id):
    sql = 'update url set is_readed = 4 where id = {0}'.format(url_id)
    conn = sqlite3.connect(dbPath,timeout=1000)
    cur = conn.cursor()
    cur.execute(sql)
    conn.commit()
    conn.close()
    return cur.rowcount
def save_img(md5,url,content,page_id,cur):
    exists_img_sql = "select id from file_info where md5 = '{0}'".format(md5)
    insert_img_sql = "insert into file_info(md5,extension,content,size,url,add_time) values (?,'jpg',?,?,?,?)"
    cur.execute(exists_img_sql)
    row = cur.fetchone()
    if row == None:
        cur.execute(insert_img_sql,[md5,sqlite3.Binary(content),len(content),url,get_time_now()])
        cur.execute("select last_insert_rowid()")
        row = cur.fetchone()
        return row[0]
    else:
        return row[0]

def save_page_image(img_arr,page_id):
    conn = sqlite3.connect(dbPath,timeout=1000)
    cur = conn.cursor()
    for img_info in img_arr:
       img_id = save_img(img_info[0],img_info[1],img_info[2],page_id,cur)
       sql = "insert into page_file(page_id,file_id,add_time) values({0},{1},'{2}')".format(page_id,img_id,get_time_now())
       cur.execute(sql)
       conn.commit()
    return 0

def add_tag(page_id,tags,cur):
    exists_tag_sql = "select id from tag where tag = '{0}'"
    insert_tag_sql = "insert into tag(tag,add_time) values('{0}','{1}')"
    insert_page_tag_sql = "insert into page_tag(page_id,tag_id,add_time) values({0},{1},'{2}')"
    update_citations_sql = "update tag set citations = citations + 1 where id = {0}"
    tag_id = 0
    for tag in tags:
        cur.execute(exists_tag_sql.format(tag))
        row = cur.fetchone()
        if row == None:
           cur.execute(insert_tag_sql.format(tag,get_time_now()))
           cur.execute("select last_insert_rowid()")
           row = cur.fetchone()
           tag_id = row[0]
        else:
            tag_id = row[0]
            cur.execute(update_citations_sql.format(tag_id))
        cur.execute(insert_page_tag_sql.format(page_id,tag_id,get_time_now()))

def add_page(url_id,encoding,content,title,description,tags):
    exists_page_saq = "select id from page_info where url_id={0}".format(url_id)
    sql = "insert into page_info (url_id,encoding,content,title,description,add_time) values ({0},'{1}','{2}','{3}','{4}','{5}')".format(url_id,encoding,content,title,description,get_time_now())
    conn = sqlite3.connect(dbPath,timeout=1000)
    cur = conn.cursor()
    page_id = 0
    try:
        cur.execute(exists_page_saq)
        row = cur.fetchone()
        if row == None:
            cur.execute(sql)
            cur.execute("select last_insert_rowid()")
            row = cur.fetchone()
            page_id = row[0]
            add_tag(page_id,tags,cur)
        else:
            return  row[0]
        conn.commit()
    except sqlite3.Error as e:
        print(e)
        conn.rollback()
        page_id = 0
    finally:
        conn.close()
    return page_id
