#coding:utf-8

import logging

logging.basicConfig(leveal=logging.INFO)
logging.config.fileConfig("logging.conf")

infoLogger = logging.getLogger("infoLogger")
errorLogger = logging.getLogger("errorLogger")