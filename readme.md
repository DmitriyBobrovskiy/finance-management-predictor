
#Run project
###docker-compose.yml

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

DATABASE_VENDOR required for orm to specify db.

