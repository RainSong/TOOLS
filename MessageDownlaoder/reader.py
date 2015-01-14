#coding:utf-8


import os
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
        r = requests.session().get(url,stream=True)
        #img_byte_arr.append(r.content)
        save_img_file(r.content,url_md5)
        md5 = get_bytes_md5(r.content)
        arr = [md5,url,r.content]
        img_arr.append(arr)
    return img_arr


def save_page(url_id,url,url_md5):
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
    
    page_id = dbhelper.add_page(url_id,'','',title,description,tags)
    img_urls = get_img_urls(contentSoup)
    img_arr = down_imgs(img_urls,url_md5)
    dbhelper.save_page_image(img_arr,page_id)
    dbhelper.set_url_readed(url_id)

def read_child_page():
    rows = dbhelper.get_top_urls(10)
    ids = ''
    for row in rows:
        if(len(ids) > 0):
            ids = ids + ','
        ids = ids + str(row[0])
    if  dbhelper.set_url_reading(ids) > 0:
        for row in rows:
            save_page(row[0],row[1],row[2])

def read_page():
    failed_times = 0
    index = 0
    while failed_times < 10:
        index = index + 1
        url = str.format('http://www.meizitu.com/a/list_1_{0}.html',index)
        print(str.format('读取页面：{0}',url))
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
        #nextlink = contentSoup.find('a',text = '下一页')
        #if nextlink != None:
        #    nextUrl = nextlink.attrs['href']
        #    if nextUrl.startswith('/a'):
        #        nextUrl = baseUrl + nextUrl
        #    else:
        #        nextUrl = baseUrl + '/a/' + nextUrl
if __name__ == '__main__':
    read_page()
    print('页面路径读取完成！')
    #read_child_page()