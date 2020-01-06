FROM python:3.7
WORKDIR /
EXPOSE 80
EXPOSE 443

COPY . .
RUN pip install -r requirements
CMD jupyter notebook --ip=0.0.0.0 --port=8080 --allow-root