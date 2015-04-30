# -*- coding: utf-8 -*-

"""
Routes and views for the flask application.
"""

from datetime import datetime
from flask import render_template
from MyReader import app
import datahelper
import sys

#sys.setdefaultencoding('utf-8')

@app.route('/')
@app.route('/home')
def home():
    """Renders the home page."""
    return render_template(
        'index.html',
        title='Home Page',
        year=datetime.now().year,
        tags=datahelper.getTags(),
        pages=[]#datahelper.getPags()
    )

@app.route('/contact')
def contact():
    """Renders the contact page."""
    return render_template(
        'contact.html',
        title='Contact',
        year=datetime.now().year,
        message='Your contact page.'
    )

@app.route('/about')
def about():
    """Renders the about page."""
    return render_template(
        'about.html',
        title='About',
        year=datetime.now().year,
        message='Your application description page.'
    )
