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

def get_bytes_md5(bytes):
    m = hashlib.md5()
    m.update(bytes)
    return m.hexdigest()
    

def get_img_urls(content):
   imgs = content.find('div',{'id':'picture'}).find_all('img')
   if len(imgs) == 0:
       return 0
   else:
       urls = []
       for img_node in imgs:
          urls.append(img_node.attrs['src'])
       return urls

def save_img_file(bytes):
    md5 = get_bytes_md5(bytes)
    img_name = str.format("D:\\imgs\\{0}.jpg",md5)
    with open(img_name,'wb') as f:
        f.write(bytes)

def down_imgs(urls):
    img_byte_arr = []
    for url in urls:
        r = requests.session().get(url,stream=True)
        img_byte_arr.append(r.content)
        save_img_file(r.content)
    return img_byte_arr


def save_page(url,url_id):
    page = requests.session().get(url,headers=head,stream=True)
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
    
    page_id = dbhelper.add_page(url_id,'','',title,description,tags)
    img_urls = get_img_urls(contentSoup)
    bytes_arr = down_imgs(img_urls)

def read_child_page():
    rows = dbhelper.get_top_urls(10)
    ids = ''
    for row in rows:
        if(len(ids) > 0):
            ids = ids + ','
        ids = ids + str(row[0])
    if  dbhelper.set_url_reading(ids) > 0:
        for row in rows:
            save_page(row[1],row[0])

def read_page(url):
    page = requests.session().get(url,headers=head)
    content = page.content
    contentSoup = BeautifulSoup(content,from_encoding='gb18030')
    picture_divs = contentSoup.find_all('div',{'class':'postContent'})
    cheildUrls = []
    for picture_div in picture_divs:
        child_page_url = picture_div.find('a').attrs['href']
        cheildUrls.append([child_page_url,getmd5(child_page_url)])
    dbhelper.add_url(cheildUrls)
    nextUrl = contentSoup.find('a',text = '下一页').attrs['href']
    if nextUrl.startswith('/a'):
        nextUrl = baseUrl + nextUrl
    else:
        nextUrl = baseUrl + '/a/' + nextUrl
    read_page(nextUrl)
    print(url)
   

if __name__ == '__main__':
    #read_page(baseUrl)
     read_child_page()