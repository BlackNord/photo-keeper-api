Command in cmd for run in debug mode: docker-compose up 

##############################################################

docker_compose_mailslurper_smtp dir:
	- docker-compose.yaml for running in Docker smtp server
	- custom-config.json for configuring mailslurper smtp server

Command for run mailslurper container without docker-compose.yaml: 

docker run -it -p 2500:2500 -p 8080:8080 -p 8085:8085 --rm marcopas/docker-mailslurper

To add authentication on server change field 'credentials' in custom-config.json in way "login": "password_hash"
Or create new user using the command: ./createcredentials

Test credential - "PhotoKeeper": "password"

Smtp by default can be checked on http://localhost:8080

##############################################################

docker_compose_postgresql dir:
	- docker-compose.yaml for running in Docker PostgreSQL and pgAdmin

To configure correct IP-adress of the container the commands below:
	Docker ps
	Docker inspect CONTAINER_ID

Test credential - "admin@pgadmin.com": "password"

PgAdmin by default can be checked on http://localhost:5050

##############################################################

mailslurper-1.14.1-windows dir:
	- contains mailslurper smtp server that can be configuring and set up locally
