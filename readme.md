# Build project

### Prepare local environment
```bash
cd <project_root>
pip install -r requirements
```

### Build docker image
Execute next commands
```bash
cd <project_root>
docker build --tag predictor --file Dockerfile .
```

### Change docker compose
Put this code to your backend docker-compose.yml as another one service
```
predictor:
  image: predictor
  environment: 
    DATABASE_VENDOR: "postgresql"
    HOST: "postgres"
    PORT: "5432"
    USERNAME: "postgres"
    PASSWORD: "Postgres2019!"
    DATABASE: "FinanceManagement"
  ports:
    - "5000:5000"
  depends_on: 
    - postgres
  networks: 
    - compose-network
```

# Выполнение запросов

http://0.0.0.0:5000/getPrediction?periods_analyze=31210&transaction_type_id=1&periods_return=3&b=0.4&k=0.4
- periods_analyze - количество строк в бд для анализа
- TransactionTypeId - тип транзакции
- periods_return - ожидаемое количество прогнозов
- b - коэффициент сглаживания ряда
- k - коэффициент сглаживания тренда

Значения по умолчанию:

| Параметр      | Значение |
| --------- | -----:|
| periods_analyze  | 100 |
| TransactionTypeId  | 1 |
| periods_return  | 1 |
| b  | 0.1 |
| k  | 0.2 |

В качестве ответа сервер возвращает массив значений следующего вида:
[640.8439245816938,223.39450942728826,-194.05490572711733]

# Экспоненциально-сглаженный ряд

- Lt=k*Yt+(1-k)*(Lt-1)*(Tt-1), где
- Lt  – сглаженная величина на текущий период;
- k – коэффициент сглаживания ряда;
- Yt – текущие значение ряда (например, объём продаж);
- Lt-1 – сглаженная величина за предыдущий период;
- Tt-1 – значение тренда за предыдущий период;
- Lt = k* Yt+(1-коэффициент сглаживания ряда)*( Lt-1(сглаженная величина за предыдущий период) -Tt-1(тренд за предыдущий период)
                                             
# значение тренда

- Tt=b*(Lt - Lt-1)+(1-b)*Tt-1,  где
- Tt – значение тренда на текущий период;
- b – коэффициент сглаживания тренда;
- Lt – экспоненциально сглаженная величина за текущий период;
- Lt-1 – экспоненциально сглаженная величина за предыдущий период;
- Tt-1 – значение тренда за предыдущий период.

# Прогноз на p периодов вперед равен:

Ŷt+p = Lt + p *Tt, где
- Ŷt+p – прогноз по методу Хольта на p период;
- Lt – экспоненциально сглаженная величина за последний период;
- p – порядковый номер периода, на который делаем прогноз;
- Tt – тренд за последний период.                                             
