#импорт модулей
import pandas as pd
import numpy as np
from os import getenv
from sqlalchemy import create_engine

print(40*' =*= ')
env_connection_data = {
	'database_vendor': getenv('DATABASE_VENDOR'),
	'host': getenv('HOST'),
	'port': getenv('PORT'),
	'user': getenv('USERNAME'),
	'pswd': getenv('PASSWORD'),
	'database': getenv('DATABASE'),
}
connection_string = '{database_vendor}://{user}:{pswd}@{host}:{port}/{database}'.format_map(env_connection_data)


#подключение к базе, если не подключается возможно дело в docker-compose
engine = create_engine('postgresql://postgres:Postgres2019!@postgres:5432/FinanceManagement')

#запрос в бд
df = pd.read_sql_table("Transactions", con=engine)


df_cat_sum = df.groupby('CategoryId')['Amount'].agg('sum')
print(df_cat_sum)