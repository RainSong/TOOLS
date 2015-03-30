#coding:utf-8

import os
from sqlalchemy.engine import create_engine
from sqlalchemy.orm.session import sessionmaker
from models import tag, page_info
from sqlalchemy.sql.expression import desc

#dbPath = "sqlite:///" + os.path.split(os.path.realpath(__file__))[0] + "\\Data\\db1.sqlite"

dbPath = "sqlite:///D:\\workspace\\TOOLS\\MessageDownlaoder\\Data\\meizitu2015.sqlite"

print(dbPath)

engine = create_engine(dbPath,echo=False)
Session = sessionmaker(bind=engine)
session = Session()

def getTags():
    tags = session.query(tag).\
        filter(tag.citations>0).\
        order_by(desc(tag.citations)).\
        all()
    return tags

def getPags():
    pages = session.query(page_info).\
    order_by(desc(page_info.add_time)).\
    limit(10).\
    all()
    return pages
