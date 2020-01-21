FROM python:3.7
WORKDIR /
EXPOSE 5000

COPY . .
RUN python3 -m pip install --upgrade pip
RUN pip install -r requirements
RUN env FLASK_APP=app.py
CMD python3 app.py