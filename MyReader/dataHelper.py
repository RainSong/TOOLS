#coding:utf-8

import os
from sqlalchemy.engine import create_engine
from sqlalchemy.orm.session import sessionmaker

dbPath = os.path.split(os.path.realpath(__file__))[0] + '\\Data\\meizitu2015.db'
engine = create_engine(dbPath,echo=True)
Session = sessionmaker(bind=engine)
session = Session()


def get_tags():
    """获取所有的标签"""
    pass

def get_readed_page(pageIndex,pageSize):
    """获取页面"""
    pass
