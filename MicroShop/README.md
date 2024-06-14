## MicroShop

### Support Containers

- SQL Server 2019 run via:

  ```Powershell
  run --restart=always -e 'ACCEPT_EULA=Y' -e "SA_PASSWORD=$pass" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest        
  ```

- RabbitMQ
  
  ```Powershell
  docker run --restart=always -d --hostname my-rabbit --name some-rabbit -v rabbitmqData:/var/lib/rabbitmq  rabbitmq:3 
  ```
