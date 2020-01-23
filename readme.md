
# Разворачивание проекта
### docker-compose.yml

Добавьте еще один серсвис в docker-compose.yml
```
ds:
  build:
    context: ./ds
    dockerfile: Dockerfile
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

Для сборки контейнерия вы полните код в каталоге с Dockerfile:
docker build -t <image_name> .
где:
- docker build - команда для генерации образа контейнера
- -t - ключ для указания названия имени образа контейнера
- image_name - спецефичное имя образа контейнера
- . - текущий каталог в котором лежит файл Dockerfile

# Выполнение запросов

http://0.0.0.0:5000/getPrediction?periods_analyze=10?TransactionTypeId=1?periods_return=3?b=0.4?k=0.4
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
