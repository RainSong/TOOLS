#coding:utf-8

from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy import Column, Integer, String, ForeignKey
import datetime

base = declarative_base()

#class url_table(base):
#    __tablename__ = "url"

#    id = Column(Integer,primary_key=True)
#    url = Column(String)
#    add_time = Column(String)
#    md5 = Column(String)
#    is_readed = Column(Integer,default=0)

#class tag_table(base):
#    __tablename__ = "tag"

#    id = Column(Integer,primary_key=True)
#    tag = Column(String)
#    add_time = Column(datetime,default=datetime.now)
#    citations = Column(Integer,default=1)

#class page_tag_table(base):
#    __tablename__ = "page_tag"

#    id = Column(Integer,primary_key=True)
#    page_id = Column(Integer)
#    tag_id = Column(Integer)
#    add_time = Column(datetime,default=datetime.now)

#class page_info_table(base):
#    __tablename__ = "page_info"

#    id = Column(Integer,primary_key=True)
#    url_id = Column(Integer)
#    encoding = Column(String)
#    content = Column(string)
#    title = Colunm(String)
#    description = Column(String)
#    add_time = Column(datetime,default=datetime.now)
#    is_readed = Column(Integer,default=0)

#class page_file_table(base):
#    __tablename__ = "page_file"

#    id = Column(Integer,primary_key=True)
#    page_id = Column(Integer)
#    file_id = Column(Integer)
#    add_time = Column(datetime,default=datetime.now)

#class file_info_table(base):
#    __tablename__= "file_info"

#    id = Column(Integer,primary_key=True)
#    md5 = Column(String)
#    extension = Column(String)
#    content = Column(String)
#    size = Column(Integer)
#    url = Column(String)
#    add_time = Column(datetime,default=datetime.now)

#class url(self):
#   pass

#class tag(self):
#    pass

#class page_tag(self):
#    pass

#class page_info(self):
#    pass

#class page_file(self):
#    pass

#class file_info(self):
#    pass

#mapper(url,url_table)
#mapper(tag,tag_talbe)
#mapper(page_tag,page_tag_table)
#mapper(page_info,page_info_table)
#mapper(page_file,page_file_table)
#mapper(file_info,file_info_table)


class url(base):
    __tablename__ = "url"

    id = Column(Integer,primary_key=True)
    url = Column(String)
    add_time = Column(String)
    md5 = Column(String)
    is_readed = Column(Integer,default=0)

class tag(base):
    __tablename__ = "tag"

    id = Column(Integer,primary_key=True)
    tag = Column(String)
    add_time = Column(datetime,default=datetime.now)
    citations = Column(Integer,default=1)

class page_tag(base):
    __tablename__ = "page_tag"

    id = Column(Integer,primary_key=True)
    page_id = Column(Integer)
    tag_id = Column(Integer)
    add_time = Column(datetime,default=datetime.now)

class page_info(base):
    __tablename__ = "page_info"

    id = Column(Integer,primary_key=True)
    url_id = Column(Integer)
    encoding = Column(String)
    content = Column(string)
    title = Colunm(String)
    description = Column(String)
    add_time = Column(datetime,default=datetime.now)
    is_readed = Column(Integer,default=0)

class page_file(base):
    __tablename__ = "page_file"

    id = Column(Integer,primary_key=True)
    page_id = Column(Integer)
    file_id = Column(Integer)
    add_time = Column(datetime,default=datetime.now)

class file_info(base):
    __tablename__= "file_info"

    id = Column(Integer,primary_key=True)
    md5 = Column(String)
    extension = Column(String)
    content = Column(String)
    size = Column(Integer)
    url = Column(String)
    add_time = Column(datetime,default=datetime.now)
