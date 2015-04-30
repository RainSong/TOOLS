"""
The flask application package.
"""

from flask import Flask
app = Flask(__name__)
app.config['BABEL_DEFAULT_LOCALE'] = 'zh_CN'
import MyReader.views
