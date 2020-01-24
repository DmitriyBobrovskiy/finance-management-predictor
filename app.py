from pandas import read_sql_table, DataFrame
import numpy as np
from os import getenv
from sqlalchemy import create_engine
from flask import Flask, jsonify, request

app = Flask(__name__)


@app.route('/getPrediction')
def getPrediction():
    periods_analyze, transaction_type_id, periods_return, b, k = generate_parameters(
        request)
    engine = connect_db()
    transactions, analyzed_transactions = create_database_connection(transaction_type_id, engine)
    predict_list = predict_generator(
        b, k, transactions, analyzed_transactions, periods_analyze, periods_return)
    return jsonify(predict_list)


def generate_parameters(request):
    periods_analyze = np.int64(request.args.get('periods_analyze', 100))
    periods_return = np.int64(request.args.get('periods_return', 1))
    transaction_type_id = np.int64(request.args.get('transaction_type_id', 1))
    b = np.float128(request.args.get('b', 0.1))
    k = np.float128(request.args.get('k', 0.2))
    return(periods_analyze, transaction_type_id, periods_return, b, k)


def create_database_connection(transaction_type_id, engine):
    transactions = read_sql_table('Transactions', con=engine)

    transactions = transactions.sort_values('Date')
    transactions = transactions.query(
        f'TransactionTypeId == {transaction_type_id}')

    analyzed_transactions = DataFrame(columns=['date', 'amount', 'Lt', 'Tt'])
    return(transactions, analyzed_transactions)


def predict_generator(b, k, transactions, analyzed_transactions, periods_analyze, periods_return):
    prev_amount = 0
    trend = 0
    # smoothed value for previous period
    Lt = transactions.iloc[0].Amount
    # trend value for previous period
    Tt = 0
    predict_list = []
    for row in transactions.tail(periods_analyze).itertuples():
        date = row.Date
        amount = row.Amount
        Lt = trend * amount + (1-k)*(prev_amount - Tt)
        Tt = b*(amount-prev_amount)+(1-b)*Tt
        instance = DataFrame(
            {'date': [date], 'amount': [amount], 'Lt': [Lt], 'Tt': [Tt]})
        analyzed_transactions = analyzed_transactions.append(instance, ignore_index=True)
        prev_amount = amount

    Lt = analyzed_transactions.tail(1).Lt
    Tt = analyzed_transactions.tail(1).Tt

    for x in range(periods_return):
        value = x * Tt + Lt
        predict_list.append(float(value))
    return predict_list


def connect_db():
    env_connection_data = {
        'database_vendor': getenv('DATABASE_VENDOR'),
        'host': getenv('HOST'),
        'port': getenv('PORT'),
        'user': getenv('USERNAME'),
        'pswd': getenv('PASSWORD'),
        'database': getenv('DATABASE'),
    }

    connection_string = '{database_vendor}://{user}:{pswd}@{host}:{port}/{database}'.format_map(
        env_connection_data)
    return create_engine(connection_string)


app.run(host='0.0.0.0')
