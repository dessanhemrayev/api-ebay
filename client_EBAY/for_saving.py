import datetime
import os

def chis(str1):
    path=os.path.abspath("log.txt")
    with open(path, "a") as file:
        file.write(str1+'\n')
    print()

def log(date12):
   
    a1=datetime.datetime.now()
    date1=a1.__str__().split(' ')
    str1=date1[1].split('.')
    str="connect successful!! "+date12[0].__str__()+' '+date12[1].__str__().split('.')[0]+' '+str1[0].__str__()
    with open(os.path.abspath("log.txt"), "a") as file:
        file.write(str)

a=datetime.datetime.now()
date=a.__str__().split(' ')

a1=datetime.datetime.now()
date1=a1.__str__().split(' ')
str1=date1[1].split('.')
str="connect successful!! "+date[0].__str__()+' '+date[1].__str__().split('.')[0]+' '+str1[0].__str__()
chis(str)


'''import os
import requests 
cat_name="USB disk"
url="D:/projects/курсовые/базы_данных/client_EBAY/client_EBAY/bin/Debug/images/"+cat_name
name_for_image=url.split('/')
print(name_for_image[-1])
 '''