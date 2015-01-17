#coding:utf-8


import os
import requests
from bs4 import BeautifulSoup
import re
import urllib
import dbhelper
import hashlib
import threading

head = {'User-Agent':'Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6'}
baseUrl = 'http://www.meizitu.com'

def getmd5(str):
    str = str.encode()
    m = hashlib.md5()
    m.update(str)
    return m.hexdigest()

def get_bytes_md5(bytes):
    m = hashlib.md5()
    m.update(bytes)
    return m.hexdigest()
    

def get_img_urls(content):
   imgbox = content.find('div',{'id':'picture'})
   if imgbox == None:
       imgbox = content.find('div',{'class','postContent'})
   if imgbox == None:
       return []
   imgs = imgbox.find_all('img')
   if len(imgs) == 0:
       return []
   else:
       urls = []
       for img_node in imgs:
          urls.append(img_node.attrs['src'])
       return urls

def save_img_file(bytes,url_md5):
    img_md5 = get_bytes_md5(bytes)
    path = str.format("D:\\imgs\\{0}",url_md5)
    if not os.path.exists(path):
        os.makedirs(path)
    img_name = str.format("{0}\\{1}.jpg",path,img_md5)
    with open(img_name,'wb') as f:
        f.write(bytes)

def down_imgs(urls,url_md5):
    img_arr = []
    for url in urls:
        print('\r########下载URL为{0}的图片'.format(url))
        r = requests.session().get(url,stream=True)
        save_img_file(r.content,url_md5)
        md5 = get_bytes_md5(r.content)
        arr = [md5,url,r.content]
        img_arr.append(arr)
    return img_arr


def save_page(url_id,url,url_md5):
    print('\r\r####读取页面{0}中的图片URL'.format(url))
    try:
        page = requests.session().get(url,headers=head)
        content = page.content
        description = ''
        title = ''
        tags = []

        contentSoup = BeautifulSoup(content,from_encoding='gb18030')
        infobox = contentSoup.find('div',{'class':'boxinfo'})
        if infobox != None:
            infos = infobox.find_all('p')
            title = infos[0].text
            description = ''
            if len(infos) >= 2:
                description = infos[0].text + '\r\n' + infos[1].text
        tagbox = contentSoup.find('div',{'class':'metaRight'})
        if tagbox != None:
            pattern = re.compile(u'[\u4E00-\u9FFF]+')
            tags = pattern.findall(tagbox.find('p').text)
            if len(title)==0:
                h2 = tagbox.find('h2')
                if h2 != None:
                    title = h2.text

        page_id = dbhelper.add_page(url_id,'','',title,description,tags)
        if page_id != 0:
            img_urls = get_img_urls(contentSoup)
            img_arr = down_imgs(img_urls,url_md5)
            print('\r####保存页面{0}中的图片'.format(url))
            dbhelper.save_page_image(img_arr,page_id)
            print('\r####设置URL{0}为已读状态\r'.format(url))
            dbhelper.set_url_readed(url_id)
        else:
            dbhelper.set_url_readerror(url_id)
    except Exception as e:
        print('\r####页面{0}下载失败'.format(url))
        print('\r####失败原因：{0}\r'.format(e))
        dbhelper.set_url_readerror(url_id)

def read_child_page():
    failed_times = 0
    while True:
        try:
            rows = dbhelper.get_top_urls(10)
            if len(rows) <= 0:
                if failed_times < 10:
                    failed_timds = failed_times + 1
                    continue
                else:
                    print('\r\r####图片下载完成')
                    break
            ids = ''
            failed_timds = 0
            for row in rows:
                if(len(ids) > 0):
                    ids = ids + ','
                ids = ids + str(row[0])
            if  dbhelper.set_url_reading(ids) > 0:
                for row in rows:
                    save_page(row[0],row[1],row[2])
        except Exception as e:
            print(e)

def read_page():
    failed_times = 0
    index = 0
    while failed_times < 10:
        url = str.format('http://www.meizitu.com/a/list_1_{0}.html',index)
        index = index + 1
        print(str.format('\r读取页面：{0}',url))
        try:
            response = requests.session().get(url,headers=head)
            if response.status_code != 200 or len(response.content) < 1000:
                failed_times = failed_times + 1
                continue
            else:
                failed_times = 0
            content = response.content
            contentSoup = BeautifulSoup(content,from_encoding='gb18030')
            picture_divs = contentSoup.find_all('div',{'class':'postContent'})
            childUrls = []
            for picture_div in picture_divs:
                child_page_url = picture_div.find('a').attrs['href']
                childUrls.append([child_page_url,getmd5(child_page_url)])
                print('####保存URL：' + child_page_url)
            dbhelper.add_url(childUrls)
            print("\r\r")
        except:
            print('\r读取页面{0}时发生错误！'.format(url))
    print('\r页面路径读取完成！')
        #nextlink = contentSoup.find('a',text = '下一页')
        #if nextlink != None:
        #    nextUrl = nextlink.attrs['href']
        #    if nextUrl.startswith('/a'):
        #        nextUrl = baseUrl + nextUrl
        #    else:
        #        nextUrl = baseUrl + '/a/' + nextUrl
if __name__ == '__main__':
    read_all = False
    while True:
        print('是否重新读取所有页面？')
        print('(Y/y)是，将会重新抓取所有页面，速度较慢')
        print('(N/n)否，将会从已有的URL中下载图片')
        word_input = input()
        if word_input == 'Y' or word_input == 'y':
            read_all = True
            break
        elif word_input == 'N' or word_input == 'n':
            read_all = False
            break
        else:
            continue
    if read_all:
        t1 = threading.Thread(target=read_page)
        t1.start()
    t2 = threading.Thread(target=read_child_page)
    t2.start()