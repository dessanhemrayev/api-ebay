import pymysql.cursors  
 
# Подключиться к базе данных. 
connection = pymysql.connect(host='localhost',
                             user='root',
                             password='dessan_ib',                             
                             db='api_ebay'
                            )
 
print ("connect successful!!")
 


def insert_book(title,about, price,url):
    query = "INSERT INTO product(id,title,about, price,url) VALUES(null,%s,%s,%s,%s)"
    args = (title,about, price,url)

    try:
        
        cursor = connection.cursor()
        cursor.execute(query, args)

        if cursor.lastrowid:
            print('last insert id', cursor.lastrowid)
        else:
            print('last insert id not found')

        connection.commit()
    except Error as error:
        print(error)

    finally:
        cursor.close()
        connection.close()