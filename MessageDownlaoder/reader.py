#coding:utf-8

import requests
from bs4 import BeautifulSoup
import re
import urllib

head = {'User-Agent':'Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.1.6) Gecko/20091201 Firefox/3.5.6'}
baseUrl = 'http://www.meizitu.com/'

def read_child_page(href):
    print(page)

def read_page(href):
    page = requests.session().get(href,headers=head)
    content = page.content
    contentSoup = BeautifulSoup(content,from_encoding='gb18030')
    picture_divs = contentSoup.find_all('div',{'class':'postContent'})
    for picture_div in picture_divs:
        child_page_url = picture_div.find('a').atttrs['href']
        read_child_page(child_page_url)
    nextUrl = baseUrl + contentSoup.find('a',text='下一页').attrs['href']
    read_page(nextUrl)
    print(page)