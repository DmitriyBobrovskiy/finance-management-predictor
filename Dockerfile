FROM python:3.7
WORKDIR /
EXPOSE 80
EXPOSE 443

COPY . .
RUN pip install -r requirements
CMD python app.py