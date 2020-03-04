import sys
import datetime
import requests
import os
import urllib.request 
import pymysql.cursors

from mysql.connector import MySQLConnection, Error
from ebaysdk.finding import Connection as finding
from bs4 import BeautifulSoup

 
# Подключиться к базе данных. 
connection = pymysql.connect(host='mydb.cmkwiytbbvat.eu-central-1.rds.amazonaws.com',
                             user='admin_amazon',
                             password='Amazon_2019',                             
                             db='api_ebay'
                            )
 
print ("connect successful!!")

a=datetime.datetime.now()

   

def insert_price(price,product_id):
    a=datetime.datetime.now()
    date=a.__str__()
   
    query = "INSERT INTO price_product(price,date,product_id) VALUES(%s,%s,%s)"
    args = (price,date,product_id)

    try:
        
        cursor = connection.cursor()
        cursor.execute(query, args)

        """if cursor.lastrowid:
            print('last insert id', cursor.lastrowid)
        else:
            print('last insert id not found')"""

        connection.commit()
    except Error as error:
        print(error)

    finally:
        cursor.close()

def update_price(price,product_id):
    
    
    query = "UPDATE product SET price=%s where id=%s"
    
    args = (price,product_id)

    try:
        
        cursor = connection.cursor()
        cursor.execute(query, args)

        """if cursor.lastrowid:
            print('last insert id', cursor.lastrowid)
        else:
            print('last insert id not found')"""

        connection.commit()
    except Error as error:
        print(error)

    finally:
        cursor.close()


def get_new_price(url):
    try:
        html=requests.get(url)
        soup = BeautifulSoup(html.text,'lxml')
        items = soup.find_all('span', class_='notranslate')
        name=items[0].attrs['content']
    except KeyError:
        name=0
    except IndexError:
        name=0
    
               
    return name

def update():
    query = "SELECT id,url FROM product"
        
    kl=0
    try:
        
        cursor = connection.cursor()
        cursor.execute(query)

        rows = cursor.fetchall()
        #print('Total Row(s):', cursor.rowcount)
        for i in range(0,cursor.rowcount):
        
            print((rows[i])[1])
            if (get_new_price((rows[i])[1])==''):
                insert_price( 0,(rows[i])[0])
            else:
                insert_price( get_new_price((rows[i])[1]),(rows[i])[0])
                update_price( get_new_price((rows[i])[1]),(rows[i])[0])
        
        connection.commit()
    except Error as error:
        print(error)

    finally:
        cursor.close()


update()
date11=a.__str__().split(' ')

a1=datetime.datetime.now()
date1=a1.__str__().split(' ')
str1=date1[1].split('.')
str="connect successful!! "+date11[0].__str__()+' '+date11[1].__str__().split('.')[0]+' '+str1[0].__str__()
with open(os.path.abspath("log.txt"), "a") as file:
    file.write(str+'\n')
