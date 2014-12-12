#coding:utf-8

import requests
from bs4 import BeautifulSoup
import re
import urllib
import dbhelper
import hashlib

head = {'User-Agent':'Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6'}
baseUrl = 'http://www.meizitu.com'

def getmd5(str):
    str = str.encode()
    m = hashlib.md5()
    m.update(str)
    return m.hexdigest()

<<<<<<< HEAD
def downImg(url,url_id):
    page = requests.session().get(url,headers=head)
    content = page.content
    #encoding = content.encoding
    contentSoup = BeautifulSoup(content,from_encoding='gb18030')
    infobox = contentSoup.find('div',{'class':'boxinfo'})
    infos = infobox.find_all('p')
    title = infos[0].text
    description = ''
    if len(infos) >= 2:
        description = infos[0].text + '\r\n' + infos[1].text
    tagStr = contentSoup.find('div',{'class':'metaRight'}).find('p').text
    pattern = re.compile(u'[\u4E00-\u9FFF]+')
    tags = pattern.findall(tagStr)
    
    dbhelper.add_page(url_id,'','',title,description,tags)

def read_child_page():
    rows = dbhelper.get_top_urls(10)
    ids = ''
    for row in rows:
        if(len(ids) > 0):
            ids = ids + ','
        ids = ids + str(row[0])
    if  dbhelper.set_url_reading(ids) > 0:
        for row in rows:
            downImg(row[1],row[0])
=======
def read_child_page(url):
    print(url)
>>>>>>> d8291d8775a88a6b75e9a66b174b2acf263bb144

def read_page(url):
    page = requests.session().get(url,headers=head)
    content = page.content
    contentSoup = BeautifulSoup(content,from_encoding='gb18030')
    picture_divs = contentSoup.find_all('div',{'class':'postContent'})
    cheildUrls = []
    for picture_div in picture_divs:
        child_page_url = picture_div.find('a').attrs['href']
        cheildUrls.append([child_page_url,getmd5(child_page_url)])
<<<<<<< HEAD
    
=======
        #read_child_page(child_page_url)
>>>>>>> d8291d8775a88a6b75e9a66b174b2acf263bb144
    dbhelper.add_url(cheildUrls)
    nextUrl = contentSoup.find('a',text = '下一页').attrs['href']
    if nextUrl.startswith('/a'):
        nextUrl = baseUrl + nextUrl
    else:
        nextUrl = baseUrl + '/a/' + nextUrl
    read_page(nextUrl)
    print(url)
   

if __name__ == '__main__':
<<<<<<< HEAD
    #read_page(baseUrl)
    read_child_page()
=======
    read_page(baseUrl)
>>>>>>> d8291d8775a88a6b75e9a66b174b2acf263bb144
