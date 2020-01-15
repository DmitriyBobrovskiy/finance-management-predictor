
# Run project
### docker-compose.yml

just put this part into your docker-compose.yml as another one service
```
jupyter-notebook:
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
    depends_on: 
      - postgres
    networks: 
      - compose-network
```

# Переменные для коэффициентов

- Lt=k*Yt+(1-k)*(Lt-1-Tt-1), где
- Lt  – сглаженная величина на текущий период;
- k – коэффициент сглаживания ряда;
- Yt – текущие значение ряда (например, объём продаж);
- Lt-1 – сглаженная величина за предыдущий период;
- Tt-1 – значение тренда за предыдущий период;
- Lt = k* Yt+(1-коэффициент сглаживания ряда)*( Lt-1(сглаженная величина за предыдущий период) -Tt-1(тренд за предыдущий период)
                                             

# Прогноз на p периодов вперед равен:

Ŷt+p = Lt + p *Tt, где
- Ŷt+p – прогноз по методу Хольта на p период;
- Lt – экспоненциально сглаженная величина за последний период;
- p – порядковый номер периода, на который делаем прогноз;
- Tt – тренд за последний период.                                             
