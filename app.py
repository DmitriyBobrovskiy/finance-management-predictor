#импорт модулей
import pandas as pd
import numpy as np
from os import getenv
from sqlalchemy import create_engine
from flask import Flask, jsonify, request

app = Flask(__name__)

@app.route('/getPrediction')
def getPrediction():
	periods_analyze, TransactionTypeId, periods_return, b, k = generate_parameters(request)
	engine = connect_db()
	df, res_pd = create_df(TransactionTypeId, engine)
	predict_list = predict_generator(b, k, df, res_pd,periods_analyze, periods_return)
	return jsonify(predict_list)

def generate_parameters(request):
	periods_analyze = np.int64(request.args.get('periods_analyze', 100))
	periods_return = np.int64(request.args.get('periods_return', 1))
	TransactionTypeId = np.int64(request.args.get('TransactionTypeId', 1))
	b = np.float128(request.args.get('b', 0.1))
	k = np.float128(request.args.get('k', 0.2))
	return(periods_analyze, TransactionTypeId, periods_return, b, k)

def create_df(TransactionTypeId, engine):
	#запрос таблицы в бд
	df = pd.read_sql_table('Transactions', con=engine)

	#отсортированная таблица
	df = df.sort_values('Date')
	df = df.query(f'TransactionTypeId == {TransactionTypeId}')

	#пустая таблица, в последующем будет заполнена
	#и использована как хранилище для анализа пердыдущих значений
	res_pd = pd.DataFrame(columns=['date', 'amount', 'Lt', 'Tt'])
	return(df, res_pd)

def predict_generator(b, k, df, res_pd, periods_analyze, periods_return):
	prev_amount = 0
	#тренд, инициализируется нулем
	t=0
	#сглаженная величина за пердыдущий период
	Lt = df.iloc[0].Amount
	#значение тренда за предыдущий период
	Tt = 0
	predict_list = []
	for row in df.tail(periods_analyze).itertuples():
		date = row.Date
		amount = row.Amount
		Lt = t * amount + (1-k)*(prev_amount - Tt)
		Tt = b*(amount-prev_amount)+(1-b)*Tt
		instance = pd.DataFrame({'date':[date], 'amount':[amount], 'Lt':[Lt], 'Tt':[Tt]})
		res_pd = res_pd.append(instance, ignore_index = True)
		prev_amount = amount

	Lt = res_pd.tail(1).Lt
	Tt = res_pd.tail(1).Tt

	for x in range(periods_return):
		value = x * Tt + Lt
		predict_list.append(float(value))
	return predict_list

#строка подключения из переменных окружения
def connect_db():
	env_connection_data = {
		'database_vendor': getenv('DATABASE_VENDOR'),
		'host': getenv('HOST'),
		'port': getenv('PORT'),
		'user': getenv('USERNAME'),
		'pswd': getenv('PASSWORD'),
		'database': getenv('DATABASE'),
	}

	connection_string = '{database_vendor}://{user}:{pswd}@{host}:{port}/{database}'.format_map(env_connection_data)
	return create_engine(connection_string)

app.run(host='0.0.0.0')
