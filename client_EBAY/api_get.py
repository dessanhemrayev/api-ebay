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
 

# Запись данных. 
def insert_product(id_p,title, price,url, cat_id):
    query = "INSERT INTO product(id,title,price,url, category_product_id) VALUES(%s,%s,%s,%s,%s)"
    args = (id_p,title, price,url,cat_id)

    try:
        
        cursor = connection.cursor()
        cursor.execute(query, args)

       
        connection.commit()
    except Error as error:
        print(error)

    finally:
        cursor.close()


def prowerka_id(id_prc):
    query = "SELECT * FROM product where id=%s"
    args = (id_prc)
    kl=0
    try:
        
        cursor = connection.cursor()
        cursor.execute(query, args)

        rows = cursor.fetchall()
        kl=cursor.rowcount
        if (kl!=0):
            kl=(rows[0])[0]

        connection.commit()
    except Error as error:
        print(error)

    finally:
        cursor.close()
    return kl
def prowerka_category(cat_name):
    query = "SELECT * FROM category_product where name_category=%s"
    args = (cat_name)
    kl=0
    try:
        
        cursor = connection.cursor()
        cursor.execute(query, args)

        rows = cursor.fetchall()
        kl=cursor.rowcount
        if (kl!=0):
            kl=(rows[0])[0]

        connection.commit()
    except Error as error:
        print(error)

    finally:
        cursor.close()
    return kl

def insert_category(cat_name):
    hj=prowerka_category(cat_name)
    if (hj==0):
        query = "INSERT INTO category_product(name_category) VALUES(%s)"
        args = (cat_name)

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
        return cursor.lastrowid
    else:
        return hj

def insert_seller(name_seller, id_product):
    query = "INSERT INTO seller_product(name_seller,product_id) VALUES(%s,%s)"
    args = (name_seller, id_product)

    try:
        
        cursor = connection.cursor()
        cursor.execute(query, args)

       
        connection.commit()
    except Error as error:
        print(error)

    finally:
        cursor.close()

def insert_price(price,product_id):
    a=datetime.datetime.now()
    date=a.__str__().split(' ')
   
    query = "INSERT INTO price_product(price,date,product_id) VALUES(%s,%s,%s)"
    args = (price,date[0],product_id)

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

def convertToBinaryData(filename):
    # Преобразование цифровых данных в двоичный формат
    with open(filename, 'rb') as file:
        binaryData = file.read()
    return binaryData

def remove_image(adr_path):
    path = os.path.join(os.path.abspath(os.path.dirname(__file__)), adr_path)
    os.remove(path)


def insert_image(adress,prod_id):
    query = "INSERT INTO image_product(adress_save,product_id) VALUES(%s,%s)"
    picture = convertToBinaryData(adress)
    args = (picture,prod_id)

    try:
        
        cursor = connection.cursor()
        cursor.execute(query, args)

      
        connection.commit()
    except Error as error:
        print(error)

    finally:
        remove_image(adress)
        cursor.close()

def savePhoto(id_pro,url, cat_product):
    
    r=requests.get(url,stream=True)
    image=url.split('/')
    name_for_image=id_pro+image[-1]
    #folder=name_for_image.split('.')
    #name_folder=folder[0]+id_pro
    home_adress_for_saving_images="D:/projects/курсовые/базы_данных/client_EBAY/client_EBAY/bin/Debug/images/"
    #+cat_product+"/"+name_folder
    if not os.path.exists(home_adress_for_saving_images):
        os.makedirs(home_adress_for_saving_images)
    with open(home_adress_for_saving_images+'/'+name_for_image, 'bw') as f:
        for kusok in r.iter_content(1024):
            f.write(kusok)
    
    return home_adress_for_saving_images+'/'+name_for_image


def seller_name(url):
    html=requests.get(url)
    soup = BeautifulSoup(html.text,'lxml')
    items = soup.find_all('span', class_='mbg-nw')
    name="" 
    for item in items:
        name= item.contents[0]
        break
    return name

def url_image_adres(url):
    html=requests.get(url)
    soup = BeautifulSoup(html.text,'lxml')
    items = soup.find_all('img', class_='img img300')
    name=''
    for item in items:
        name= item.attrs["src"]
        break
    
    return name
    


# подключение к api. 
ID_APP = 'DessanHe-DessanHe-PRD-5b31d5c2f-6461a8ae'

Keywords = sys.argv[1]
#Keywords = "Apple products"
api = finding(appid=ID_APP, config_file=None)
api_request = { 'keywords': Keywords }
response = api.execute('findItemsByKeywords', api_request)
soup = BeautifulSoup(response.content,'lxml')

#totalentries = int(soup.find('totalentries').text)
items = soup.find_all('item')
k=0 
#print(items)

for item in items:
    k+=1
    if (k<15):
        cat = item.categoryname.string
        title = item.title.string
        price = int(round(float(item.currentprice.string)))
        url = item.viewitemurl.string.lower()
        photourl= url_image_adres(url)
        id_pr=item.itemid.string
        if (prowerka_id(id_pr)==0):
            last_row_id=insert_category(cat)
            insert_product(id_pr, title,float(price),url, last_row_id)
            insert_seller(seller_name(url), id_pr)
            insert_image(savePhoto(id_pr,photourl,cat), id_pr)
            insert_price(price, id_pr)

        print(k,'________', sep=" ")
        #print('seller:\n'+ seller_name(url)+'\n')
        #print('title:\n' + title + '\n')
        #print('photourl:\n' + url_image_adres(url) + '\n')
        #print('price:\n' + str(price) + '\n')
        #print('url:\n' + url + '\n')

 
