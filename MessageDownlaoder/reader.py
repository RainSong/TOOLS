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

def read_child_page(url):
    print(url)

def read_page(url):
    page = requests.session().get(url,headers=head)
    content = page.content
    contentSoup = BeautifulSoup(content,from_encoding='gb18030')
    picture_divs = contentSoup.find_all('div',{'class':'postContent'})
    cheildUrls = []
    for picture_div in picture_divs:
        child_page_url = picture_div.find('a').attrs['href']
        cheildUrls.append([child_page_url,getmd5(child_page_url)])
        #read_child_page(child_page_url)
    dbhelper.add_url(cheildUrls)
    nextUrl = contentSoup.find('a',text = '下一页').attrs['href']
    if nextUrl.startswith('/a'):
        nextUrl = baseUrl + nextUrl
    else:
        nextUrl = baseUrl + '/a/' + nextUrl
    read_page(nextUrl)
    print(url)

if __name__ == '__main__':
    read_page(baseUrl)