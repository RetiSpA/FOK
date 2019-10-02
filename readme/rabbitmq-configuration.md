### Enable RabbitMQ Dashboard ###
Eseguire via Terminale il comando (see https://www.codementor.io/bosunbolawa/how-to-enable-rabbitmq-management-interface-owc5lzg7f):
* rabbitmq-plugins enable rabbitmq_management

### Abilitare nuovo utente per la Dashboard ###
Eseguire via terminale i comandi (see https://www.rabbitmq.com/rabbitmqctl.8.html#User_Management https://www.rabbitmq.com/rabbitmqctl.8.html#Access_Control)
* rabbitmqctl add_user fokUser PasswordReti01
* rabbitmqctl set_user_tags fokUser administrator
* rabbitmqctl set_permissions -p / fokUser ".*" ".*" ".*"