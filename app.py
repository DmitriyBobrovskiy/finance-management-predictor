#импорт модулей
import pandas as pd
import numpy as np
from os import getenv
from sqlalchemy import create_engine

#строка подключения из переменных окружения
env_connection_data = {
	'database_vendor': getenv('DATABASE_VENDOR'),
	'host': getenv('HOST'),
	'port': getenv('PORT'),
	'user': getenv('USERNAME'),
	'pswd': getenv('PASSWORD'),
	'database': getenv('DATABASE'),
}

connection_string = '{database_vendor}://{user}:{pswd}@{host}:{port}/{database}'.format_map(env_connection_data)
engine = create_engine(connection_string)

#запрос таблицы в бд
df = pd.read_sql_table("Transactions", con=engine)
#отсортированная таблица
df = df.sort_values('Date')

#берем лишь транзакции с тратой средств
spent_df = df.query("TransactionTypeId == 1")

#берем лишь транзакции с с получением средств
earn_df = df.query("TransactionTypeId == 2")

#пустая таблица, в последующем будет заполнена и записана в бд
res_pd = pd.DataFrame(columns=['date', 'amount', 'Lt', 'Tt'])

#коэфициент сглаживания тренда, задается от 0 до 1
b=0.1
#коэффициент сглаживания ряда
k=0.2
#тренд, инициализируется нулем
t=0
#обход всей полей таблицы
Lt = spent_df.iloc[0].Amount
Tt = 0
prev_amount = 0
for row in spent_df.itertuples():
    date = row.Date
    amount = row.Amount
    Lt = t * amount + (1-k)*(prev_amount - Tt)
    Tt = b*(amount-prev_amount)+(1-b)*Tt
    instance = pd.DataFrame({'date':[date], 'amount':[amount], 'Lt':[Lt], 'Tt':[Tt]})
    res_pd = res_pd.append(instance, ignore_index = True)
    prev_amount = amount
res_pd.to_sql('spent_data', con=engine)

#коэфициент сглаживания тренда, задается от 0 до 1
b=0.1
#коэффициент сглаживания ряда
k=0.2
#тренд, инициализируется нулем
t=0
#обход всей полей таблицы
Lt = spent_df.iloc[0].Amount
Tt = 0
prev_amount = 0
for row in earn_df.itertuples():
    date = row.Date
    amount = row.Amount
    Lt = t * amount + (1-k)*(prev_amount - Tt)
    Tt = b*(amount-prev_amount)+(1-b)*Tt
    instance = pd.DataFrame({'date':[date], 'amount':[amount], 'Lt':[Lt], 'Tt':[Tt]})
    res_pd = res_pd.append(instance, ignore_index = True)
    prev_amount = amount
res_pd.to_sql('earn_data', con=engine)