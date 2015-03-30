from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy import Column, Integer, String,DateTime
import datetime

base = declarative_base()

class url(base):
    __tablename__ = "url"

    id = Column(Integer,primary_key=True)
    url = Column(String)
    add_time = Column(DateTime,default=datetime.datetime.now())
    md5 = Column(String)
    is_readed = Column(Integer,default=0)

class tag(base):
    __tablename__ = "tag"

    id = Column(Integer,primary_key=True)
    tag = Column(String)
    add_time = Column(DateTime,default=datetime.datetime.now())
    citations = Column(Integer,default=1)

class page_tag(base):
    __tablename__ = "page_tag"

    id = Column(Integer,primary_key=True)
    page_id = Column(Integer)
    tag_id = Column(Integer)
    add_time = Column(DateTime,default=datetime.datetime.now())

class page_info(base):
    __tablename__ = "page_info"

    id = Column(Integer,primary_key=True)
    url_id = Column(Integer)
    encoding = Column(String)
    content = Column(String)
    title = Column(String)
    description = Column(String)
    add_time = Column(DateTime,default=datetime.datetime.now())
    is_readed = Column(Integer,default=0)

class page_file(base):
    __tablename__ = "page_file"

    id = Column(Integer,primary_key=True)
    page_id = Column(Integer)
    file_id = Column(Integer)
    add_time = Column(DateTime,default=datetime.datetime.now())

class file_info(base):
    __tablename__= "file_info"

    id = Column(Integer,primary_key=True)
    md5 = Column(String)
    extension = Column(String)
    content = Column(String)
    size = Column(Integer)
    url = Column(String)
    add_time = Column(DateTime,default=datetime.datetime.now())